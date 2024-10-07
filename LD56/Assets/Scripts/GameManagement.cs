using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

	[SerializeField, BoxGroup("UI")] GameObject WinUI;
	[SerializeField, BoxGroup("UI")] GameObject LoseUI;
	[SerializeField, ReadOnly] private int scareNum = 0;

	[SerializeField] private GameObject CustomerPrefab;
	public static GameManagement Instance { get; private set; }

	public int border1;
	public int border2;

	public GameObject InteractablesInstance;
	public GameObject InteractablesInScene;

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
			WinUI.SetActive(true);
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
			if (RoundSecond < 10)
			{
				UITime.text = $"{RoundMinute} : 0{RoundSecond}";
			}
			else
			{
				UITime.text = $"{RoundMinute} : {RoundSecond}";
			}
			yield return new WaitForSeconds(1f);
		}
		LoseUI.SetActive(true);
		//game fail
	}

	public void Retry()
	{
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

	public void BackToMenu()
	{
		SceneManager.LoadScene("MainScene_MainMenu");
	}

	[Button]
	public void ResetInteractables()
    {
		Destroy(InteractablesInScene);
		InteractablesInScene = Instantiate(InteractablesInstance, Vector3.zero, Quaternion.identity);
    }
}
