using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFall : MonoBehaviour
{
    public GameObject PuffAnimation;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<InteractableObject>()) 
        {
            Instantiate(PuffAnimation,collision.bounds.center,Quaternion.identity);
            collision.gameObject.GetComponent<InteractableObject>().Scared(collision.bounds.center.x);
            Destroy(collision.gameObject);
        }
    }
}
