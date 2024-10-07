using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using MoreMountains.Tools;

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

	List<GameObject> customers = new List<GameObject>();

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
		spawnCustomer();
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

	public void popCustomer(GameObject customer)
	{
		customers.Remove(customer);
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
				spawnCustomer();
			}
			if (RoundSecond == 30)
			{
				spawnCustomer();
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

	void spawnCustomer()
	{
		GameObject customer = Instantiate(CustomerPrefab);
		customer.GetComponent<Customer>().type = (CustomerType)Random.Range(0, 3);
		for (int i = -5; i > -15; i -= 2)
		{
			foreach (var cus in customers)
			{
				if (cus.GetComponent<SpriteRenderer>().sortingOrder == i)
				{
					continue;
				}
				else
				{
					customer.GetComponent<SpriteRenderer>().sortingOrder = i;
					customer.transform.GetChild(3).GetComponent<SpriteRenderer>().sortingOrder = i;
					customer.transform.GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = i + 1;
					break;
				}
			}
		}

		customers.Add(customer);
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
		InteractablesInScene.SetActive(true);
		MMSoundManagerAllSoundsControlEvent.Trigger(MMSoundManagerAllSoundsControlEventTypes.FreeAllButPersistent);
	}
}
