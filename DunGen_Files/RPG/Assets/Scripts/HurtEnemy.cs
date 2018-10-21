using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour {

	public GameObject bloodEffect;

	public Transform PointOfImpact;

	public GameObject damageNumber;

	private int damage;

	public int dealtDamage;

	private PlayerController Controller;

    public int knockbackForce = 5;

    void Start()
    {
    	Controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

	void GetDamage()
	{
		damage = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponArray>().damageOfCurrentWeapon;
		
		dealtDamage = Controller.damageOfAttack + damage;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		GetDamage();

		if(other.gameObject.tag == "Enemy")
		{
			other.gameObject.GetComponent<EnemyHealth>().DamageEnemy(dealtDamage);
			Instantiate(bloodEffect, PointOfImpact.position, PointOfImpact.rotation);

            //draw a vector from this object's center of mass to the enemy's center of mass
            Vector2 force = Controller.atkVector;

            //Debug.Log(force);
            force *= 200;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            
            //Debug.Log(other.gameObject.name);
		}
	}
}
