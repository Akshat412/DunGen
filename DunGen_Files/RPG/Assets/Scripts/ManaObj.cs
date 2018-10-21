using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaObj : MonoBehaviour {

	public int boost = 20;

	public GameObject Effect;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.name == "Player")
		{
			if(other.gameObject.GetComponent<PlayerHealth>().playerMana + boost > other.gameObject.GetComponent<PlayerHealth>().playerMaxMana)
			{
				other.gameObject.GetComponent<PlayerHealth>().playerMana = other.gameObject.GetComponent<PlayerHealth>().playerMaxMana;
			}

			else
			{
				other.gameObject.GetComponent<PlayerHealth>().playerMana += boost;
			}

			Instantiate(Effect, other.gameObject.transform.position, other.gameObject.transform.rotation);

			Destroy(gameObject);
		}	
	}
}
