using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData
{
    public Vector3 position;
	public Vector2 velocity;
	public bool[] isEat;
	public int health;
	public bool[] isActive;

	public StageData(Vector3 position, Vector2 velocity, bool[] leaves, int health, bool[] isActive)
	{
		this.position = position;
		this.velocity = velocity;
		this.isEat = leaves;
		this.health = health;
		this.isActive = isActive;
	}
}
