using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOvertime : MonoBehaviour {

	public float TimeToDestroy;

	void Start()
	{
		
	}

	void Update()
	{
		TimeToDestroy -= Time.deltaTime;

		if(TimeToDestroy <= 0)
		{
			Destroy(gameObject);
		}
	}
}
