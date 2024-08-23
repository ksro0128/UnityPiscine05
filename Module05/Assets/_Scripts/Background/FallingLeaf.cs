using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingLeaf : MonoBehaviour
{
    [SerializeField] float speed = 2f;

    // Update is called once per frame
    void Update()
    {
		transform.Translate(Vector3.down * speed * Time.deltaTime);
		if (transform.position.y < -5)
		{
			Destroy(gameObject);
		}
        
    }
}
