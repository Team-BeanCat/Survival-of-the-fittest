using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI; //For Navmesh
using UnityEngine.UI; //For Sliders
using TMPro;

public class Creature : MonoBehaviour
{
    //What action the creature is currently taking
    enum State
    {
        FoodSearch, EatingFood, Idle, MateSearch, WaterSearch //, MovingToKnownFood
    }

    public FoodSource[] _allFood; //All the food in the scene (that are edible by this creature)
    public List<float> _foodDist = new List<float>(); //Distances To every valid foodsource in the scene
    public FoodSource targetFood; //Food Currently being Pursued
    public bool showRanges; // render the sight range of creature
    public CreatureStats _creatureStats;
    [Range(0, 100f)]
    public float _sustinance = 100f;
    Transform _foodParent;


    [SerializeField] private State _currentState = State.Idle;
    private float _health = 100f;
    private float _maxhealth = 100f;
    private float _age = 0f;
    
    private int _segments = 50; //Segments for sight field line
    private LineRenderer line; // line renderer for 
    private NavMeshAgent _agent; //Nav mesh agent
    Vector3 newDest = new Vector3();

    public List<FoodSource> knownFood;
    public List<float> knownFoodDist;

    [Header("Stat Sliders")]
    public Slider _healthSlider;
    public Slider _foodSlider;
    public TextMeshProUGUI _status;



    void Start()
    {
        //Get All Food
        _foodParent = GameObject.Find("FoodContainer").transform;

        //Line renderer for displaying sight range
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount =_segments + 1;
        line.useWorldSpace = false;
        CreatePoints();
        if (showRanges) //if show ranges box ticked then enable line renderer
        {
            CreatePoints();
        }
        line.enabled = showRanges;
        _agent = GetComponent<NavMeshAgent>();
        _sustinance = 80f;
}

    void FixedUpdate()
    {
        _sustinance -= 0.02f;

        _healthSlider.value = _health;
        _foodSlider.value = _sustinance;
        _status.text = _currentState.ToString();

        #region RefreshDistances
        _foodDist.Clear(); //Wipe the list clean
        _allFood = FindObjectsOfType<FoodSource>();

        for (int i = 0; i < _allFood.Length; i++)
        {
            //FOOD TYPE FILTERING - UBERJANK (try not to think about it too hard)
            if (_allFood[i]._servesRemaining > 0 && _creatureStats._dietLock == CreatureStats.DietType.Omnivore) //If this creature is an omnivore then just go for it!
            {
                //Debug.Log("foodtype");
                _foodDist.Add(Vector3.Distance(_allFood[i].transform.position, transform.position));
            }

            //Carnivore
            else if (_allFood[i]._servesRemaining > 0 && _allFood[i]._foodStats._type == Food.FoodType.Meat && _creatureStats._dietLock == CreatureStats.DietType.Carnivore)
            {
                //Debug.Log("foodtype");
                _foodDist.Add(Vector3.Distance(_allFood[i].transform.position, transform.position));
            }

            //Herbivore
            else if (_allFood[i]._servesRemaining > 0 && _allFood[i]._foodStats._type == Food.FoodType.Plant && _creatureStats._dietLock == CreatureStats.DietType.Herbivore)
            {
                //Found possible food
                //Debug.Log("foodtype");
                _foodDist.Add(Vector3.Distance(_allFood[i].transform.position, transform.position));
            }
        } 
        #endregion
    }

