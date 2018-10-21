using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour {

	public int damage;

	void Start()
	{

	}

	void Update()
	{

	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.name == "Player")
		{
			other.gameObject.GetComponent<PlayerHealth>().DamagePlayer(damage);
		}
	}
}
