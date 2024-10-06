using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[SerializeField] GameObject Player;
	public bool isFollowing = true;
	[SerializeField] private float smoothSpeed = 0.125f;
	[SerializeField] float cameraZOffset;
	private Vector3 velocity = Vector3.zero;

	public static CameraManager Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}


	private void Update()
	{
		if (isFollowing)
		{
			Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, Player.transform.position + new Vector3(0, 0, cameraZOffset), ref velocity, smoothSpeed);
			transform.position = smoothedPosition;
		}
	}
}
