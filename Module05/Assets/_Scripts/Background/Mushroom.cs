using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
	private Animator animator;
	[SerializeField] GameObject ob;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			animator.SetBool("Pressed", true);
			StartCoroutine(PressUp());
			ob.SetActive(false);
		}
	}

	IEnumerator PressUp()
	{
		yield return new WaitForSeconds(5f);
		animator.SetBool("Pressed", false);	
	}
}
