using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class PnjController : MonoBehaviour
{

    private bool _randomRun = false;
    [SerializeField]
    public bool forcedontmove = false;
    
    [Header("Speed Categorie")]
    [SerializeField] private float RangeMinSpeed;
    [SerializeField] private float RangeMaxSpeed;
    
    private float speed = 5f;

    [SerializeField] private float _distanceRayCast = 6f;
    [SerializeField] private float _distanceRayCastDiago = 4f;
    

    [Header("Update Movement Categorie")]
    [SerializeField] private float RangeMinUpdate;
    [SerializeField] private float RangeMaxUpdate;

    private float nextUpdateTo;
    private float nextUpdateIn;
    private Vector3 velocity;

    private Rigidbody rb;
    private GameObject player;
    [Header("Intervalle Movement")] 
    [SerializeField] private float intervalleMovement = 0.3f;
    private Intervalle rangeX;
    private Intervalle rangeZ;

    private bool _hitWall = false;
    
    private Animator anim;
    public bool inRun;
    
    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        nextUpdateTo = Random.Range(RangeMinUpdate, RangeMaxUpdate);
        nextUpdateIn = nextUpdateTo;
        rb = GetComponent<Rigidbody>();

        rangeX = new Intervalle(-1f, 1f);
        rangeZ = new Intervalle(-1f, 1f);

        speed = Random.Range(RangeMinSpeed, RangeMaxSpeed);
        anim = GetComponentInChildren<Animator>();
        _randomRun = (Random.value < 0.5);
    }

    private void OnDisable()
    {
        inRun = false;
    }

    private void OnEnable()
    {
        if(!forcedontmove)
            ChangeVelocity(rangeX, rangeZ);
    }

 
    private void Update()
    {
        
        if (rb.constraints == RigidbodyConstraints.None)
        {
           
            forcedontmove = true;
            Idle();
        }

        if (forcedontmove && rb.constraints != RigidbodyConstraints.None)
        {
            Idle();
            return;
        }

            RaycastHit hitForward, hitRight, hitLeft, hitRightForward, hitLeftForward;
        
        Ray rayForward = new Ray(gameObject.transform.position + Vector3.up, gameObject.transform.forward+ Vector3.up);
        Ray rayRight = new Ray(gameObject.transform.position+ Vector3.up, gameObject.transform.right+ Vector3.up);
        Ray rayLeft = new Ray(gameObject.transform.position+ Vector3.up, -gameObject.transform.right+ Vector3.up);
        Ray rayRightForward = new Ray(gameObject.transform.position+ Vector3.up, gameObject.transform.forward + gameObject.transform.right+ Vector3.up);
        Ray rayLeftForward = new Ray(gameObject.transform.position+ Vector3.up, gameObject.transform.forward - gameObject.transform.right+ Vector3.up);
        
        if (nextUpdateIn >= nextUpdateTo)
        {
            ChangeVelocity(rangeX, rangeZ);
        }
        
        bool rayForwardCheck = Physics.Raycast(rayForward, out hitForward, _distanceRayCast), 
            rayRightCheck = Physics.Raycast(rayRight, out hitRight, _distanceRayCast), 
            rayLeftCheck = Physics.Raycast(rayLeft, out hitLeft, _distanceRayCast), 
            rayRightForwardCheck = Physics.Raycast(rayRightForward, out hitRightForward, _distanceRayCastDiago), 
            rayLeftForwardCheck = Physics.Raycast(rayLeftForward, out hitLeftForward, _distanceRayCastDiago);

        
           // CheckChangementDirection(rayForwardCheck, hitForward, rayRightCheck, hitRight, rayLeftCheck, hitLeft, rayRightForwardCheck, hitRightForward, rayLeftForwardCheck, hitLeftForward);
        
        rb.velocity = velocity;

        nextUpdateIn += Time.deltaTime;
    }

    private void CheckChangementDirection(
        bool rayForwardCheck, RaycastHit hitForward,
        bool rayRightCheck, RaycastHit hitRight,
        bool rayLeftCheck, RaycastHit hitLeft, 
        bool rayRightForwardCheck, RaycastHit hitRightForward, 
        bool rayLeftForwardCheck, RaycastHit hitLeftForward)
    {
        
        bool directionFind = false;

        
        
        //Check if pnj detect an other pnj with all ray
        if (rayForwardCheck && rayRightCheck && rayLeftCheck)
        {
            RaycastHit hitBackward;

            Ray rayBackward = new Ray(gameObject.transform.position, -gameObject.transform.forward);

            if (Physics.Raycast(rayBackward, out hitBackward, _distanceRayCast))
            {
                rangeX.ChangeIntervalle(0,0);
                rangeZ.ChangeIntervalle(0,0);
                directionFind = true;
            }
            else
            {
                rangeX.ChangeIntervalle(-gameObject.transform.forward.x,-gameObject.transform.forward.x);
                rangeZ.ChangeIntervalle(-gameObject.transform.forward.z,-gameObject.transform.forward.z);
                directionFind = true;
            }
        }
        
        //Check if pnj detect an other pnj
        List<Direction> listCheck = new List<Direction> { Direction.Forward, Direction.Right, Direction.Left, Direction.Right_Forward, Direction.Left_Forward };
        Direction direction;
        int sizeList = listCheck.Count;

        while (!directionFind && sizeList > 0)
        {
            int index = Random.Range(0, sizeList - 1);
            direction = listCheck[index];
            directionFind = CheckRaycast(direction, rayForwardCheck, hitForward, rayRightCheck, hitRight, rayLeftCheck, hitLeft, rayRightForwardCheck, hitRightForward, rayLeftForwardCheck, hitLeftForward);
            listCheck.Remove(direction);
            sizeList--;
        }

        if (directionFind)
        {
            ChangeVelocity(rangeX, rangeZ);
        }
        else
        {
            rangeX.ChangeIntervalle(-1f, 1f);
            rangeZ.ChangeIntervalle(-1f, 1f);
        }

        // Debug.DrawRay(gameObject.transform.position+ Vector3.up, gameObject.transform.forward * 4 +  Vector3.up);
        // Debug.DrawRay(gameObject.transform.position+ Vector3.up, gameObject.transform.right * 4 + Vector3.up);
        // Debug.DrawRay(gameObject.transform.position+ Vector3.up, -gameObject.transform.right * 4+ Vector3.up);
        // Debug.DrawRay(gameObject.transform.position+ Vector3.up, (gameObject.transform.forward + gameObject.transform.right) * 4+ Vector3.up);
        // Debug.DrawRay(gameObject.transform.position+ Vector3.up, (gameObject.transform.forward - gameObject.transform.right) * 4 + Vector3.up);
    }

    private bool CheckRaycast(Direction direction, bool rayForwardCheck, RaycastHit hitForward, bool rayRightCheck, RaycastHit hitRight,
        bool rayLeftCheck, RaycastHit hitLeft, bool rayRightForwardCheck, RaycastHit hitRightForward,
        bool rayLeftForwardCheck, RaycastHit hitLeftForward)
    {
        bool directionFind = false;
        
        if (direction == Direction.Forward && rayForwardCheck)
        {
         
            directionFind = DirectionFind(hitForward);
        }

        if (direction == Direction.Right && rayRightCheck)
        {
            directionFind = DirectionFind(hitRight);
        }

        if (direction == Direction.Left && rayLeftCheck)
        {
            directionFind = DirectionFind(hitLeft);
        }

        if (direction == Direction.Right_Forward && rayRightForwardCheck)
        {
            directionFind = DirectionFind(hitRightForward);
        }

        if (direction == Direction.Left_Forward && rayLeftForwardCheck)
        {
            directionFind = DirectionFind(hitLeftForward);
        }

        return directionFind;
    }

    private bool DirectionFind(RaycastHit hitForward)
    {
        Vector3 direction;
        
        direction = -(gameObject.transform.position - hitForward.transform.position).normalized;

        return CalculateIntervalle(direction);
    }

    private void ChangeVelocity(Intervalle intervalleX, Intervalle intervalleZ)
    {
        if (intervalleX != null && intervalleZ != null)
        {
            float x;
            float z;

            if (intervalleX.nb == 2)
            {
                x = Random.Range(intervalleX.Intervalles["min1"], intervalleX.Intervalles["max1"]);
            }
            else
            {
                var candidates = new[] {Random.Range(intervalleX.Intervalles["min1"], intervalleX.Intervalles["max1"]), Random.Range(intervalleX.Intervalles["min2"], intervalleX.Intervalles["max2"])};
                var idx = Random.Range(0,2); // this should give you either 0 or 1
                x = candidates[idx];
            }
            
            if (intervalleZ.nb == 2)
            {
                z = Random.Range(intervalleZ.Intervalles["min1"], intervalleZ.Intervalles["max1"]);
            }
            else
            {
                var candidates = new[] {Random.Range(intervalleZ.Intervalles["min1"], intervalleZ.Intervalles["max1"]), Random.Range(intervalleZ.Intervalles["min2"], intervalleZ.Intervalles["max2"])};
                var idx = Random.Range(0,2); // this should give you either 0 or 1
                z = candidates[idx];
            }

            Vector3 dir = new Vector3(x, 0, z);

            if (dir != Vector3.zero && rb.constraints != RigidbodyConstraints.None)
            {
                transform.forward = dir;

                if (!inRun)
                {
                    
                    Run();
                    inRun = true;
                }
            }
            else
            {
                if (inRun)
                {
                    Idle();
                    inRun = false;
                }
            }

            Vector3 forward = dir.normalized * speed;
            velocity = forward;
        
            nextUpdateIn = 0f;
        }
    }

    private bool CalculateIntervalle(Vector3 direction)
    {
        float xMinNotMove = direction.x - intervalleMovement, 
            xMaxNotMove = direction.x + intervalleMovement, 
            zMinNotMove = direction.z - intervalleMovement, 
            zMaxNotMove = direction.z + intervalleMovement;

        if (xMinNotMove <= -1f)
        {
            rangeX.ChangeIntervalle(xMaxNotMove, 1f);
        } 
        else if (xMaxNotMove >= 1f)
        {
            rangeX.ChangeIntervalle(-1f, xMinNotMove);
        }
        else
        {
            rangeX.ChangeIntervalle(-1f, xMinNotMove, xMaxNotMove, 1f);
        }
        
        if (zMinNotMove <= -1f)
        {
            rangeZ.ChangeIntervalle(zMaxNotMove, 1f);
        } 
        else if (zMaxNotMove >= 1f)
        {
            rangeZ.ChangeIntervalle(-1f, zMinNotMove);
        }
        else
        {
            rangeZ.ChangeIntervalle(-1f, zMinNotMove, zMaxNotMove, 1f);
        }

        return true;
    }
    
    public void Idle()
    {
        if(anim != null)
            anim.SetFloat("Blend",0);
    }
    
    public void Run()
    {
        if (anim != null)
        {
            if (_randomRun)
            {
                anim.SetFloat("Blend",0.5f);
            }
            else
            {
                anim.SetFloat("Blend",1f);
            }
        }
        
    }
    
    
   
}

public class Intervalle
{
    public IDictionary<String, float> Intervalles;
    public int nb;

    public Intervalle(float min, float max)
    {
        Intervalles = new Dictionary<string, float>();
        nb = 2;
        Intervalles.Add("min1", min);
        Intervalles.Add("max1", max);
    }

    public void ChangeIntervalle(float min, float max)
    {
        
        Intervalles.Clear();
        nb = 2;
        Intervalles.Add("min1", min);
        Intervalles.Add("max1", max);
    }
    
    public void ChangeIntervalle(float min1, float max1, float min2, float max2)
    {
        Intervalles.Clear();
        nb = 4;
        Intervalles.Add("min1", min1);
        Intervalles.Add("max1", max1);
        Intervalles.Add("min2", min2);
        Intervalles.Add("max2", max2);
    }

  

   
}

public enum Direction
{
    Forward,
    Right,
    Left,
    Right_Forward,
    Left_Forward,
}
