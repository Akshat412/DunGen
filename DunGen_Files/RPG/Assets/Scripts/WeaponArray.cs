using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponArray : MonoBehaviour {

	public int currentWeaponID;

	public GameObject[] weapons;

	public int[] weaponDamages;

	public int damageOfCurrentWeapon;

	void Start()
	{
		currentWeaponID = 0;
	}

	void Update()
	{
		for(int i = 0; i < weapons.Length; i++)
		{
			if(i == currentWeaponID)
			{
				weapons[i].SetActive(true);
			}

			else
			{
				weapons[i].SetActive(false);
			}
		}

		damageOfCurrentWeapon = weaponDamages[currentWeaponID];
	}
}
