using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
	private bool flag = false;
    void OnTriggerEnter2D(Collider2D other)
	{
		if (GameManager.instance.GetLeafCnt() < 5)
		{
			Debug.Log("Not enough leaves");
			GameManager.instance.NotEnoughLeaf();
			return;
		}
		if (other.CompareTag("Player") && !flag)
		{
			flag = true;
			GameManager.instance.StageClear();
		}
	}
}
