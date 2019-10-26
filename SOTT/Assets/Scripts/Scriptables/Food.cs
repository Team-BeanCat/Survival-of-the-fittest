using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu][System.Serializable]
public class Food : ScriptableObject
{
    public enum FoodType
    {
        Meat, Plant
    }

    public float _nutritionalValue = 0.3f;
    public FoodType _type = FoodType.Plant;
    public int _serves = 1;
    public float _eatRange = 2f;
}
