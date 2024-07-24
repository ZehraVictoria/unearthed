using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject gameWonScreen;

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameLost()
    {
        gameOverScreen.SetActive(true);
    }

    public void gameWon()
    {
        gameWonScreen.SetActive(true);
    }
}
