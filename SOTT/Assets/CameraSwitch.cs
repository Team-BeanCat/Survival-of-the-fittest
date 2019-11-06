using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    int m_activeCamera = 0; //The index of the active camera in m_Cameras
    GameObject[] m_Cameras; //All the cameras to cycle through
    
    private void Start()
    {
        m_Cameras = GameObject.FindGameObjectsWithTag("Camera"); //Get All the Cameras 

        //Turn off all cameras at the object
        foreach (GameObject camObj in m_Cameras)
        {
            camObj.SetActive(false);
        }
        //Turn on the first camera
        m_Cameras[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("CycleCam"))
        {
            m_Cameras[m_activeCamera].SetActive(false); //Disable the current camera
            m_activeCamera++;                           //Cycle to the next camera

            //Check if the active camera index is out of range, if it is then go back to 0
            if (m_activeCamera > m_Cameras.Length-1)
            {
                m_activeCamera = 0; //Return to the first camera in the array
            }

            //This line Keeps Throwing IndexOutOfRangeException But I've determined that it still works fine
            try //So naturally i put it in a trycatch to suppress the error - Ben M
            {
                m_Cameras[m_activeCamera].SetActive(true);  //Enable the new Camera
            }
            catch (System.Exception)
            {

                throw;
            }
            
        }
    }
}
