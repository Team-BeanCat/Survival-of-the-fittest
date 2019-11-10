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
                if (hit.transform.gameObject.layer == 10)
                {
                    hit.transform.GetComponent<Creature>().Kill();
                }
                else if (hit.transform.gameObject.layer == 9)
                {
                    Destroy(hit.transform.parent.gameObject);
                }
            }
        }
    }
}
