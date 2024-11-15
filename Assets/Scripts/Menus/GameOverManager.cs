using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    public bool gameOver;
    public bool gameWon;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }



    public void TriggerGameOver()
    {
        gameOver = true;
        SceneManager.LoadScene("CanvasScene");
    }


    public void TriggerVictory()
    {
        gameWon = true;
        SceneManager.LoadScene("CanvasScene");
    }
}
