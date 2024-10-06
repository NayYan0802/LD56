using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerVision : MonoBehaviour
{
    public Transform player;
    public float swingDuration = 2f;
    public float moveRange = 5f;
    public float maxSwingTime = 10f;
    public float playerDistanceThreshold = 10f;
    public float distanceFromEdge = 1f;
    public float restartDelay = 3f;

    private Vector3 startPos;
    private bool isMoving = false;
    private Camera mainCamera;
    private float slideInEndTime;
    private bool isSlidingOut = false;
    private float currentSinValue;
    private bool maxTimeReached = false;
    private float lastXPos;
    private float direction;

    void Start()
    {
        mainCamera = Camera.main;
    }

    [Button]
    public void StartSwinging()
    {
        if (!isMoving)
        {
            StartCoroutine(SwingCoroutine());
        }
    }

    private IEnumerator SwingCoroutine()
    {
        isMoving = true;
        isSlidingOut = false;
        maxTimeReached = false;

        Vector3 screenLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, mainCamera.nearClipPlane));
        Vector3 screenRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, mainCamera.nearClipPlane));
        startPos = player.position;

        Vector3 initialPosition = new Vector3(screenLeft.x - distanceFromEdge, player.position.y, player.position.z);
        transform.position = initialPosition;

        float targetSpeed = (2 * Mathf.PI) / swingDuration * moveRange;
        float slideInSpeed = Mathf.Abs(targetSpeed);

        while (Vector3.Distance(transform.position, startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, slideInSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = startPos;
        slideInEndTime = Time.time;

        while (true)
        {
            lastXPos = transform.position.x;

            float elapsedSwingTime = (Time.time - slideInEndTime) % swingDuration;
            float timeFactor = elapsedSwingTime / swingDuration;
            currentSinValue = Mathf.Sin(timeFactor * 2 * Mathf.PI);
            float moveX = currentSinValue * moveRange;

            transform.position = new Vector3(startPos.x + moveX, startPos.y, startPos.z);

            if (transform.position.x > lastXPos)
            {
                direction = 1f;
            }
            else if (transform.position.x < lastXPos)
            {
                direction = -1f;
            }

            if (Time.time - slideInEndTime > maxSwingTime)
            {
                maxTimeReached = true;
            }

            if (maxTimeReached && Mathf.Abs(currentSinValue) < 0.01f || Vector3.Distance(player.position, startPos) > playerDistanceThreshold)
            {
                StartCoroutine(SlideOutCoroutine());
                break;
            }

            yield return null;
        }
    }

    private IEnumerator SlideOutCoroutine()
    {
        isSlidingOut = true;

        Vector3 currentPos = transform.position;
        Vector3 targetPosition;

        if (direction > 0)
        {
            Vector3 screenRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, mainCamera.nearClipPlane));
            targetPosition = new Vector3(screenRight.x + distanceFromEdge, currentPos.y, currentPos.z);
        }
        else
        {
            Vector3 screenLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, mainCamera.nearClipPlane));
            targetPosition = new Vector3(screenLeft.x - distanceFromEdge, currentPos.y, currentPos.z);
        }

        float targetSpeed = (2 * Mathf.PI) / swingDuration * moveRange;
        float slideOutSpeed = Mathf.Abs(targetSpeed);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, slideOutSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        yield return new WaitForSeconds(restartDelay);
        isMoving = false;
        StartSwinging();
    }

    public void StopSwinging()
    {
        StopAllCoroutines();
        isMoving = false;
        isSlidingOut = false;
    }
}
