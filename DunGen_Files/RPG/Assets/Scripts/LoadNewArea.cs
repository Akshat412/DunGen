using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewArea : MonoBehaviour {

	public string level_to_load;
    public BSPLevelGenController bsp;


    void Start() {
        bsp = GameObject.FindGameObjectWithTag("proceduralGenerator").GetComponent<BSPLevelGenController>();
    }

	IEnumerator fadeIn()
	{
		float fadeTime = GameObject.Find("Fader").GetComponent<Fading>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
        //bsp.levelEndShowUI();
        //bsp.BSP_initLevel();
        float fadeTime2 = GameObject.Find("Fader").GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime2);
    }

    IEnumerator fadeOut() {
        float fadeTime = GameObject.Find("Fader").GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }

	void OnTriggerEnter2D(Collider2D Trigger)
	{
		if (Trigger.gameObject.name == "Player") 
		{
            //StartCoroutine(fadeIn());

            bsp.levelEndShowUI();
            //bsp.BSP_initLevel();
            //SceneManager.LoadScene(level_to_load);
            //bsp.BSP_initLevel();
            //StartCoroutine(fadeOut());
            //StartCoroutine(newMap());
        }
	}



    IEnumerator newMap() {
        yield return StartCoroutine(fadeIn());
        bsp.levelEndShowUI();
        bsp.BSP_initLevel();
        yield return StartCoroutine(fadeOut());
    }
}
