using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawn : MonoBehaviour
{
    public GameObject creature;
   
    public GameObject CreatureParent;
    [Range(1,20)]
    public int numPerSpecies;
    public float x = 50;
    public float z = 50;
    bool ValidSpawn; 

    void Start()
    {
        for (int x = 0; x < numPerSpecies; x++)
        {
            ValidSpawn = false;
            while (!ValidSpawn)
            {
                float xInst = Random.Range(-((x / 2) - 5), (x / 2) - 5);
                float zInst = Random.Range(-((z / 2) - 5), (z / 2) - 5);
                Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 2f, 8);
                if (hitColliders.Length == 0)
                {
                    GameObject TempCreature = Instantiate(creature, new Vector3(xInst, -1, zInst), Quaternion.Euler(0f, Random.Range(0f, 359f), 0f));
                    TempCreature.transform.parent = CreatureParent.transform;
                    ValidSpawn = true;
                }
            }
        }
    }
}
