using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liana : MonoBehaviour
{
    private CircleCollider2D lianaCollider;
	private Animator animator;
	[SerializeField] float detectRange = 3f;

	private int face = 1;


    void Start()
	{
		lianaCollider = GetComponent<CircleCollider2D>();
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
                float distanceToPlayer = transform.position.x - player.transform.position.x;

                if (distanceToPlayer <= detectRange * face)
				{
					animator.SetTrigger("Attack");
					AudioManager.instance.PlayLianaAttack();
				}
				yield return new WaitForSeconds(5f);
            }

            yield return new WaitForSeconds(0.1f);
        }
	}

	public void EnableCollider()
	{
		lianaCollider.enabled = true;
	}

	public void DisableCollider()
	{
		lianaCollider.enabled = false;
	}
}
