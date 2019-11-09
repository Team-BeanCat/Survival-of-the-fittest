using UnityEngine;
using UnityEngine.AI;

public class CreatureSpawn : MonoBehaviour
{
    public GameObject _creature;
    public GameObject Spawn;
   
    public GameObject _creatureParent;
    [Range(1,20)]
    public int _numPerSpecies;

    [HideInInspector]
    public int size; //Value Driven By Terrain Generator

    private bool _validSpawn;
    private Transform[] _cams; //The transforms of every camera in the scene

    void Start()
    {
        Vector3 randomDirection = Vector3.zero;

        for (int i = 0; i < _numPerSpecies; i++)
        {
            randomDirection = Random.insideUnitSphere * size / 2;
            randomDirection += new Vector3(0, 1, 0);
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, size / 2, 1);
            GameObject NewCreature = Instantiate(_creature, new Vector3(navHit.position.x, 1, navHit.position.z), Quaternion.identity);
            NewCreature.transform.parent = _creatureParent.transform;
            NewCreature.name = "creature" + (i + 1);
        }
    }

    Vector3 RandomPointOnMap(float borderMarginWidth)
    {
        Vector3 point = Vector3.zero;

        //Generate Point (subtract margin width for margin)
        point.x = Random.Range(-size - borderMarginWidth, size - borderMarginWidth);
        point.z = Random.Range(-size - borderMarginWidth, size - borderMarginWidth);

        return point;
    }
}
