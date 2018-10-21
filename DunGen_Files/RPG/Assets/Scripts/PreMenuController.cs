using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreMenuController : MonoBehaviour {

    public InputField playerName;
    public Text nameError;




    IEnumerator FadingLevel() {
        float fadeTime = GameObject.Find("Fader").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
    }

    public void StartButton() {
        if(playerName.text.Length == 0 || playerName.text.Length == 40) {
            nameError.text = "Enter  a  valid  name  to  continue";
        }
        else {
            PlayerPrefs.SetString("Name", playerName.text);
            StartCoroutine("FadingLevel");
            Debug.Log("Man's not hot, but " + PlayerPrefs.GetString("Name") + " is.");
            SceneManager.LoadScene(1);
            
        }
        
    }


}