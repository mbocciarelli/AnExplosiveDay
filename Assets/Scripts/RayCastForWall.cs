using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastForWall : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private PnjController _pnj;
    public GameObject _pnjGameObject;
    public Transform _pnjTransform;

    public bool activeRayCast = true;

    private float distance = 0;

    private float distanceAffichPnj = 70f;
   //public Transform _pnjParentTransform;
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        _pnj = FindObjectOfType<PnjController>();
      


    }

    // Update is called once per frame
    void Update()
    {
        if (!_pnj.forcedontmove)
        {
            IsProtectedByWall();
            IsInCameraView();
        }
        
        transform.position = _pnjTransform.position;
    }

    void IsProtectedByWall()
    {
        RaycastHit hit;
       // float distance = Vector3.Distance(transform.position, player.transform.position);

     
        Ray myRay = new Ray(transform.position, player.transform.position - transform.position );

        if (Physics.Raycast(myRay, out hit))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                if (_pnjGameObject.GetComponent<Rigidbody>().velocity == Vector3.zero)
                {
                    _pnjGameObject.GetComponent<Rigidbody>().isKinematic = true;
                }
                
            }
            else if(hit.collider.CompareTag("Player"))
            {
                _pnjGameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        if (distance < distanceAffichPnj)
        {
            Debug.DrawRay (transform.position, player.transform.position - transform.position );
        }
       
    }
    
    
    private void IsInCameraView()
    {
        
      distance = Vector3.Distance(transform.position, player.transform.position);
       
        if (distance < distanceAffichPnj)
        {
            _pnjGameObject.SetActive(true);
        }
        else
        {
            if(activeRayCast)
                _pnjGameObject.SetActive(false);
        }
    }
    
    
    
}
