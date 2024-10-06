using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class GameManagement : MonoBehaviour
{
	[SerializeField, BoxGroup("Time")] int RoundMinute;
	[SerializeField, BoxGroup("Time")] int RoundSecond;

	[SerializeField, BoxGroup("UI")] TMP_Text UITime;
	[SerializeField, BoxGroup("UI")] Image[] PeopleIndicators;
	[SerializeField, BoxGroup("UI")] Sprite NumberMeter_Full;
	[SerializeField, ReadOnly] private int scareNum = 0;

	[SerializeField] private GameObject CustomerPrefab;
	public static GameManagement Instance { get; private set; }

	public int border1;
	public int border2;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		
		StartCoroutine(CountDown());
	}

	public void ScareOnePeople()
	{
		scareNum++;
		PeopleIndicators[scareNum - 1].sprite = NumberMeter_Full;
		if (scareNum >= 3)
		{ 
			//game success
		}
	}

	IEnumerator CountDown()
	{
		while (RoundMinute != 0 || RoundSecond != 0)
		{ 
			RoundSecond--;
			if (RoundSecond < 0)
			{
				RoundMinute--;
				RoundSecond = 59;
				Instantiate(CustomerPrefab);
			}
			UITime.text = $"{RoundMinute} : {RoundSecond}";
			yield return new WaitForSeconds(1f);
		}
		//game fail
	}
}
