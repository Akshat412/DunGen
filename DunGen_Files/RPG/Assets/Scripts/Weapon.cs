using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public int weaponID;

	public GameObject[] weapons;

	private float time;

	public GameObject Effect;

	void Start()
	{
		time = 5f;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.name == "Player")
		{
			int temp;

			temp = other.gameObject.GetComponent<WeaponArray>().currentWeaponID;

			other.gameObject.GetComponent<WeaponArray>().currentWeaponID = weaponID;

			weaponID = temp;
		}
	}

	void Update()
	{
		time -= Time.deltaTime;

		if(time <= 0)
		{
			Instantiate(Effect, gameObject.transform.position, gameObject.transform.rotation);

			Destroy(gameObject);
		}

		for(int i = 0; i < weapons.Length; i++)
		{
			if(i == weaponID)
			{
				weapons[i].SetActive(true);
			}

			else
			{
				weapons[i].SetActive(false);
			}
		}
	}
}
