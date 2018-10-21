using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextMap : MonoBehaviour {

    public ProceduralGenController pcg_controller;

	void Start () {
        pcg_controller = GameObject.Find("LevelGenerator").GetComponent<ProceduralGenController>();
	}

    void OnTriggerEnter2D(Collider2D Trigger) {
        if (Trigger.gameObject.name == "Player") {
            GameObject.Destroy(pcg_controller.levelHolderInstance);
            pcg_controller.levelHolderInstance = Instantiate(pcg_controller.levelHolderTemplate);
            pcg_controller.currentLevel += 1;
            pcg_controller.seed = pcg_controller.seed + pcg_controller.currentLevel;
            pcg_controller.initLevel(pcg_controller.seed);
        }
    }

    IEnumerator FadingLevel() {
        float fadeTime = GameObject.Find("Fader").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
    }
}
