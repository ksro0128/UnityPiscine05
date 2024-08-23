using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{
	private BoxCollider2D cactusCollider;
	private Animator animator;
	[SerializeField] float detectRange = 3f;


    void Start()
	{
		cactusCollider = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator>();
		StartCoroutine(DetectPlayer());
	}

	IEnumerator DetectPlayer()
	{
		while (true)
		{
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

                if (distanceToPlayer <= detectRange)
                {
                    animator.SetTrigger("Attack");
					AudioManager.instance.PlayCactusAttack();
                }
				yield return new WaitForSeconds(2f);
            }

            yield return new WaitForSeconds(0.1f);
        }
	}

	void Update()
	{
	}

	public void EnableCollider()
	{
		cactusCollider.enabled = true;
	}

	public void DisableCollider()
	{
		cactusCollider.enabled = false;
	}

	public void SetColliderSize()
	{
		cactusCollider.offset = new Vector2(0.01365542f, 1.063043f);
		cactusCollider.size = new Vector2(2.173313f, 2.120782f);
	}

	public void ExpandCollider1()
	{
		cactusCollider.offset = new Vector2(0.000000f, 1.363477f);
		cactusCollider.size = new Vector2(2.528371f, 2.721651f);
	}

	public void ExpandCollider2()
	{
		cactusCollider.offset = new Vector2(0.04711771f, 1.774071f);
		cactusCollider.size = new Vector2(3.322635f, 3.542839f);
	}
	
	public void ExpandCollider3()
	{
		cactusCollider.offset = new Vector2(-0.05384779f, 2.34621f);
		cactusCollider.size = new Vector2(4.843852f, 4.687117f);
	}	
}
