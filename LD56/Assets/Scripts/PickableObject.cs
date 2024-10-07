using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PickableObject : MonoBehaviour
{
    [SerializeField] MMF_Player pickupFB, putdownFB;
    public PickableObject PickUp(Player picker)
    {
        this.transform.SetParent(picker.pickOffset);
        this.transform.localPosition = Vector3.zero;
        this.GetComponent<SpriteRenderer>().sortingOrder = 6;
        this.GetComponent<Rigidbody2D>().gravityScale = 0; 
        this.GetComponent<Rigidbody2D>().isKinematic = true;
        this.gameObject.layer = 8;
        if (pickupFB != null)
            pickupFB.PlayFeedbacks();
        return this;
    }

    public void PutDown()
    {
        Debug.Log("Put Down");
        this.transform.SetParent(null);
        this.GetComponent<SpriteRenderer>().sortingOrder = 0;
        this.GetComponent<Rigidbody2D>().gravityScale = Constant.gravityScale;
        this.GetComponent<Rigidbody2D>().isKinematic = false;
        this.gameObject.layer = 7;
        if (putdownFB != null)
            putdownFB.PlayFeedbacks();
    }
}
