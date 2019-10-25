﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{ 
    CreatureStats _species;
    float health = 100f;
    float maxhealth = 100f;
    float age = 0f;
    float hunger = 100f;

    int segments = 50;
    [Range(0, 100)]
    public float range = 10;

    public CreatureStats creatureStats;

    public GameObject[] FoodsObj;
    public float[] FoodDist;

    public FoodSource targetFood;

    public bool showRanges; // render the sight range of creature

    LineRenderer line; // line renderer for 

    void Start()
    {

        //Line renderer for displaying sight range
        line = gameObject.GetComponent<LineRenderer>();
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        CreatePoints();
    }


    private void Init(float maxhealth)
    {
        
    }

    void Update()
    {


        if (hunger >= 50f) //if hunger is large enough then find a mate
        {
            mate();
        }

        if (health >= maxhealth/2) // if above 50% health attack
        {
            //attack
        }

        if (showRanges) //if show ranges box ticked then enable line renderer
        {
            CreatePoints();
        }
        line.enabled = showRanges;
    }



    public void mate() // ...
    {
        
    }

    public GameObject findMate() // find the nearest mate then return it as a gameObject
    {
        return null;
    }



    bool foodSourceInRange(float range) // checks whether the nearest food source is in range
    {
        if (Vector3.Distance(GetNearestFood().transform.position, transform.position) < range)
        {
            return true;
        }
        else return false;
        
    }


    private FoodSource GetNearestFood() // finds the nearest object with tag food and returns it
    {

        FoodsObj = GameObject.FindGameObjectsWithTag("Food");

        FoodDist = new float[FoodsObj.Length]; 

        for (int i = 0; i < FoodsObj.Length; i++)
        {
            FoodDist[i] = Vector3.Distance(FoodsObj[i].transform.position, transform.position);
        }
        targetFood = FoodsObj[MinDistance(FoodDist)].GetComponent<FoodSource>();
        return targetFood;

    }
    
    private int MinDistance(float[] Dist) // takes an array and returns the location of that number
    {
        float min = Dist[0];
        int minLoc = 0;
        for (int i = 0; i < Dist.Length; i++)
        {
            if (min > Dist[i])
            {
                minLoc = i;
                min = Dist[i];
            }
        }
        return minLoc; 

    }
    void CreatePoints() // renders the range
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * range;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * range;

            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / segments);
        }
    }
}
