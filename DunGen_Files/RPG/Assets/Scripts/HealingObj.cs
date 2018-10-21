using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingObj : MonoBehaviour {

	public int healing;

	public GameObject Effect;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.name == "Player")
		{
			if(other.gameObject.GetComponent<PlayerHealth>().playerHP + healing > other.gameObject.GetComponent<PlayerHealth>().playerMaxHP)
			{
				other.gameObject.GetComponent<PlayerHealth>().playerHP = other.gameObject.GetComponent<PlayerHealth>().playerMaxHP;
			}

			else
			{
				other.gameObject.GetComponent<PlayerHealth>().playerHP += healing;
			}

			Instantiate(Effect, other.gameObject.transform.position, other.gameObject.transform.rotation);

			Destroy(gameObject);
		}	
	}
}
