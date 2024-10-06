using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class InteractableDoor : MonoBehaviour
{

    private bool playerTrigger = false;

    
    void Update()
    {
        if (playerTrigger = true && Input.GetKeyDown(KeyCode.R))
        {
            float doorRotationZ = transform.eulerAngles.z;
            if (Mathf.Approximately(doorRotationZ, 0f) || Mathf.Approximately(doorRotationZ, 360f))
            {
                transform.eulerAngles = new Vector3(0, 0, 90f);
            }
            if (Mathf.Approximately(doorRotationZ, 90f))
            {
                transform.eulerAngles = new Vector3(0, 0, 0f);
            }
        }
    }

    public void OnTriggerEnter2D (Collider2D other)
    {
        playerTrigger = true;
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        playerTrigger = false;
    }


}
