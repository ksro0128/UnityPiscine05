using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance = null;
	[SerializeField] GameObject SettingPanel;
	[SerializeField] GameObject DiaryPanel;
	[SerializeField] Slider BGMVolumeSlider;
	[SerializeField] Slider SFXVolumeSlider;
	[SerializeField] TMP_InputField NicknameInputField;
	[SerializeField] TMP_Text NicknameText;
	[SerializeField] Button DiaryStage1;
	[SerializeField] Button DiaryStage2;
	[SerializeField] Button DiaryStage3;
	[SerializeField] TMPro.TMP_Text DiaryStage1Text;
	[SerializeField] TMPro.TMP_Text DiaryStage2Text;
	[SerializeField] TMPro.TMP_Text DiaryStage3Text;
	[SerializeField] TMPro.TMP_Text TotalLeavesText;
	[SerializeField] TMPro.TMP_Text TotalDeathsText;
	[SerializeField] Sprite DiaryStageNotClear;
	[SerializeField] Sprite DiaryStageClear;
	[SerializeField] TMPro.TMP_Text NoSaveDataText;
	[SerializeField] Button[] menuButtons;
	


	private void Awake()
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

	void Start()
	{
		BGMVolumeSlider.onValueChanged.AddListener(BGMVolumeChange);
		SFXVolumeSlider.onValueChanged.AddListener(SFXVolumeChange);
		SettingPanel.SetActive(false);
		DiaryPanel.SetActive(false);
		NoSaveDataText.gameObject.SetActive(false);
		NicknameText.text = PlayerPrefsManager.instance.GetNickname();
		GameManager.instance.SetUI(false);
	}

	public void BGMVolumeChange(float volume)
	{
		AudioManager.instance.SetBGMVolume(volume);
	}

	public void SFXVolumeChange(float volume)
	{
		AudioManager.instance.SetSFXVolume(volume);
	}


	public void NewGameButton()
	{
		PlayerPrefsManager.instance.DeleteAllData();
		GameManager.instance.LoadStage("Stage1");
		StartCoroutine(AllButtonBlock());
	}

	public void ResumeGameButton()
	{
		string stage = PlayerPrefsManager.instance.GetStage();
		if (stage == "")
		{
			if (PlayerPrefsManager.instance.GetClearData("Stage3") == 0)
			{
				NoSaveDataText.text = "No saved data!!";
				NoSaveData();
			}
			else
			{
				NoSaveDataText.text = "Game Clear!!\nPlease start new game!!";
				NoSaveData();
			}
			Debug.Log("No saved data");
		}
		else
		{
			GameManager.instance.LoadStage(stage);
		}
		StartCoroutine(AllButtonBlock());
	}

	public void NoSaveData()
	{
		StartCoroutine(NoSaveDataCoroutine());
	}

	IEnumerator NoSaveDataCoroutine()
	{
		NoSaveDataText.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		NoSaveDataText.gameObject.SetActive(false);
	}

	public void ExitGameButton()
	{
		#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
		#else
				Application.Quit();
		#endif
	}

	public void SettingButton()
	{
		if (SettingPanel.activeSelf)
		{
			PlayerPrefsManager.instance.SetNickname(NicknameInputField.text);
			NicknameText.text = PlayerPrefsManager.instance.GetNickname();
			SettingPanel.SetActive(false);
		}
		else
		{
			BGMVolumeSlider.value = AudioManager.instance.GetBGMVolume();
			SFXVolumeSlider.value = AudioManager.instance.GetSFXVolume();
			NicknameInputField.text = PlayerPrefsManager.instance.GetNickname();
			SettingPanel.SetActive(true);
		}
		StartCoroutine(AllButtonBlock());
	}

	public void CloseSettingButton()
	{
		PlayerPrefsManager.instance.SetNickname(NicknameInputField.text);
		NicknameText.text = PlayerPrefsManager.instance.GetNickname();
		SettingPanel.SetActive(false);
	}

	public void CloseDiaryButton()
	{
		DiaryPanel.SetActive(false);
	}

	public void DiaryButton()
	{
		if (DiaryPanel.activeSelf)
		{
			DiaryPanel.SetActive(false);
		}
		else
		{
			int stage1 = PlayerPrefsManager.instance.GetClearData("Stage1");
			int stage2 = PlayerPrefsManager.instance.GetClearData("Stage2");
			int stage3 = PlayerPrefsManager.instance.GetClearData("Stage3");
			if (stage1 == 0)
			{
				DiaryStage1.image.sprite = DiaryStageNotClear;
				DiaryStage1.interactable = false;
				DiaryStage1Text.text = "0 / 8";
			}
			else
			{
				DiaryStage1.image.sprite = DiaryStageClear;
				DiaryStage1.interactable = true;
				DiaryStage1Text.text = stage1 + " / 8";
			}
			if (stage2 == 0)
			{
				DiaryStage2.image.sprite = DiaryStageNotClear;
				DiaryStage2.interactable = false;
				DiaryStage2Text.text = "0 / 8";
			}
			else
			{
				DiaryStage2.image.sprite = DiaryStageClear;
				DiaryStage2.interactable = true;
				DiaryStage2Text.text = stage2 + " / 8";
			}
			if (stage3 == 0)
			{
				DiaryStage3.image.sprite = DiaryStageNotClear;
				DiaryStage3.interactable = false;
				DiaryStage3Text.text = "0 / 8";
			}
			else
			{
				DiaryStage3.image.sprite = DiaryStageClear;
				DiaryStage3.interactable = true;
				DiaryStage3Text.text = stage3 + " / 8";
			}
			TotalLeavesText.text = "Total Leaves Collected: " + (stage1 + stage2 + stage3) + " / 24";
			TotalDeathsText.text = "Total Deaths: " + PlayerPrefsManager.instance.GetDeathCount();
			DiaryPanel.SetActive(true);
		}
		StartCoroutine(AllButtonBlock());
	}

	public void ClickSound()
	{
		AudioManager.instance.PlayClick();
	}

	IEnumerator AllButtonBlock()
	{
		foreach (Button button in menuButtons)
		{
			button.interactable = false;
		}
		yield return new WaitForSeconds(1f);
		foreach (Button button in menuButtons)
		{
			button.interactable = true;
		}
	}
}
