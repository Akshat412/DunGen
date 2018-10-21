using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEndUIController : MonoBehaviour {

    public GameObject bsp;
    public Text enemies;
    public Text potions;
    public Text score;

    void update() {
        //LevelStats lvl_s = bsp.GetComponent<BSPLevelGenController>().generateStats();
        //int potions_drank = lvl_s.drankHealthPotions + lvl_s.drankManaPotions;
        //int potions_total = lvl_s.totalHealthPotions + lvl_s.totalManaPotions;
        //enemies.text = "Enemies  Killed: " + lvl_s.killedMonsters + "/" + lvl_s.totalMonsters;
        //potions.text = "Potions  Drank: " + potions_drank + "/" + potions_total;
        //score.text = "Total  score: " + PlayerPrefs.GetInt("GameScore");
    }

    public void showStats(int enemyKill, int enemyTotal, int potionDrank, int potionTotal, int totalScore) {
        enemies.text = "Enemies  Killed: " + enemyKill + "/" + enemyTotal;
        potions.text = "Potions  Drank: " + potionDrank + "/" + potionTotal;
        score.text = "Total  score: " + totalScore;
    }

	public void continuePlaying() {
        Time.timeScale = 1f;
        bsp.GetComponent<BSPLevelGenController>().BSP_initLevel();
    }

    public void stahp() {
        SceneManager.LoadScene(3);
    }
}
