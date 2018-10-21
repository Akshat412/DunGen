using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerHealth : MonoBehaviour {

	public Slider healthBar;

	public TMP_Text HealthText;

	private PlayerHealth Health;

	private static bool UIExists;

	void Start()
	{
		if(!UIExists)
		{
			UIExists = true;

			DontDestroyOnLoad(transform.gameObject);
		}

		else
		{
			Destroy(gameObject);
		}

		Health = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerHealth>();
	}

	void Update()
	{
		healthBar.maxValue = Health.playerMaxHP;

		healthBar.value = Health.playerHP;

		if(Health.playerHP > 0)
		{
			HealthText.text = "HP : " + Health.playerHP + "/" + Health.playerMaxHP; 
		}

		else
		{
			HealthText.text = "HP : " + "0" + "/" + Health.playerMaxHP; 
		}	
	}
}
