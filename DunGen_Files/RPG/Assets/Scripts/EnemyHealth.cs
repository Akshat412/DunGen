using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public int enemyMaxHP = 50;

	public int enemyHP;

	public bool EnemyIsHurt;

	public GameObject WeaponSpawner;

	private float enemyKnockbackCounter;

	public float enemyKnockback;

	private SFXManager SFX;

	public GameObject damageNumber;

	void Start()
	{
		EnemyHealthToMax();

		SFX = FindObjectOfType<SFXManager>();
	}

	void Update()
	{
		if(enemyHP <= 0)
		{
			int ID, chance;

			//if(enemyMaxHP > 120) ID = 5;
			//else if(enemyMaxHP > 100) ID = 4;
			//else if(enemyMaxHP > 80) ID = 3;
			//else if(enemyMaxHP > 60) ID = 2;
			//else if(enemyMaxHP > 40) ID = 1;
			//else ID = 0;

			ID = 5;

			chance = (int)Random.Range(0, 2);

			if(chance == 1)
			{
				WeaponSpawner.GetComponent<Weapon>().weaponID = (int)Random.Range(0, ID);

				Instantiate(WeaponSpawner, transform.position, transform.rotation);
			}

			Destroy(gameObject);
		}

		if(enemyKnockbackCounter <= 0)
		{
			EnemyIsHurt = false;
		}

		else
		{
			enemyKnockbackCounter -= Time.deltaTime;
		}
	}

	public void DamageEnemy(int damage)
	{
		enemyHP -= damage;

		EnemyIsHurt = true;

		enemyKnockbackCounter = enemyKnockback;

		var clone = (GameObject)Instantiate(damageNumber, transform.position, Quaternion.Euler(Vector3.zero));

		clone.transform.position = transform.position;

		clone.GetComponent<FloatingNumbers>().number = damage;

		SFX.enemyHurt.Play();
	}

	public void EnemyHealthToMax()
	{
		enemyHP = enemyMaxHP;
	}
}
