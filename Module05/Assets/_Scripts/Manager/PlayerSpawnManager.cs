using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager instance;

	[SerializeField] GameObject playerPrefab;
	private GameObject player;

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

	public void RespawnPlayer()
	{
		if (playerPrefab == null)
		{
			return;
		}
		player = Instantiate(playerPrefab, transform.position, Quaternion.identity, transform);
	}

	public Vector3 GetPlayerPosition()
	{
		if (player == null)
		{
			return Vector3.zero;
		}
		return player.transform.position;
	}

	public Vector2 GetPlayerVelocity()
	{
		if (player == null)
		{
			return Vector2.zero;
		}
		return player.GetComponent<Rigidbody2D>().velocity;
	}

}
