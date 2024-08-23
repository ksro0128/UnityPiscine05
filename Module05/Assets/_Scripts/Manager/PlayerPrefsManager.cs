using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager instance = null;

	void Awake()
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

	void Start()
	{
		if (!PlayerPrefs.HasKey("SFXVolume"))
		{
			PlayerPrefs.SetFloat("SFXVolume", 1f);
		}
		if (!PlayerPrefs.HasKey("BGMVolume"))
		{
			PlayerPrefs.SetFloat("BGMVolume", 1f);
		}
	}

	public float GetBGMVolume() 
	{
		return PlayerPrefs.GetFloat("BGMVolume", 1f);
	}

	public void SetBGMVolume(float volume)
	{
		PlayerPrefs.SetFloat("BGMVolume", volume);
	}

	public float GetSFXVolume()
	{
		return PlayerPrefs.GetFloat("SFXVolume", 1f);
	}

	public void SetSFXVolume(float volume)
	{
		PlayerPrefs.SetFloat("SFXVolume", volume);
	}

	public string GetNickname()
	{
		return PlayerPrefs.GetString("Nickname", "Player");
	}

	public void SetNickname(string nickname)
	{
		PlayerPrefs.SetString("Nickname", nickname);
	}

	public void SetStage(string stage)
	{
		PlayerPrefs.SetString("Stage", stage);
	}

	public string GetStage()
	{
		return PlayerPrefs.GetString("Stage", "");
	}
	
	public void SaveStageData(string stage, Vector3 position, Vector2 velocity, bool[] isEat, int health, bool[] isActive)
	{
		StageData stageData = new StageData(position, velocity, isEat, health, isActive);
		string json = JsonUtility.ToJson(stageData);
		PlayerPrefs.SetString(stage, json);
	}

	public StageData LoadStageData(string stage)
	{
		if (!PlayerPrefs.HasKey(stage))
			return null;
		string json = PlayerPrefs.GetString(stage);
		StageData stageData = JsonUtility.FromJson<StageData>(json);
		return stageData;
	}

	public void DeleteStageData(string stage)
	{
		PlayerPrefs.DeleteKey(stage);
	}

	public void DeleteAllData()
	{
		float bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
		float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
		string nickname = PlayerPrefs.GetString("Nickname");
		PlayerPrefs.DeleteAll();
		PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
		PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
		PlayerPrefs.SetString("Nickname", nickname);
	}

	public void SetClearData(string stage, int leafCnt)
	{
		PlayerPrefs.SetInt(stage + "ClearDate", leafCnt);
	}

	public int GetClearData(string stage)
	{
		return PlayerPrefs.GetInt(stage + "ClearDate", 0);
	}

	public void AddCountDeath()
	{
		int deathCount = PlayerPrefs.GetInt("DeathCount", 0);
		PlayerPrefs.SetInt("DeathCount", deathCount + 1);
	}

	public int GetDeathCount()
	{
		return PlayerPrefs.GetInt("DeathCount", 0);
	}

}
