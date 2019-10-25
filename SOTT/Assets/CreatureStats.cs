using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] [CreateAssetMenu]
public class CreatureStats : ScriptableObject
{
    public enum DietType //Whether the creature eats meat veg or both
    {
        Herbivore, Carnivore, Omnivore
    }

    List<Creature> _instances = new List<Creature>(); //All the creatures of that species

    public float _maxhealth = 100f;
    public float _speed = 10f;
    [Range(0, 1)] float _hunger = 0f;
    public float _age = 0;
    public float _aggrorange;
    public float _defence;
    public float _attack;
    public float _reproductiveUrge;
    public float _sight;
    /*
    public GameObject GiveBirth()
    {

    }*/

    public bool IsExtinct()
    {
        if(_instances.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
