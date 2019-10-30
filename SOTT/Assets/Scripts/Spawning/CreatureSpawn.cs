using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawn : MonoBehaviour
{
    public GameObject _creature;
   
    public GameObject _creatureParent;
    [Range(1,20)]
    public int _numPerSpecies;
    public float x = 50;
    public float z = 50;
    bool _validSpawn;
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
        for (int x = 0; x < _numPerSpecies; x++)
        {
            _validSpawn = false;
            while (!_validSpawn)
            {
                float xInst = Random.Range(-((x / 2) - 5), (x / 2) - 5);
                float zInst = Random.Range(-((z / 2) - 5), (z / 2) - 5);
                Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 2f, 8);
                if (hitColliders.Length == 0)
                {
                    GameObject TempCreature = Instantiate(_creature, new Vector3(xInst, -1, zInst), Quaternion.Euler(0f, Random.Range(0f, 359f), 0f));
                    TempCreature.transform.parent = _creatureParent.transform;
                    TempCreature.transform.GetChild(0).gameObject.GetComponent<CameraFacingBillboard>().m_Camera = _cam;

                    _validSpawn = true;
                }
            }
        }
    }
}
