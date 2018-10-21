using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour 
{
	public PlayerStatSheet StatSheet;

	public GameObject Effect;

	public float moveSpeed;

	private Rigidbody2D pRigidBody;

	private bool moving;

	public Vector2 lastMove;

	private Vector2 Move;

	private Vector2 dir;

	private Animator pAnim;

	private static bool playerExists;

	private bool attacking;

	public float attackTime;

	private float attackTimeCounter;

	public int damageOfAttack;

	public GameObject Player;

	private float boostTimeCounter;

	private bool EffectIsOn;

	private SFXManager SFX;

    public Vector2 atkVector;

    private Vector3 point;

    private Vector3 screenPoint;

	void Mover()
	{
		moving = false;
		
		if (Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw ("Horizontal") < -0.5f) 
		{
			pRigidBody.velocity = new Vector2 (Input.GetAxisRaw ("Horizontal") * moveSpeed, pRigidBody.velocity.y);

			moving = true;

			Move.x = playerToCameraDir().x;
		}

		if (Input.GetAxisRaw ("Vertical") > 0.5f || Input.GetAxisRaw ("Vertical") < -0.5f) 
		{
			pRigidBody.velocity = new Vector2 (pRigidBody.velocity.x, Input.GetAxisRaw ("Vertical") * moveSpeed);

			moving = true;

			Move.y = playerToCameraDir().y;
        }

		if (Input.GetAxisRaw ("Horizontal") < 0.5f && Input.GetAxisRaw ("Horizontal") > -0.5f) 
		{
			pRigidBody.velocity = new Vector2 (0f, pRigidBody.velocity.y);

			Move.x = 0f;
		}

		if (Input.GetAxisRaw ("Vertical") < 0.5f && Input.GetAxisRaw ("Vertical") > -0.5f) 
		{
			pRigidBody.velocity = new Vector2 (pRigidBody.velocity.x ,0f);

			Move.y = 0f;
		}

		lastMove = playerToCameraDir();
	}

	void CalcDamage(int multiplier = 1)
	{
		damageOfAttack = StatSheet.damages[StatSheet.Strength - 1] * multiplier;
	}

	void Attack()
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			CalcDamage(2);

			attackTimeCounter = attackTime;

			attacking = true;

			pRigidBody.velocity = new Vector2(0f, 0f);

			SFX.swordSwing.Play();

			if(Math.Abs(transform.position.y - point.y) >= 2f)
			{
				pAnim.SetBool("Attacking UD", true);

				//Debug.Log("UD : " + Vector2.Distance(transform.position, dir));
			}

			else
			{
				pAnim.SetBool("Attacking LR", true);

				//Debug.Log("LR : " + Vector2.Distance(transform.position, dir));
			}
		}

		else if(Input.GetKeyDown(KeyCode.H))
		{
			attackTimeCounter = 0.4f;

			pRigidBody.velocity = new Vector2(0f, 0f);

			attacking = true;

			Player.gameObject.GetComponent<PlayerHealth>().HealPlayer(1);
		}
	}

	void CheckPlayerExistence()
	{
		if(!playerExists)
		{
			playerExists = true;

			DontDestroyOnLoad(transform.gameObject);
		}

		else
		{
			Destroy(gameObject);
		}
	}

	void SetPlayerLevels()
	{
		moveSpeed = StatSheet.speeds[StatSheet.Stamina - 1];

		attackTime = StatSheet.attackTimes[StatSheet.Stamina - 1];
	}

	void Start ()
	{
		SetPlayerLevels();

		pRigidBody = GetComponent<Rigidbody2D>();

		pAnim = GetComponent<Animator>();

		CheckPlayerExistence();	

		lastMove = new Vector2(1f, 0f);

		SFX = FindObjectOfType<SFXManager>();
	}

	void Update () 
	{
		point = new Vector3();

		screenPoint = Input.mousePosition;

		point = Camera.main.ScreenToWorldPoint(screenPoint);

        playerToCameraDir();

        if (!attacking)
		{
			Mover();
			Attack();
		}

		if(attackTimeCounter > 0)
		{
			attackTimeCounter -= Time.deltaTime;
		}

		if(attackTimeCounter <= 0)
		{
			attacking = false;

			pAnim.SetBool("Attacking UD", false);

			pAnim.SetBool("Attacking LR", false);
		}
		
		pAnim.SetFloat("MoveX", Move.x);

		pAnim.SetFloat("MoveY", Move.y);

		pAnim.SetFloat("LastX", lastMove.x);

		pAnim.SetFloat("LastY", lastMove.y);

		pAnim.SetBool("Moving", moving);

        setAtkVector();

    }

    void setAtkVector()
    {
        if(pRigidBody.velocity == Vector2.zero) 
        {
            return;
        }

        else 
        {
            atkVector = pRigidBody.velocity;
        }
    }

    // -1 is looking left, 1 is looking right, 0 is fail

    Vector2 playerToCameraDir() 
    {
    	dir = Vector2.zero;

        if (point.x < transform.position.x) 
        {            
        	dir.x = -1f;
        }

        if(point.x > transform.position.x) 
        {
        	dir.x = 1f;
        }

        if(point.y < transform.position.y) 
        {
        	dir.y = -1f;
        }

        if(point.y > transform.position.y) 
        {
        	dir.y = 1f;
        }

        return dir;
    }
}
