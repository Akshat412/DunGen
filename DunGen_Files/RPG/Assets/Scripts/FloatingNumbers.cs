using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingNumbers : MonoBehaviour {

	public float floatingSpeed;

	public int number;

	public TMP_Text displayText;

	void Start()
	{

	}

	void Update()
	{
		displayText.text = "" + number;

		transform.position = new Vector3(transform.position.x, transform.position.y + (floatingSpeed * Time.deltaTime), transform.position.z);
	}
}
