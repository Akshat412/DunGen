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
 *		Chase the player till he's dead [or the ghost is dead]
 */

public class EnemyAI1 : MonoBehaviour 
{
    public float moveSpeed;

    public Transform target;

    public float aggroRadius;

    private bool aggro;

    private bool moving;

    private Animator eAnim;

    public Vector2 atkVector;

    private Rigidbody2D eRigidbody;

    void Start () 
	{
		eRigidbody = GetComponent<Rigidbody2D>();

		eAnim = GetComponent<Animator>();

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

		setAtkVector();

		eAnim.SetBool("Aggro", aggro);

		eAnim.SetBool("Moving", moving);

		eAnim.SetFloat("MoveX", atkVector.x);
	}		

	void MoveToPlayer()
	{
		if(Vector2.Distance(transform.position, target.position) < aggroRadius)
		{
			eRigidbody.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

			moving = true;

			aggro = true;
		}

		else
		{
			eRigidbody.velocity = Vector3.zero;

			moving = false;

			aggro = false;
		}
	}

	void setAtkVector() 
    {
    	atkVector = (target.position - transform.position).normalized;
    }
}
