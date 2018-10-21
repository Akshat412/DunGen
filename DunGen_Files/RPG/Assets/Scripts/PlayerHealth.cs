using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

	public int playerMaxHP;

	public int playerHP;

	public int playerMaxMana;

	public int playerMana;

	public PlayerStatSheet StatSheet;

	public CameraShake cameraShake;

	public GameObject Effect;

	public GameObject bloodEffect;

	public GameObject Player;

	private SFXManager SFX;

	//private float timeToReload = 1f;

	private bool reloading;

	IEnumerator FadingLevel()
	{
		float fadeTime = GameObject.Find("Fader").GetComponent<Fading>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
	}

	void Start()
	{
		PlayerHealthToMax();
		PlayerManaToMax();

		SFX = FindObjectOfType<SFXManager>();
	}

	void Update()
	{
		if(playerHP <= 0)
		{	
			StartCoroutine("FadingLevel");

			gameObject.SetActive(false);

            //Destroy everything that's not needed any more
            GameObject.Destroy(GameObject.Find("HealthUI"));
            GameObject.Destroy(GameObject.Find("ManaUI"));
            GameObject.Destroy(GameObject.Find("SFXManager"));
            SceneManager.LoadScene(3);
		}
	}

	public void DamagePlayer(int damage)
	{
		playerHP -= damage;

		float magnitude = (float)damage / 100;

		Instantiate(bloodEffect, transform.position, transform.rotation);

		StartCoroutine(cameraShake.Shake(0.15f, magnitude));

		SFX.playerHurt.Play();
		
		SFX.enemyHurt.Play();
	}

	public void HealPlayer(int multiplier = 1)
	{
		if(playerMana > 0)
		{
			if(playerHP + (StatSheet.healingPowers[StatSheet.Intelligence - 1] * multiplier) > playerMaxHP)
			{
				playerHP = playerMaxHP;
			}

			else
			{
				playerHP += StatSheet.healingPowers[StatSheet.Intelligence - 1] * multiplier;
			}

			Instantiate(Effect, Player.gameObject.transform.position, Player.gameObject.transform.rotation);

			playerMana -= 10;
		}

		else
		{
			//Do Nothing
		}
	}

	public void PlayerHealthToMax()
	{
		playerMaxHP = StatSheet.Strength * 20;
		playerHP = playerMaxHP;
	}

	public void PlayerManaToMax()
	{
		playerMaxMana = StatSheet.Intelligence * 20;
		playerMana = playerMaxMana;
	}
}
