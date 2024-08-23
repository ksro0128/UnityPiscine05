using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	private float mapLeftCameraLimit = -0.01f;
	private float mapRightCameraLimit = 38.17f;
	private float mapTopCameraLimit = 6.34f;
	private float mapBottomCameraLimit = -2.29f;
	public static StageManager instance = null;
	[SerializeField] GameObject leafPrefab;
	[SerializeField] GameObject[] leafPoints;
	[SerializeField] string stageName;
	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject startPoint;
	[SerializeField] GameObject endPoint;
	[SerializeField] GameObject[] walls;
	private GameObject player;
	private GameObject[] leaves;
	private bool[] isEat;
	private bool[] isActive;
	private int getCnt = 0;
	private bool resetFlag = false;

	private bool isStageClear = false;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R) && !resetFlag && !isStageClear)
		{
			GameManager.instance.RestartStage();
			StartCoroutine(ResetFlagCoroutine());
		}
	}

	IEnumerator ResetFlagCoroutine()
	{
		resetFlag = true;
		yield return new WaitForSeconds(10f);
		resetFlag = false;
	}

    void Start()
    {
		leaves = new GameObject[leafPoints.Length];
		isEat = new bool[leafPoints.Length];
		isActive = new bool[walls.Length];

		StageData stageData = PlayerPrefsManager.instance.LoadStageData(stageName);
		if (stageData == null)
			InitStage();
		else
			LoadStage(stageData);
		GameManager.instance.SetUI(true);
		GameManager.OnSaveData += PauseGameAndSave;
		GameManager.OnPlayerDeath += ResetStage;
		GameManager.OnStageClear += StageClear;
		GameManager.instance.SetHealthText();
		GameManager.instance.SetLeafText(getCnt);
    }

	void StageClear()
	{
		isStageClear = true;
	}

	void ResetStage()
	{
		StartCoroutine(ResetStageCoroutine());
	}

	IEnumerator ResetStageCoroutine()
	{
		yield return new WaitForSeconds(2f);
		Destroy(player);
		for (int i = 0; i < leafPoints.Length; i++)
		{
			isEat[i] = false;
			leaves[i].SetActive(true);
		}
		for (int i = 0; i < walls.Length; i++)
		{
			walls[i].SetActive(true);
			isActive[i] = true;
		}
		player = Instantiate(playerPrefab, startPoint.transform.position, Quaternion.identity, transform);
		player.GetComponent<PlayerController>().SetIsRespawn(true);
		GameManager.instance.SetHealth(3);
		GameManager.instance.SetHealthText();
		getCnt = 0;
		GameManager.instance.SetLeafText(getCnt);
	}

	void OnDestroy()
	{
		GameManager.OnSaveData -= PauseGameAndSave;
		GameManager.OnPlayerDeath -= ResetStage;
		GameManager.OnStageClear -= StageClear;
	}

	void PauseGameAndSave()
	{
		Debug.Log("PauseGameAndSave");
		player.GetComponent<PlayerController>().SetStun();
		SaveStageData();
	}

	void SaveStageData()
	{
		Debug.Log("SaveStageData");
		Vector3 position = player.transform.position;
		Vector2 velocity = player.GetComponent<Rigidbody2D>().velocity;
		int health = GameManager.instance.GetHealth();
		GetIsActives();
		PlayerPrefsManager.instance.SaveStageData(stageName, position, velocity, isEat, health, isActive);
	}

	void GetIsActives()
	{
		for (int i = 0; i < walls.Length; i++)
		{
			isActive[i] = walls[i].activeSelf;
		}
	}

	void LoadStage(StageData stageData)
	{
		Debug.Log("LoadStage");
		getCnt = 0;
		for (int i = 0; i < leafPoints.Length; i++)
		{
			isEat[i] = stageData.isEat[i];
			leaves[i] = Instantiate(leafPrefab, leafPoints[i].transform.position, Quaternion.identity, transform);
			leaves[i].GetComponent<Leaf>().SetNum(i);
			if (isEat[i])
			{
				leaves[i].SetActive(false);
				getCnt++;
			}
		}
		for (int i = 0; i < walls.Length; i++)
		{
			walls[i].SetActive(stageData.isActive[i]);
			isActive[i] = stageData.isActive[i];
		}
		player = Instantiate(playerPrefab, stageData.position, Quaternion.identity, transform);
		player.GetComponent<Rigidbody2D>().velocity = stageData.velocity;
		player.GetComponent<PlayerController>().SetIsRespawn(false);
		GameManager.instance.SetHealth(stageData.health);
		GameManager.instance.SetHealthText();
		GameManager.instance.SetLeafText(getCnt);
	}

	void InitStage()
	{
		Debug.Log("InitStage");
		for (int i = 0; i < leafPoints.Length; i++)
		{
			isEat[i] = false;
			leaves[i] = Instantiate(leafPrefab, leafPoints[i].transform.position, Quaternion.identity, transform);
			leaves[i].GetComponent<Leaf>().SetNum(i);
			leaves[i].SetActive(true);
		}
		for (int i = 0; i < walls.Length; i++)
		{
			walls[i].SetActive(true);
			isActive[i] = true;
		}
		player = Instantiate(playerPrefab, startPoint.transform.position, Quaternion.identity, transform);
		player.GetComponent<PlayerController>().SetIsRespawn(true);
		GameManager.instance.SetHealth(3);
		GameManager.instance.SetHealthText();
		getCnt =  0;
		GameManager.instance.SetLeafText(getCnt);
		SaveStageData();
	}

	public float GetMapLeftCameraLimit()
	{
		return mapLeftCameraLimit;
	}

	public float GetMapRightCameraLimit()
	{
		return mapRightCameraLimit;
	}

	public float GetMapTopCameraLimit()
	{
		return mapTopCameraLimit;
	}

	public float GetMapBottomCameraLimit()
	{
		return mapBottomCameraLimit;
	}

	public void GetLeaf(int num)
	{
		if (isEat[num])
			return;
		isEat[num] = true;
		leaves[num].SetActive(false);
		getCnt++;
		GameManager.instance.SetLeafText(getCnt);
		SaveStageData();
	}



}
