using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class ItemFall : MonoBehaviour
{
    [SerializeField] MMF_Player hitGroundFB;
    public GameObject PuffAnimation;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<InteractableObject>()) 
        {
            Instantiate(PuffAnimation,collision.bounds.center,Quaternion.identity);
            collision.gameObject.GetComponent<InteractableObject>().Scared(collision.bounds.center.x);
            Destroy(collision.gameObject);
            hitGroundFB.PlayFeedbacks();
        }
    }
}
