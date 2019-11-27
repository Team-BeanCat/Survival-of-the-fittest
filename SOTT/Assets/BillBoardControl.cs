using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardControl : MonoBehaviour
{
    public GameObject camControl;
    public bool state = true;
    public GameObject BillBoard; 
    
    // Start is called before the first frame update
    void Start()
    {
        camControl = GameObject.Find("Cameras");
        state = camControl.GetComponent<CameraSwitch>().showNames;
    }

    // Update is called once per frame
    void Update()
    {
        state = camControl.GetComponent<CameraSwitch>().showNames;
        BillBoard.SetActive(state);
    }
}
