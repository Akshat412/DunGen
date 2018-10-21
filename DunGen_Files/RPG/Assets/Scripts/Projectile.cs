using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float moveSpeed;

	public int damage;

	public GameObject Effect;

	private Transform player;

	private Vector2 target;

    bool playerMissing;

	void Start () 
	{
        if ((GameObject.FindGameObjectWithTag("Player"))) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            target = new Vector2(player.position.x, player.position.y);
            playerMissing = false;
                }
        else {
            playerMissing = true;
        }
		
	}
	
	void Update () 
	{
		transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

		if(transform.position.x == target.x && transform.position.y == target.y)
		{
			DestroyProjectile();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
        if (!playerMissing) {
            EnemyAI2 script = other.GetComponent<EnemyAI2>();

            if (other.gameObject.name == "Player") {
                DestroyProjectile();

                other.GetComponent<PlayerHealth>().DamagePlayer(damage);
            }

            else if (script == null) {
                DestroyProjectile();
                if (other.GetComponent<EnemyHealth>()) {
                    other.GetComponent<EnemyHealth>().DamageEnemy(damage);
                }
            }

            else if (other.gameObject.name == "Swing_Effect") {
                DestroyProjectile();
            }
        }
		
	}

	void DestroyProjectile()
	{
		Instantiate(Effect, transform.position, transform.rotation);

		Destroy(gameObject);
	}
}
