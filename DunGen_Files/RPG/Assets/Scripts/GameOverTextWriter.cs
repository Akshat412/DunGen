using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverTextWriter : MonoBehaviour {

    public Text GOText;

	// Use this for initialization
	void Start () {
        int score = PlayerPrefs.GetInt("GameScore");
        int highScore = PlayerPrefs.GetInt("HighScore");

        if(score >= highScore) {
            GOText.text = "Well  done,  " + PlayerPrefs.GetString("Name") + ".  You  scored  " + score + "  points.  This  is  the  high  score.";
        }
        else {
            GOText.text = "Alas,  you  made  it  only  as  far  as  " + score + "  points.";
        }
	}
	
    public void replayButton() {
        SceneManager.LoadScene(2);
    }

    public void quitButton() {
        Application.Quit();
    }
}
