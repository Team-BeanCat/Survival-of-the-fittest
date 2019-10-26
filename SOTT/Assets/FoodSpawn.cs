using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    public GameObject Tree;

    public Transform Floor;

    public int numTrees;


    void Start()
    {
        float x = Random.Range(-((Floor.localScale.x / 2) * 10), (Floor.localScale.x / 2) * 10);
        float z = Random.Range(-((Floor.localScale.z / 2) * 10), (Floor.localScale.z / 2) * 10);
        Instantiate(Tree, new Vector3(x, 0, z));
    }

}
