using UnityEngine;
using UnityEngine.AI;

public class CreatureSpawn : MonoBehaviour
{
    public GameObject _creature;
    public GameObject Spawn;
   
    public GameObject _creatureParent;
    [Range(1,20)]
    public int _numPerSpecies;
    float size;

    private bool _validSpawn;
    private Transform[] _cams; //The transforms of every camera in the scene

    void Awake()
    {
        size = Spawn.GetComponent<StaticSpawns>().size;
        
        for (int i = 0; i < _numPerSpecies; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * size / 2;
            randomDirection += new Vector3(0, 1, 0);
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, size / 2, 1);
            GameObject NewCreature = Instantiate(_creature, new Vector3(navHit.position.x, 1, navHit.position.z), Quaternion.identity);
            NewCreature.transform.parent = _creatureParent.transform;
            NewCreature.name = "creature" + (i + 1);
        }
    }
}
