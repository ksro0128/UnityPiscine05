using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

	private AudioSource BGMSource;
	[SerializeField] AudioClip BGM;

	private AudioSource jumpSource;
	[SerializeField] AudioClip jump;

	private AudioSource takeDamageSource;
	[SerializeField] AudioClip takeDamage;

	private AudioSource defeatSource;
	[SerializeField] AudioClip defeat;

	private AudioSource respawnSource;
	[SerializeField] AudioClip respawn;

	private AudioSource LianaAttackSource;
	[SerializeField] AudioClip LianaAttack;

	private AudioSource CactusAttackSource;
	[SerializeField] AudioClip CactusAttack;

	private AudioSource ClickSource;
	[SerializeField] AudioClip Click;

	private AudioSource GetLeafSource;
	[SerializeField] AudioClip GetLeaf;

	
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this)
			Destroy(gameObject);
	}

	void Start()
	{
		BGMSource = gameObject.AddComponent<AudioSource>();
		BGMSource.clip = BGM;
		BGMSource.loop = true;
		BGMSource.volume = PlayerPrefsManager.instance.GetBGMVolume();

		jumpSource = gameObject.AddComponent<AudioSource>();
		jumpSource.clip = jump;
		jumpSource.volume = PlayerPrefsManager.instance.GetSFXVolume();
		
		takeDamageSource = gameObject.AddComponent<AudioSource>();
		takeDamageSource.clip = takeDamage;
		takeDamageSource.volume = PlayerPrefsManager.instance.GetSFXVolume();

		defeatSource = gameObject.AddComponent<AudioSource>();
		defeatSource.clip = defeat;
		defeatSource.volume = PlayerPrefsManager.instance.GetSFXVolume();

		respawnSource = gameObject.AddComponent<AudioSource>();
		respawnSource.clip = respawn;
		respawnSource.volume = PlayerPrefsManager.instance.GetSFXVolume();

		LianaAttackSource = gameObject.AddComponent<AudioSource>();
		LianaAttackSource.clip = LianaAttack;
		LianaAttackSource.volume = PlayerPrefsManager.instance.GetSFXVolume();

		CactusAttackSource = gameObject.AddComponent<AudioSource>();
		CactusAttackSource.clip = CactusAttack;
		CactusAttackSource.volume = PlayerPrefsManager.instance.GetSFXVolume();

		ClickSource = gameObject.AddComponent<AudioSource>();
		ClickSource.clip = Click;
		ClickSource.volume = PlayerPrefsManager.instance.GetSFXVolume();

		GetLeafSource = gameObject.AddComponent<AudioSource>();
		GetLeafSource.clip = GetLeaf;
		GetLeafSource.volume = PlayerPrefsManager.instance.GetSFXVolume();
	}

	public void PlayBGM()
	{
		BGMSource.Play();
	}

	public void StopBGM()
	{
		BGMSource.Stop();
	}

	public void PlayJump()
	{
		jumpSource.Play();
	}

	public void PlayTakeDamage()
	{
		takeDamageSource.Play();
	}

	public void PlayDefeat()
	{
		defeatSource.Play();
	}

	public void PlayRespawn()
	{
		respawnSource.Play();
	}

	public void PlayLianaAttack()
	{
		LianaAttackSource.Play();
	}

	public void PlayCactusAttack()
	{
		CactusAttackSource.Play();
	}

	public void PlayClick()
	{
		ClickSource.Play();
	}

	public void PlayGetLeaf()
	{
		GetLeafSource.Play();
	}
	
	public void SetBGMVolume(float volume) {
		BGMSource.volume = volume;
		PlayerPrefsManager.instance.SetBGMVolume(volume);
	}

	public void SetSFXVolume(float volume) {
		jumpSource.volume = volume;
		takeDamageSource.volume = volume;
		defeatSource.volume = volume;
		respawnSource.volume = volume;
		LianaAttackSource.volume = volume;
		CactusAttackSource.volume = volume;
		ClickSource.volume = volume;
		GetLeafSource.volume = volume;
		PlayerPrefsManager.instance.SetSFXVolume(volume);
	}

	public float GetBGMVolume() {
		return BGMSource.volume;
	}

	public float GetSFXVolume() {
		return jumpSource.volume;
	}


}
