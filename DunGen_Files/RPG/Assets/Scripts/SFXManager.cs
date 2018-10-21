using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {

	public AudioSource playerHurt;
	public AudioSource enemyHurt;
	public AudioSource swordSwing;

	private static bool SFXManagerExists;

	void Start()
	{
		if(!SFXManagerExists)
		{
			SFXManagerExists = true;
			DontDestroyOnLoad(transform.gameObject);
		}

		else
		{
			Destroy(gameObject);
		}
	}
}
