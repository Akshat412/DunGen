using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * ---------------------------------
 *          AI CONTROLLER (TYPE 3)
 * ---------------------------------
 * 
 *  Idle Behaviour
 *      Stay still till the unfortunate soul walks into aggro radius
 *  Aggro Behaviour
 *		Run towards the player if he is in the aggro radius
 */

public class EnemyAI3 : MonoBehaviour 
{
    public float moveSpeed;

    public Transform target;

    public float aggroRadius;

    private Rigidbody2D eRigidbody;

    public float attackTime;

    private float attackTimeCounter;

    public float waitTime;

    private float waitTimeCounter;

    private bool moving;

    private Animator eAnim;

    public Vector2 atkVector;

    private CameraShake cameraShake;

    void Start () 
	{
		eRigidbody = GetComponent<Rigidbody2D>();

		eAnim = GetComponent<Animator>();

		target = GameObject.FindGameObjectWithTag("Player").transform;

		cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
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

		setAtkVector();

		eAnim.SetFloat("MoveX", atkVector.x);

		eAnim.SetBool("Moving", moving);
	}

	void MoveToPlayer()
	{
		if(Vector2.Distance(transform.position, target.position) < aggroRadius)
		{
			if(moving)
			{
				attackTimeCounter -= Time.deltaTime;

				if(attackTimeCounter <= 0)
				{
					moving = false;

					waitTimeCounter = waitTime;
				}

				eRigidbody.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

				StartCoroutine(cameraShake.Shake(0.15f, 0.12f));
			}

			else
			{
				waitTimeCounter -= Time.deltaTime;

				if(waitTimeCounter <= 0)
				{
					moving = true;

					attackTimeCounter = attackTime;
				}
			}
		}

		else
		{
			eRigidbody.velocity = Vector3.zero;
		}
	}

	void setAtkVector() 
    {
        atkVector = (target.position - transform.position).normalized;
    }
}
