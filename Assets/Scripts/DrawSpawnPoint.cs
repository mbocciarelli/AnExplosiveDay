using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSpawnPoint : MonoBehaviour
{
    [SerializeField] private bool activeVisual;
    [SerializeField] private float radius = 10f;

    [SerializeField] private List<GameObject> SpawnPoints;
    
    

    private void OnDrawGizmos()
    {
        if (activeVisual)
        {
            foreach (var o in SpawnPoints)
            {
                if (o != null)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(o.transform.position, radius);
                }
            }
        }
    }
}
 