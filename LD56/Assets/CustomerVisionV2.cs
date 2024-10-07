using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerVisionV2 : MonoBehaviour
{
    public Transform leftPosition;
    public Transform centerPosition;
    public Transform rightPosition;
    public Transform player;
    public float swayAmplitude = 1f;
    public float swaySpeed = 2f;

    private Vector3 targetPosition;
    private float initialY;
    private bool isSwaying = false;

    public int swayAreaIndex;

    public Customer customer;
    public SpriteMask visionObject;

    void Start()
    {
        targetPosition = centerPosition.position;
        initialY = player.transform.position.y;
        leftPosition = GameObject.Find("Left").transform;
        centerPosition = GameObject.Find("Center").transform;
        rightPosition = GameObject.Find("Right").transform;
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (isSwaying)
        {
            float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmplitude;

            float newX = targetPosition.x + sway;

            transform.position = new Vector3(newX, initialY, transform.position.z);
        }
    }

    [Button]
    public void StartSwaying()
    {
        visionObject.enabled = true;
        swayAreaIndex = customer.currentZone;

        switch (swayAreaIndex)
        {
            case 0:
                MoveToLeft();
                break;
            case 1:
                MoveToCenter();
                break;
            case 2:
                MoveToRight();
                break;
        }

        isSwaying = true;
        initialY = player.transform.position.y;
    }

    [Button]
    public void StopSwaying()
    {
        visionObject.enabled = false;
        isSwaying = false;
    }

    public void MoveToLeft()
    {
        targetPosition = leftPosition.position;
        initialY = transform.position.y;
    }

    public void MoveToCenter()
    {
        targetPosition = centerPosition.position;
        initialY = transform.position.y;
    }

    public void MoveToRight()
    {
        targetPosition = rightPosition.position;
        initialY = transform.position.y;
    }
}
