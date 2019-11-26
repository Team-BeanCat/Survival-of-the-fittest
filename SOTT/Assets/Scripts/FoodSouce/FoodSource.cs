using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodSource : MonoBehaviour
{
    

    //The nutritional value of one serve
    public Food _foodStats;
    public Animator _animator;
    //[HideInInspector]
    public int _servesRemaining;
    public Creature _approachingCreature; //The Creature Currently Approaching this food to eat it

    private void Awake()
    {
        _servesRemaining = _foodStats._serves;
    }

    public virtual void Consume(Creature consumer)
        {
        if (_foodStats._serves > 0)
        {
            if (consumer._creatureStats._dietLock == CreatureGenome.DietType.Omnivore)
            {
                consumer._sustinance += (_foodStats._nutritionalValue / 2); //Award Half Value
                Debug.Log(gameObject.name + " (" + _foodStats._type + ") eaten by Omnivore, Half Value");
                OnEat();
            }
            else if (consumer._creatureStats._dietLock == CreatureGenome.DietType.Carnivore && _foodStats._type == Food.FoodType.Meat)
            {
                consumer._sustinance += _foodStats._nutritionalValue; //Award The Full Value
                Debug.Log(gameObject.name + " (" + _foodStats._type + ") eaten by Carnivore, Full Value");
                OnEat();
            }
            else if (consumer._creatureStats._dietLock == CreatureGenome.DietType.Herbivore && _foodStats._type == Food.FoodType.Plant)
            {
                consumer._sustinance += _foodStats._nutritionalValue; //Award the full value
                Debug.Log(gameObject.name + " (" + _foodStats._type + ") eaten by Herbivore, Full Value");
                OnEat();
            }
            else
            {
                //Award Nothing because it is either a carnivore eating a plant or vice versa
            } 

            //Ensure Sustinance remains below 100
            if (consumer._sustinance > 100f)
            {
                consumer._sustinance = 100f;
            }
        }
        else
        {
            //OnEat();
        }
    }

    public virtual void OnEat()
    {
        Debug.Log("Food Eaten");
        _servesRemaining--;
        _approachingCreature = null; //Remove reservation
    }

}
