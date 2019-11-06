using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
    [SerializeField] private int m_activeCamera = 0; //The index of the active camera in m_Cameras
    [SerializeField] private GameObject[] m_Cameras; //All the cameras to cycle through

    private void Awake()
    {
        m_Cameras = GameObject.FindGameObjectsWithTag("Camera"); //Get All the Cameras 
    }

    private void Update()
    {
        if (Input.GetButtonDown("CycleCam"))
        {
            m_activeCamera++;                           //Cycle to the next camera
            
            //Check if the active camera index is out of range, if it is then go back to 0
            if (m_activeCamera > m_Cameras.Length-1)
            {
                m_activeCamera = 0; //Return to the first camera in the array
            }

            //Turn to face it in LateUpdate
        }
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.LookAt(transform.position + m_Cameras[m_activeCamera].transform.rotation * Vector3.forward,
            m_Cameras[m_activeCamera].transform.rotation * Vector3.up);
    }
}