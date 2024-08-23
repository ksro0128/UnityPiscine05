using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    private int num = -1;

	void Start()
	{
		
	}

	public void SetNum(int num)
	{
		this.num = num;
	}

	public int GetNum()
	{
		return num;
	}
}
