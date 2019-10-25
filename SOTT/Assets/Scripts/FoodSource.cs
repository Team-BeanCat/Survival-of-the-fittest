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
    int _serves = 1;
    public FoodType _foodType;
    public Animator _animator;

    public void Consume(Creature consumer)
    {
        if (_serves > 0)
        {
            if (consumer._creatureStats._dietLock == CreatureStats.DietType.Omnivore)
            {
                consumer._hunger += (_nutritionalValue / 2); //Award Half Value
                Debug.Log(_foodType + " eaten by Omnivore, Half Value");
                Destroy(gameObject);
            }
            else if (consumer._creatureStats._dietLock == CreatureStats.DietType.Carnivore && _foodType == FoodType.Meat)
            {
                consumer._hunger += _nutritionalValue; //Award The Full Value
                Destroy(gameObject);
                Debug.Log(_foodType + " eaten by Carnivore, Full Value");
            }
            else if (consumer._creatureStats._dietLock == CreatureStats.DietType.Herbivore && _foodType == FoodType.Plant)
            {
                consumer._hunger += _nutritionalValue; //Award the full value
                Debug.Log(_foodType + " eaten by Herbivore, Full Value");
                Destroy(gameObject);
            }
            else
            {
                //Award Nothing because it is either a carnivore eating a plant or vice versa
            } 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
}
