using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	IEnumerator FadingLevel()
	{
		float fadeTime = GameObject.Find("Fader").GetComponent<Fading>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
	}

	public void StartButton()
	{
		StartCoroutine("FadingLevel");
		SceneManager.LoadScene(2);
	}

	public void QuitButton()
	{
		StartCoroutine("FadingLevel");
		Application.Quit();

		Debug.Log("Quit");
	}
}
