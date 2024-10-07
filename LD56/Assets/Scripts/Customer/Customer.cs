using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public enum CustomerType { 
	GrandPa,
	Student,
	RepairGuy
}

public class Customer : MonoBehaviour
{
	[SerializeField] float finalDis = 5;
	[SerializeField] float initialDis= 50;
	[SerializeField] float moveSpeed = 0.2f;
	[SerializeField, Tooltip("percentage of possibility customer go to his favor spot, only 0-1, percentage for other two area is equal")] float favorSpotPercentage = 0.7f;
	[SerializeField] public CustomerType type;
	[SerializeField] Vector3 GroceriesPos, SnackPos, ToolsPos, ExitPos;
	[SerializeField] float exitScale, shelfScale;
	[SerializeField] float PosOffsetx;
	[SerializeField] float eyeMax;
	[SerializeField] int StayTimeMin, StayTimeMax;
	[SerializeField] Sprite scared1, scared2, scared3, scared4;
	[SerializeField] Sprite scaredEye1, scaredEye2, scaredEye3;
	[SerializeField] MMF_Player moveFeedback;
	[SerializeField] SpriteRenderer spriteRendererEye;
	[SerializeField] GameObject eyeView;
	public int currentZone;
	public int m_scareMeter = 0;

	bool runaway;
	SpriteRenderer spriteRenderer;

	Coroutine timeCoroutine = null;

	private Vector3 initialPos;
	private float initialEyePos, initialEyePosFlip;
	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		initialPos = ExitPos;
		initialEyePos = spriteRendererEye.transform.localPosition.x;
		initialEyePosFlip = 0 - initialEyePos;
		timeCoroutine = StartCoroutine(Timer());
	}

	private void Update()
	{
		if (transform.position.x < GameManagement.Instance.border1)
		{
			currentZone = 0;
		}
		else if (transform.position.x > GameManagement.Instance.border2)
		{
			currentZone = 2;
		}
		else
		{
			currentZone = 1;
		}
		if (spriteRendererEye.flipX)
		{
			spriteRendererEye.transform.localPosition = new Vector3(initialEyePosFlip + eyeMax * ((eyeView.transform.position.x - transform.position.x) / 13), spriteRendererEye.transform.localPosition.y, spriteRendererEye.transform.localPosition.z);
		}
		else
		{
			spriteRendererEye.transform.localPosition = new Vector3(initialEyePos + eyeMax * ((eyeView.transform.position.x - transform.position.x) / 13), spriteRendererEye.transform.localPosition.y, spriteRendererEye.transform.localPosition.z);

		}
	}

	public void freeze()
	{
		GetComponentInChildren<CustomerVisionV2>().enabled = false;
		Invoke("fail", 2);
	}

	private void fail()
	{
		StopCoroutine(timeCoroutine);
		MoveTo(ExitPos);
	}

	public void Scared(int scareMeter)
    {
		m_scareMeter += scareMeter;

		if (m_scareMeter >= 30 && !runaway)
		{
			runaway = true;
			spriteRenderer.sprite = scared4;
			spriteRendererEye.enabled = false;
			StopCoroutine(timeCoroutine);
			GameManagement.Instance.ScareOnePeople();
			MoveTo(ExitPos);
		}
		else if (m_scareMeter >= 20)
		{
			spriteRenderer.sprite = scared3;
			spriteRendererEye.sprite = scaredEye3;
		}
		else if (m_scareMeter >= 10)
		{
			spriteRenderer.sprite = scared2;
			spriteRendererEye.sprite = scaredEye2;
		}
		else if (m_scareMeter > 0)
		{
			spriteRenderer.sprite = scared1;
			spriteRendererEye.sprite = scaredEye1;
		}
	}

	IEnumerator Timer()
	{
		Vector3 nextDes = getNextDes();
		MoveTo(nextDes);
		int t = 0;
		int stayT = 0;
		int stayTime = Random.Range(StayTimeMin, StayTimeMax);
		while (t  <= 60)
		{
			if (stayT >= stayTime)
			{
				stayT = 0;
				stayTime = Random.Range(StayTimeMin, StayTimeMax);
				nextDes = getNextDes();
				MoveTo(nextDes);
			}
			t++;
			stayT++;
			yield return new WaitForSeconds(1f);
		}
		MoveTo(ExitPos);
	}

	void MoveTo(Vector3 nextDes)
	{
		float t = 0;
		if (initialPos.x < nextDes.x && !spriteRenderer.flipX)
		{
			spriteRenderer.flipX = true;
			spriteRendererEye.flipX = true;
		}
		if (initialPos.x > nextDes.x && spriteRenderer.flipX)
		{
			spriteRenderer.flipX = false;
			spriteRendererEye.flipX = false;
		}
		float timeUsed = Vector3.Distance(initialPos, nextDes) / moveSpeed;
		MMF_Position move = moveFeedback.GetFeedbackOfType<MMF_Position>();
		move.DestinationPosition = nextDes;
		move.InitialPosition = initialPos;
		move.SetFeedbackDuration(timeUsed);
		MMF_Scale scale = moveFeedback.GetFeedbackOfType<MMF_Scale>();
		scale.Active = false;
		if (initialPos == ExitPos)
		{
			scale.Active = true;
			scale.AnimateScaleDuration = timeUsed;
			scale.RemapCurveZero = exitScale;
			scale.RemapCurveOne = shelfScale;
		}
		if (nextDes == ExitPos)
		{
			GetComponentInChildren<CustomerVisionV2>().enabled = true;
			scale.Active = true;
			GetComponent<SpriteRenderer>().sortingOrder = -17;
			transform.GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = -16;
			scale.AnimateScaleDuration = timeUsed;
			scale.RemapCurveZero = shelfScale;
			scale.RemapCurveOne = exitScale;
			moveFeedback.Events.OnComplete.AddListener(SelfDestory);
		}
		moveFeedback.PlayFeedbacks();
		initialPos = nextDes;
	}

	Vector3 getNextDes()
	{
		int randomNum = Random.Range(0, 1);
		Vector3 groceryPos = GroceriesPos + new Vector3(Random.Range(-PosOffsetx, PosOffsetx), 0, 5);
		Vector3 snackPos = SnackPos + new Vector3(Random.Range(-PosOffsetx, PosOffsetx), 0, 5);
		Vector3 toolsPos = ToolsPos + new Vector3(Random.Range(-PosOffsetx, PosOffsetx), 0, 5);
		if (randomNum <= favorSpotPercentage)
		{
			switch (type)
			{
				case CustomerType.GrandPa:
					return groceryPos;
					break;
				case CustomerType.Student:
					return snackPos;
					break;
				case CustomerType.RepairGuy:
					return toolsPos;
					break;
			}

			//favorspot
		}
		else if (randomNum <= favorSpotPercentage + (1 - favorSpotPercentage) / 2)
		{
			switch (type)
			{
				case CustomerType.GrandPa:
					return snackPos;
					break;
				case CustomerType.Student:
					return groceryPos;
					break;
				case CustomerType.RepairGuy:
					return groceryPos;
					break;
			}
		}
		else
		{
			switch (type)
			{
				case CustomerType.GrandPa:
					return toolsPos;
					break;
				case CustomerType.Student:
					return toolsPos;
					break;
				case CustomerType.RepairGuy:
					return snackPos;
					break;
			}
		}
		return groceryPos;
	}

	public void SelfDestory()
	{
		GameManagement.Instance.popCustomer(gameObject);
		Destroy(gameObject);
	}
}
