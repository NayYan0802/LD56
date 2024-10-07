using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vending : MonoBehaviour
{
    public Sprite plugOff;
    public GameObject[] vendingObjects;

    public void Unplug()
    {
        this.GetComponent<SpriteRenderer>().sprite = plugOff;
        foreach(var _object in vendingObjects)
        {
            _object.GetComponent<SpriteRenderer>().sortingOrder = -1;
            _object.GetComponent<Rigidbody2D>().gravityScale = Constant.gravityScale;
        }
    }
}
