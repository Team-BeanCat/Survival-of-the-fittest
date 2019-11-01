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
    private Camera _cam;

    void Start()
    {
        size = Spawn.GetComponent<StaticSpawns>().size;
        

        _cam = Camera.main;
        for (int i = 0; i < _numPerSpecies; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * size / 2;
            randomDirection += new Vector3(0, 1, 0);
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, size / 2, 1);
            GameObject TempRock = Instantiate(_creature, new Vector3(navHit.position.x, 1, navHit.position.z), Quaternion.identity);
            TempRock.transform.parent = _creatureParent.transform;
            TempRock.name = "creature" + (i + 1);
            TempRock.transform.GetChild(0).gameObject.GetComponent<CameraFacingBillboard>().m_Camera = _cam; //Set up the Camera facing Billboard Script
        }
    }
}
