using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
	public delegate void OnGrounded();
	public event OnGrounded onGrounded;


	public bool isGrounded { get; private set; }
    private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Ground")) {
			isGrounded = true;
			onGrounded?.Invoke();
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag("Ground")) {
			isGrounded = false;
		}
	}

}
