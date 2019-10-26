using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    public GameObject Tree;
    public GameObject FoodParent;
    public Transform Floor;
    public float x = 50;
    public float z = 50;
    [Range(1,100)]
    public int numTrees;


    void Start()
    {
        //It works, That's all that matters
        //Don't spend too long trying to comprehend it
        /*
        float x = Random.Range(-((Floor.localScale.x / 2) * 10), (Floor.localScale.x / 2) * 10)*10;
        float z = Random.Range(-((Floor.localScale.z / 2) * 10), (Floor.localScale.z / 2) * 10)*10;
        */

        
        for (int i = 0; i <= numTrees; i++)
        {
            float xInst = Random.Range(-((x / 2)-5), (x / 2)-5);
            float zInst = Random.Range(-((z / 2)-5), (z / 2)-5);
            GameObject TempTree = Instantiate(Tree, new Vector3(xInst, -1, zInst), Quaternion.Euler(0f, Random.Range(0f, 359f), 0f));
            TempTree.transform.parent = gameObject.transform;
        }
    }
   
     
}
