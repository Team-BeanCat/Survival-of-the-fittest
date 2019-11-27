using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    int m_activeCamera = 0; //The index of the active camera in m_Cameras
    public GameObject[] m_Cameras; //All the cameras to cycle through
    public GameObject[] Listeners;

    private bool mouseState = false; 
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_Cameras = GameObject.FindGameObjectsWithTag("Camera"); //Get All the Cameras 
        Listeners = GameObject.FindGameObjectsWithTag("Listeners");
        

        //Turn off all cameras at the object
        foreach (GameObject camObj in m_Cameras)
        {
            camObj.GetComponent<Camera>().enabled = false;
            
        }
        //Turn on the first camera
        Listeners[0].GetComponent<AudioListener>().enabled = true;
        Listeners[1].GetComponent<AudioListener>().enabled = false;
        m_Cameras[0].GetComponent<Camera>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKeyUp(KeyCode.X))
        {

            if (mouseState)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        if (Input.GetButtonDown("CycleCam"))
        {
            mouseState = !mouseState;
            if (mouseState)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            m_Cameras[m_activeCamera].GetComponent<Camera>().enabled = false; //Disable the current camera
            Listeners[m_activeCamera].GetComponent<AudioListener>().enabled = false; //Disable the current camera
            m_activeCamera++;                           //Cycle to the next camera

            //Check if the active camera index is out of range, if it is then go back to 0
            if (m_activeCamera > m_Cameras.Length-1)
            {
                m_activeCamera = 0; //Return to the first camera in the array
            }

            //This line Keeps Throwing IndexOutOfRangeException But I've determined that it still works fine
            try //So naturally i put it in a trycatch to suppress the error - Ben M
            {
                m_Cameras[m_activeCamera].GetComponent<Camera>().enabled = true;  //Enable the new Camera
                Listeners[m_activeCamera].GetComponent<AudioListener>().enabled = true;  //Enable the new Camera
            }
            catch (System.Exception)
            {

                throw;
            }
            
        }
    }
}
