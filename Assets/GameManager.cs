using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool gameHasEnded = false;
    public Text result;
    public Text wavesText;
    public static int waves;
    public static int maxWaves;
    public GameObject completeLevelUI;

    public void CompleteLevelUI() {
        completeLevelUI.SetActive(true);
    }

    public void LoseGame() {
        if (!gameHasEnded) {
            gameHasEnded = true;
            result.text = "FAILED";
            wavesText.text = "Wave: " + waves + " / " + maxWaves + " Completed";
        }
    }

    public void WonGame() {
        if (!gameHasEnded) {
            gameHasEnded = true;
            result.text = "COMPLETE";
        }
    }
}
