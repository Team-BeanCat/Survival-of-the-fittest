using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //For Navmesh

public class Creature : MonoBehaviour
{
    //What action the creature is currently taking
    enum State
    {
        FoodSearch, Idle, MateSearch, WaterSearch, ContinuingTheBloodLine
    }

    public FoodSource[] _allFood;
    public List<float> _foodDist = new List<float>();
    public FoodSource targetFood; //Food Currently being Pursued
    public bool showRanges; // render the sight range of creature
    public CreatureStats _creatureStats;
    [Range(0, 100f)]
    public float _sustinance = 100f;

    [SerializeField] private State _currentState = State.Idle;
    private float _health = 100f;
    private float _maxhealth = 100f;
    private float _age = 0f;
    
    private int _segments = 50; //Segments for sight field line
    private LineRenderer line; // line renderer for 
    private NavMeshAgent _agent; //Nav mesh agent
    Vector3 newDest = new Vector3();

    public List<FoodSource> knownFood;

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


    private void Init(float maxhealth)
    {
        
    }

    void FixedUpdate()
    {
        _sustinance -= 0.05f;
    }

    void Update()
    {
        //if (hunger >= 50f) //if hunger is large enough then find a mate
        //{
        //    mate();
        //}



        //If The Hunger is below 75% then go eat
        if (_sustinance < 75f)
        {
            //Debug.Log(gameObject + " Is hungry");
            _currentState = State.FoodSearch; //Set the state to searching for food

            //If the creature has found food
            if (foodSourceInRange() == true)
            {
                //Debug.Log("Found Food!");
                Vector3 FoodLocation = GetNearestFood().transform.position;
                //Check if the nearest food source is within eating range

                NavMeshHit hit;
                NavMesh.SamplePosition(FoodLocation, out hit, 5f, 1 << NavMesh.GetAreaFromName("Walkable"));
                //Debug.Log(hit.position);
                if (Vector3.Distance(hit.position, transform.position) < 1.5*GetNearestFood().transform.localScale.x) //Eat range dependant on object size
                {
                    //Debug.Log("Eating Food");
                    if (!(knownFood.Contains(GetNearestFood())))
                    {
                        knownFood.Add(GetNearestFood());
                    }
                    GetNearestFood().Consume(this); //Eat the food
                    targetFood = null;
                    _agent.SetDestination(transform.position); //Trigger the wander function
                }
                else
                {
                    //Go towards it
                    _agent.SetDestination(FoodLocation); //Find the nearest food (will be within range) and head towards it 
                }
            }
            /*
            else if (_hunger<0.1f && !foodSourceInRange())
            {
                //find nearest known food source and go to it 
                
            }

              */  

            //Was an else if IF BROKEN CHANGE IT BACK
            if (Vector3.Distance(_agent.destination, transform.position) < 1.5) //Check if the Creature is at the pathfinder's destination
            {
                //Debug.Log("Searching");
                //Get a random point in a circle with a radius equal to the creature's sight * 2
                newDest = RandomNavSphere(transform.position, _creatureStats._sight * 2, LayerMask.NameToLayer("Walkable")); //Pick a random point within the sight range
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
        targetFood = _allFood[MinDistance(_foodDist)].GetComponent<FoodSource>();
        _foodDist.Clear(); //Wipe the list clean
        return targetFood;

    }
    
    private int MinDistance(List<float> Dist) // takes an array and returns the location of that number
    {
        float min = Dist[0];
        int minLoc = 0;
        for (int i = 0; i < Dist.Count; i++)
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
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, 2, layermask);

        return navHit.position;
        //return randDirection;
    }
}
