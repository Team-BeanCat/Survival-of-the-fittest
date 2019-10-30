using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSpawns : MonoBehaviour
{
    public GameObject Tree;
    public GameObject Rock;
    public GameObject FoodParent;
    public GameObject RockParent;
    public Transform Floor;
    public float x;
    public float z;
    [Range(1, 100)]
    public int numTrees;
    [Range(1, 100)]
    public int numRocks;
<<<<<<< Updated upstream

=======
    public Transform target;
>>>>>>> Stashed changes

    void Start()
    {
        //It works, That's all that matters
        //Don't spend too long trying to comprehend it
        /*
        float x = Random.Range(-((Floor.localScale.x / 2) * 10), (Floor.localScale.x / 2) * 10)*10;
        float z = Random.Range(-((Floor.localScale.z / 2) * 10), (Floor.localScale.z / 2) * 10)*10;
        */


        for (int i = 0; i < numTrees; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * x / 2;
            randomDirection += new Vector3(0, 1, 0);
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, x / 2, 1);
            GameObject TempTree = Instantiate(Tree, new Vector3(navHit.position.x, -1.2f, navHit.position.z), Quaternion.Euler(0f, Random.Range(0f, 359f), 0f));
            TempTree.transform.parent = FoodParent.transform;
            TempTree.transform.localScale = Vector3.one * Random.Range(0.75f, 2f);
            TempTree.name = "Tree" + (i + 1);
        }
        for (int i = 0; i < numRocks; i++)
        {
<<<<<<< Updated upstream
            float xInst = Random.Range(-((x / 2) - 5), (x / 2) - 5);
            float zInst = Random.Range(-((z / 2) - 5), (z / 2) - 5);
            GameObject TempRock = Instantiate(Rock, new Vector3(xInst, -1, zInst), Quaternion.Euler(Random.Range(0f, 359f), Random.Range(0f, 359f), Random.Range(0f, 359f)));
            TempRock.transform.parent = RockParent.transform;
        }
    }
=======
            Vector3 randomDirection = Random.insideUnitSphere * x/2;
            randomDirection += new Vector3(0, 1, 0);
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, x/2, 1);
            GameObject TempRock = Instantiate(Rock, new Vector3(navHit.position.x, -1.2f, navHit.position.z), Quaternion.Euler(Random.Range(0f, 359f), Random.Range(0f, 359f), Random.Range(0f, 359f)));
            TempRock.transform.parent = RockParent.transform;
            TempRock.name = "Rock" + (i + 1);
            
        }
    }
    

    
>>>>>>> Stashed changes
}
