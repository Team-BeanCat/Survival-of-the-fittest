using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureSpawn : MonoBehaviour
{
    public GameObject creature;
   
    public GameObject CreatureParent;
    [Range(1,20)]
    public int numPerSpecies;
    public float x;
    public float z;
    bool ValidSpawn; 

    void Start()
    {
        for (int i = 0; i < numPerSpecies; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * x / 2;
            randomDirection += new Vector3(0, 1, 0);
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, x / 2, 1);
            GameObject TempRock = Instantiate(creature, new Vector3(navHit.position.x, 1, navHit.position.z), Quaternion.identity);
            TempRock.transform.parent = CreatureParent.transform;
            TempRock.name = "creature" + (i + 1);
        }
    }
}
