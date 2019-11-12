using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndKill : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Point and Kill (Creature)
        {
            Debug.Log("PEW");
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, 50f))
            {
                if (hit.transform.gameObject.CompareTag("Food")) //Check if it's a tree
                {
                    Destroy(hit.transform.parent.gameObject);
                }
                else if (hit.transform.parent.gameObject.CompareTag("Food")) //Check the parent too
                {
                    Destroy(hit.transform.parent.gameObject);
                }
                else if (hit.transform.gameObject.CompareTag("Creature"))
                {
                    hit.transform.GetComponent<Creature>().Kill(); 
                }
                else if (hit.transform.parent.gameObject.CompareTag("Creature"))
                {
                    hit.transform.parent.GetComponent<Creature>().Kill();
                }
            }
        }
    }
}
