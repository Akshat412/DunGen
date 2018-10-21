using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerMana : MonoBehaviour {

	public Slider ManaBar;

	public TMP_Text ManaText;

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
		ManaBar.maxValue = Health.playerMaxMana;

		ManaBar.value = Health.playerMana;

		if(Health.playerMana > 0)
		{
			ManaText.text = "Mana : " + Health.playerMana + "/" + Health.playerMaxMana; 
		}

		else
		{
			ManaText.text = "Mana : " + "0" + "/" + Health.playerMaxMana; 
		}
	}
}
