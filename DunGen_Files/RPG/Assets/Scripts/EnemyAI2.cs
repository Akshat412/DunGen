using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * ---------------------------------
 *          AI CONTROLLER (TYPE 1)
 * ---------------------------------
 * 
 *  Idle Behaviour
 *      Stay still till the unfortunate soul walks into aggro radius
 *  Aggro Behaviour
 *		Get close to the player then fire stuff at player's last position
 */

public class EnemyAI2 : MonoBehaviour 
{
    public float moveSpeed;

    private Transform target;

    public float aggroRadius;

    public float stopRadius;

    private Rigidbody2D eRigidbody;

    public GameObject Projectile;

    public float TimeBetweenShots;
    
    private float TimeBetweenShotsCounter;

    void Start () 
	{
		eRigidbody = GetComponent<Rigidbody2D>();

		target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void Update () 
	{
		if(!GetComponent<EnemyHealth>().EnemyIsHurt)
		{
			MoveToPlayer();
		}

		else
		{
			eRigidbody.velocity = Vector3.zero;
		}
	}

	void MoveToPlayer()
	{
		if(Vector2.Distance(transform.position, target.gameObject.transform.position) < aggroRadius && Vector2.Distance(transform.position, target.gameObject.transform.position) > stopRadius)
		{
			eRigidbody.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
		}

		else
		{
			eRigidbody.velocity = Vector3.zero;
		}

		if(Vector2.Distance(transform.position, target.gameObject.transform.position) < aggroRadius)
		{
			FireAtPlayer();
		}
	}

	void FireAtPlayer()
	{
		if(TimeBetweenShotsCounter <= 0)
		{
			Instantiate(Projectile, transform.position, Quaternion.identity);
			
			TimeBetweenShotsCounter = TimeBetweenShots;
		}

		else
		{
			TimeBetweenShotsCounter -= Time.deltaTime;
		}
	}
}
