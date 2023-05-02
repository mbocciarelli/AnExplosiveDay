using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PnjManager : MonoBehaviour
{
    [Header("SpawnPoint")] 
    [SerializeField] private List<GameObject> SpawnPoint;
    [SerializeField] private List<int> numberPnjToSpawnPerSpawnPoint;
    
    [SerializeField] private bool activePnj;

    [Header("Visual Spawn Points")]
    [SerializeField] private bool activeVisual;

    [SerializeField] private bool desactiveVisualGrandes;
    [SerializeField] private bool desactiveVisualMoyennes;
    [SerializeField] private bool desactiveVisualIntermediaires;
    [SerializeField] private bool desactiveVisualPetites;

    [Header("Skin Pnj")] 
    [SerializeField] private List<GameObject> PrefabsPnj;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPnjs(SpawnPoint);
    }
    

    private void SpawnPnjs(List<GameObject> SpawnPoint)
    {
        for (int index = 0; index < SpawnPoint.Count; index++)
        {
            var o = SpawnPoint[index];

            int numberPnjToSpawn;
            if (o.name.Contains("Grande"))
            {
                numberPnjToSpawn = numberPnjToSpawnPerSpawnPoint[0];
            } 
            else if (o.name.Contains("Moyenne"))
            {
                numberPnjToSpawn = numberPnjToSpawnPerSpawnPoint[1];
            } 
            else if (o.name.Contains("Intermediaire"))
            {
                numberPnjToSpawn = numberPnjToSpawnPerSpawnPoint[2];
            }
            else
            {
                numberPnjToSpawn = numberPnjToSpawnPerSpawnPoint[3];
            }
            
            #if UNITY_EDITOR

            if (!activePnj)
            {
                numberPnjToSpawn = 0;
            }
            
            #endif

            for (int numberPnjSpawn = 0; numberPnjSpawn < numberPnjToSpawn; numberPnjSpawn++)
            {
                int indexPnj = Random.Range(0, PrefabsPnj.Count);
        
                GameObject pnj = Instantiate(PrefabsPnj[indexPnj]);

                float x = Random.Range(o.transform.position.x - o.transform.localScale.x / 2,
                    o.transform.position.x + o.transform.localScale.x / 2);
                float z = Random.Range(o.transform.position.z - o.transform.localScale.z / 2,
                    o.transform.position.z + o.transform.localScale.z / 2);
        
                pnj.transform.position = new Vector3(x, o.transform.position.y, z);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (activeVisual)
        {
            foreach (var o in SpawnPoint)
            {
                if (o != null)
                {
                    if (!desactiveVisualGrandes && o.name.Contains("Grande"))
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(o.transform.position, o.transform.localScale);
                    } 
                    else if (!desactiveVisualMoyennes && o.name.Contains("Moyenne"))
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawCube(o.transform.position, o.transform.localScale);
                    }
                    else if (!desactiveVisualIntermediaires && o.name.Contains("Intermediaire"))
                    {
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawCube(o.transform.position, o.transform.localScale);
                    } 
                    else if (!desactiveVisualPetites && o.name.Contains("Petite"))
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawCube(o.transform.position, o.transform.localScale);
                    }   
                }
            }
        }
    }
}
