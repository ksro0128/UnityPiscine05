using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
	[SerializeField] GameObject fadeOutPanel;
	[SerializeField] GameObject uiCanvas;
	[SerializeField] TMPro.TMP_Text healthText;
	[SerializeField] TMPro.TMP_Text leafText;
	[SerializeField] TMPro.TMP_Text NotEnoughLeafText;
	[SerializeField] TMPro.TMP_Text ScoreText;
	[SerializeField] TMPro.TMP_Text GameClearText;
	private string currentStage;
	private int health;
	private int leafCnt;
	public static event Action OnPlayerDeath;
	public static event Action OnSaveData;
	public static event Action OnStageClear;
	private bool isMenu = true;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		AudioManager.instance.PlayBGM();
		NotEnoughLeafText.gameObject.SetActive(false);
		GameClearText.gameObject.SetActive(false);
		fadeOutPanel.SetActive(false);
	}

	public void GameOver()
	{
		AudioManager.instance.PlayDefeat();
		PlayerPrefsManager.instance.DeleteStageData(currentStage);
		PlayerPrefsManager.instance.AddCountDeath();
		StartCoroutine(FadeOutFadeIn());
	}

	IEnumerator FadeOutFadeIn()
	{
		yield return new WaitForSeconds(1f);
		FadeOut();
		yield return new WaitForSeconds(3.0f);
		FadeIn();
	}

	private void FadeIn()
	{
		StartCoroutine(FadeInCoroutine());
	}

	IEnumerator FadeInCoroutine()
	{
		while (fadeOutPanel.GetComponent<Image>().color.a > 0)
		{
			Color color = fadeOutPanel.GetComponent<Image>().color;
			color.a -= 0.2f;
			fadeOutPanel.GetComponent<Image>().color = color;
			yield return new WaitForSeconds(0.01f);
		}
		fadeOutPanel.SetActive(false);
	}

	private void FadeOut()
	{
		StartCoroutine(FadeOutCoroutine());
	}

	IEnumerator FadeOutCoroutine()
	{
		fadeOutPanel.SetActive(true);
		while (fadeOutPanel.GetComponent<Image>().color.a < 1)
		{
			Color color = fadeOutPanel.GetComponent<Image>().color;
			color.a += 0.1f;
			fadeOutPanel.GetComponent<Image>().color = color;
			yield return new WaitForSeconds(0.01f);
		}
	}

	public void GoMainMenu()
	{
		isMenu = true;
		Debug.Log("GoMainMenu");
		StartCoroutine(GoMainMenuCoroutine());
	}

	IEnumerator GoMainMenuCoroutine()
	{
		OnSaveData?.Invoke();
		yield return StartCoroutine(FadeOutCoroutine());
		UnloadAllScene();
		Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Additive);
		yield return new WaitForSeconds(2.0f);
		Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
		yield return StartCoroutine(FadeInCoroutine());
	}

	public void TakeDamage()
	{
		health--;
		SetHealthText();
		Debug.Log("Health: " + health);
		if (health <= 0)
		{
			OnPlayerDeath?.Invoke();
			GameOver();
		}
	}

	public void FallDeath()
	{

		OnPlayerDeath?.Invoke();
		GameOver();
	}

	public void RestartStage()
	{
		OnPlayerDeath?.Invoke();
		GameOver();
	}

	public void UnloadAllScene()
	{
		for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
		{
			if (UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name != "Manager")
				UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetSceneAt(i));
		}
	}
	
	public void LoadStage(string stage)
	{
		isMenu = false;
		currentStage = stage;
		StartCoroutine(LoadStageCoroutine(stage));
		PlayerPrefsManager.instance.SetStage(stage);
	}

	private IEnumerator LoadStageCoroutine(string stage)
	{
		yield return StartCoroutine(FadeOutCoroutine());
		yield return new WaitForSeconds(1.0f);
		UnloadAllScene();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(stage, UnityEngine.SceneManagement.LoadSceneMode.Additive);
		yield return StartCoroutine(FadeInCoroutine());
	}

	public void SetUI(bool isShow)
	{
		uiCanvas.SetActive(isShow);
	}

	public void SetHealth(int health)
	{
		this.health = health;
	}

	public int GetHealth()
	{
		return health;
	}

	public void SetLeafText(int leaf)
	{
		leafCnt = leaf;
		leafText.text = leaf + " / 8";
		ScoreText.text = "Score: " + leafCnt * 5;
	}

	public void SetHealthText()
	{
		healthText.text = "X " + health;
	}

	public int GetLeafCnt()
	{
		return leafCnt;
	}

	public void StageClear()
	{
		if (isMenu)
			return;
		OnStageClear?.Invoke();
		PlayerPrefsManager.instance.DeleteStageData(currentStage);
		PlayerPrefsManager.instance.SetClearData(currentStage, leafCnt);
		if (currentStage == "Stage1")
		{
			LoadStage("Stage2");
		}
		else if (currentStage == "Stage2")
		{
			LoadStage("Stage3");
		}
		else if (currentStage == "Stage3")
		{
			ClearGame();
		}
	}

	public void ClearGame()
	{
		Debug.Log("Game Clear");
		StartCoroutine(ClearGameCoroutine());
	}

	IEnumerator ClearGameCoroutine()
	{
		yield return StartCoroutine(FadeOutCoroutine());
		GameClearText.gameObject.SetActive(true);
		yield return new WaitForSeconds(3.0f);
		GoMainMenu();
		PlayerPrefsManager.instance.DeleteStageData(currentStage);
		PlayerPrefsManager.instance.SetStage("");
		yield return StartCoroutine(FadeInCoroutine());
		GameClearText.gameObject.SetActive(false);
	}

	public void NotEnoughLeaf()
	{
		StartCoroutine(NotEnoughLeafCoroutine());
	}

	IEnumerator NotEnoughLeafCoroutine()
	{
		NotEnoughLeafText.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		NotEnoughLeafText.gameObject.SetActive(false);
	}
}
