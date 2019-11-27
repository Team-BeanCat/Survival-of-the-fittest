using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] [CreateAssetMenu]
public class CreatureGenome : ScriptableObject
{
    public enum DietType //Whether the creature eats meat veg or both
    {
        Herbivore, Carnivore, Omnivore
    }

    List<Creature> _instances = new List<Creature>(); //All the creatures of that species

    [Header("Stats")]

    public float _maxhealth = 100f;
    public float _speed = 10f;
    public DietType _dietLock = DietType.Omnivore;

    public float _age = 0;
    public float _aggrorange;
    public float _defence;
    public float _attack;
    public float _reproductiveUrge = 0.3f;
    public float _sight = 5;
    public float _reproductioncooldown = 60f;

    [Header("Mutations")]

    const float _mutationChance = 0.2f;
    const float _maxMutationAmount = 0.3f;
    const float _reproductionspeedpenalty = 5;

    CreatureGenome RandomGenome()
    {
        CreatureGenome newGenes = new CreatureGenome();

        float mod = Random.Range(_maxMutationAmount, -_maxMutationAmount);
        newGenes._maxhealth += mod * 100;
        newGenes._speed += mod * 10;

        mod = Random.Range(_maxMutationAmount, -_maxMutationAmount);
        newGenes._reproductioncooldown += mod * 60;

        return newGenes;
    }

    /*
    public GameObject GiveBirth()
    {

    }*/

    //public CreatureGenome CalculateChildGenome ()
    //{

    //}

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
