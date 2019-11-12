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

    //Is the food compatable with the diet type
    bool DietMatchQuery(CreatureStats.DietType diet, Food.FoodType foodtype)
    {
        if (diet == CreatureStats.DietType.Omnivore)                                            //Omnivore? 
        {
            return true;
        }
        else if (diet == CreatureStats.DietType.Carnivore && foodtype == Food.FoodType.Meat)    //Carnivore and Meat?
        {
            return true;
        }
        else if (diet == CreatureStats.DietType.Herbivore && foodtype == Food.FoodType.Plant)   //Herbivore and Plant?
        {
            return true;
        }
        else //Can't Be edible
        {
            return false; 
        }
    }

    public List<FoodSource> _allFood = new List<FoodSource>(); //All the food in the scene (that are edible by this creature)
    //public List<float> _foodDist = new List<float>(); //Distances To every valid foodsource in the scene
    public Transform targetFood; //Food Currently being Pursued
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

    //public List<FoodSource> knownFood;
    //public List<float> knownFoodDist;

    [Header("Stat Sliders")]
    public Slider _healthSlider;
    public Slider _foodSlider;
    public TextMeshProUGUI _status;



    void Start()
    {

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

        //Take damage if starving
        if (_sustinance < 5f)
        {
            _health -= 0.05f;
        }

        //"Die if dead" - Ben McGaw 10/11/2019
        if (_health < 0)
        {
            Kill();
        }

        //If The Hunger is below 75% then go eat
        if (_sustinance < 75f)
        {
            //Debug.Log(gameObject + " Is hungry");
            _currentState = State.FoodSearch; //Set the state to searching for food

            //If the creature has found food
            if (FoodSense())
            {
            FoodSense(targetFood, out targetFood); //Run overlapsphere again with output
                //Debug.Log("Found Food!");
                _currentState = State.EatingFood; //Update State
                Vector3 FoodLocation = Vector3.zero;

                //Check if the nearest food source is within eating range
                NavMeshHit hit;
                NavMesh.SamplePosition(FoodLocation, out hit, 5f, 1);
                float dist = Vector3.Distance(hit.position, transform.position);
                if (dist < 1.5 * targetFood.transform.localScale.x) //Eat range dependant on object size
                {
                    ////Debug.Log("Eating Food");
                    //if (!(knownFood.Contains(GetNearestFood())))
                    //{
                    //    knownFood.Add(GetNearestFood());
                    //}
                    targetFood.GetComponent<FoodSource>().Consume(this); //Eat the food
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

    //Is any food in the creature's sensory sphere, optionally output the closest
    bool FoodSense(Transform defaultout, out Transform output)
    {

        float tempdistance;
        float nearest = float.MaxValue;
        FoodSource tempsource;
        Transform outtemp = null;
        Collider[] cols = Physics.OverlapSphere(transform.position, _creatureStats._sight, 1 << 9);
        foreach (Collider foodcol in cols)
        {
            tempdistance = Vector3.Distance(transform.position, foodcol.transform.position);
            tempsource = foodcol.GetComponent<FoodSource>();
            if (tempdistance < nearest && tempsource._servesRemaining > 0 && tempsource._approachingCreature == this || tempsource._approachingCreature == null)
            {
                nearest = tempdistance;
                outtemp = foodcol.transform;
            }
        }

        if (outtemp != null)
        {
            output = outtemp;
            output.GetComponent<FoodSource>()._approachingCreature = this;
            return true;
        }
        else
        {
            output = defaultout;
            return false;
        }
    }

    //Overload of FoodSense that only checks for food within range
    bool FoodSense()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _creatureStats._sight, 1 << 9);
        if (cols.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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
        //it works, just don't think about it too much
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

    //Get all food (filtered)
    [System.Obsolete()]
    void RefreshFoodSources()
    {
        FoodSource[] tempFood = FindObjectsOfType<FoodSource>(); //Get all food in the scene for filtering

        _allFood.Clear(); //remove anything already in the list

        //Filter Compatable food into _allFood
        for (int i = 0; i < tempFood.Length; i++)
        {
            //Add to _allFood if valid
            if (DietMatchQuery(_creatureStats._dietLock, tempFood[i]._foodStats._type))
            {
                _allFood.Add(tempFood[i]);
            }
        }
    }

    public void Kill()
    {
        Debug.Log(gameObject + " has died");
        CameraControl._instance.creatures.Remove(transform); //Remove this from the camera controller targets
        Destroy(gameObject);
    }

    
}
