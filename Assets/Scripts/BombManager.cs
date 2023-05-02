using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BombManager : MonoBehaviour
{
    private int _numberOfVictims = 0;
    //Debug circle

    
    
    [SerializeField] private GameObject player;
    [SerializeField] private float radius = 5;
    [SerializeField] private float power = 400;
    [SerializeField] private ParticleSystem explosionParticles = null;
    private GameManager _gm;
    public bool _exploded = false;
    
    [SerializeField] private GameObject parentExplosions;
    [SerializeField] private GameObject explosionPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && !_exploded && !_gm.inMenu)
        {
            if (_gm.isTuto)
            {
                if(_gm.isPossibleToExplodeInTuto)
                    Explosion();
            }
            else
            {
                Explosion();
            }
        }
     
    }
    
    void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    
    public void Explosion()
    {
       // Debug.Log("BOOM");
        Vector3 explosionPos = new Vector3(transform.position.x, transform.position.y,transform.position.z);
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        if (!_gm.isTuto)
        {
            Vector3 positionPlayer = (Vector3) GameObject.FindGameObjectsWithTag("Player")[0]?.transform.position;
            if (positionPlayer != null)
            {
                GameObject o = Instantiate(explosionPrefab, parentExplosions.transform);
                o.transform.position = new Vector3(positionPlayer.x, o.transform.position.y, positionPlayer.z);
            }
        }

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                if (!rb.isKinematic)
                {
                    rb.constraints = RigidbodyConstraints.None;
                    rb.AddExplosionForce(power,explosionPos, radius,7.0f);
                    _numberOfVictims += 1;
                }
            }

            
            
            //Play particles :
            explosionParticles.Play();
            _exploded = true;
        }
        if (_numberOfVictims == 0)
        {
            if (!_gm.isTuto)
            {
                _gm.Win();
            }
            else
            {
                _gm.EndTuto();
            }
            Debug.Log("VICTIMS : "+ _numberOfVictims );
        }
        else
        {
            _gm.Lose();
            Debug.Log("VICTIMS Lose : "+ _numberOfVictims );
        }
       // Debug.Log("Nombre de victims : " + (_numberOfVictims));
        _numberOfVictims = 0;
    }


   
}