    void Update()
    {
        //if (hunger >= 50f) //if hunger is large enough then find a mate
        //{
        //    mate();
        //}

        //FindNearestKnownFood();

        //If The Hunger is below 75% then go eat
        if (_sustinance < 75f)
        {
            //Debug.Log(gameObject + " Is hungry");
            _currentState = State.FoodSearch; //Set the state to searching for food

            //If the creature has found food
            if (foodSourceInRange() == true)
            {
                targetFood = GetNearestFood();

                //Debug.Log("Found Food!");
                _currentState = State.EatingFood; //Update State
                Vector3 FoodLocation = targetFood.transform.position;
                //Check if the nearest food source is within eating range

                NavMeshHit hit;
                NavMesh.SamplePosition(FoodLocation, out hit, 5f, 1);
                float dist = Vector3.Distance(hit.position, transform.position);
                if (dist < 1.5*targetFood.transform.localScale.x) //Eat range dependant on object size
                {
                    ////Debug.Log("Eating Food");
                    //if (!(knownFood.Contains(GetNearestFood())))
                    //{
                    //    knownFood.Add(GetNearestFood());
                    //}
                    targetFood.Consume(this); //Eat the food
                    targetFood = null;
                    _agent.SetDestination(transform.position); //Trigger the wander function
                }
                else
                {
                    //Go towards it
                    _agent.SetDestination(FoodLocation); //Find the nearest food (will be within range) and head towards it 
                }
            }

            //Was an else if IF BROKEN CHANGE IT BACK
            if (Vector3.Distance(_agent.destination, transform.position) < 1.5) //Check if the Creature is at the pathfinder's destination
            {
                //Debug.Log("Searching");
                //Get a random point in a circle with a radius equal to the creature's sight * 2
                newDest = RandomNavSphere(transform.position, _creatureStats._sight * 2); //Pick a random point within the sight range
                //Debug.Log("Heading To " + newDest);

                //Set that as the destination for the Agent
                _agent.SetDestination(newDest);

            }
            //Debug.Log(Vector3.Distance(_agent.destination, transform.position));
            if (_sustinance<5f)
            {
                Debug.Log(gameObject + " has died");
                CameraControl._instance.creatures.Remove(transform); //Remove this from the camera controller targets
                Destroy(gameObject);
            }

        }
        else
        {
            _currentState = State.Idle;
        }

        //If above 50% health attack and is either omnivorou or carnivorous
        if (_health >= _maxhealth/2 && _creatureStats._dietLock == CreatureStats.DietType.Carnivore || _creatureStats._dietLock == CreatureStats.DietType.Omnivore) 
        {
            //attack
        }

        
    }

    public Creature findNearestMate() // find the nearest mate then return it as a gameObject
    {
        return null;
    }
    /*
    Transform FindNearestKnownFood()
    {
        knownFoodDist.Clear();
        for (int i = 0; i < knownFood.Count; i++)
        {
            knownFoodDist.Add(Vector3.Distance(knownFood[i].gameObject.transform.position, transform.position));
        }
        
    //    int Loc = MinDistance(knownFoodDist);

        return knownFood[Loc].transform;
    }*/

    bool foodSourceInRange() // checks whether the nearest food source is in range
    {
        FoodSource nearest = GetNearestFood();
        if (Vector3.Distance(nearest.transform.position, transform.position) <= _creatureStats._sight && nearest._servesRemaining > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }


    private FoodSource GetNearestFood() // finds the nearest object with tag food and returns it
    {
        _foodDist.Clear(); //Wipe the list clean
        _allFood = FindObjectsOfType<FoodSource>();

        FoodSource nearest;

        for (int i = 0; i < _allFood.Length; i++)
        {
            //FOOD TYPE FILTERING - UBERJANK (try not to think about it too hard)
            if (_allFood[i]._servesRemaining > 0 && _creatureStats._dietLock == CreatureStats.DietType.Omnivore) //If this creature is an omnivore then just go for it!
            {
                //Debug.Log("foodtype");
                _foodDist.Add(Vector3.Distance(_allFood[i].transform.position, transform.position));
            }

            //Carnivore
            else if (_allFood[i]._servesRemaining > 0 && _allFood[i]._foodStats._type == Food.FoodType.Meat && _creatureStats._dietLock == CreatureStats.DietType.Carnivore)
            { 
                //Debug.Log("foodtype");
                _foodDist.Add(Vector3.Distance(_allFood[i].transform.position, transform.position));
            }

            //Herbivore
            else if (_allFood[i]._servesRemaining > 0 && _allFood[i]._foodStats._type == Food.FoodType.Plant && _creatureStats._dietLock == CreatureStats.DietType.Herbivore)
            {
                //Found possible food
                //Debug.Log("foodtype");
                _foodDist.Add(Vector3.Distance(_allFood[i].transform.position, transform.position));
            }
        }
        nearest = _allFood[MinDistance(_foodDist)].GetComponent<FoodSource>();
        return nearest;

    }
    
    private int MinDistance(List<float> distances) // takes an array and returns the location of that number
    {
        float minVal = distances.Min(); //F
        int index = distances.IndexOf(minVal);
        return index;
    }

    void CreatePoints() // renders the range
    {
        float x;
        float z;

        float angle = 20f;
        //it works, just don't think about it to much
        for (int i = 0; i < (_segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * _creatureStats._sight;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * _creatureStats._sight;

            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / _segments);
        }
    }

    //Generate wander destination
    public static Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        //Debug.Log("Generating a random point!");

        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, 2, 1);

        return navHit.position;
        //return randDirection;
    }
}
