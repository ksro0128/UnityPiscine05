using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSpawner : MonoBehaviour
{

	[SerializeField] GameObject[] spawnPoints;
	[SerializeField] GameObject leafPrefab;
	[SerializeField] float spawnRate = 5f;

    void Start()
    {
        InvokeRepeating("SpawnLeaf", 0, spawnRate);
    }

    void SpawnLeaf()
	{
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			StartCoroutine(SpawnLeafRandomTime(i));
		}
	}

	IEnumerator SpawnLeafRandomTime(int index)
	{
		yield return new WaitForSeconds(Random.Range(2, 5));
		Instantiate(leafPrefab, spawnPoints[index].transform.position, Quaternion.identity);
	}
}
