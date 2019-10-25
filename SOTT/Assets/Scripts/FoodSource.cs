using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodSource : MonoBehaviour
{
    public enum FoodType
    {
        Meat, Plant
    }

    //The nutritional value of one serve
    float _nutritionalValue = 0.3f;
    int _serves = 3;

    void Consume(Creature consumer)
    {

    }

}
