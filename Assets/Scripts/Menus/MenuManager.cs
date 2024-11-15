using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    public GameObject GameOverBG;
    public GameObject VictoryBG;
    public GameObject PlayBTN;
    public GameObject RetryBTN;
    public GameObject QuitBTN;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (GameOverManager.Instance != null)
        {
            if (GameOverManager.Instance.gameOver)
                ShowGameOverScreen();
            else if (GameOverManager.Instance.gameWon)
                ShowVictoryScreen();
        }
    }

    public void ShowGameOverScreen()
    {

        GameOverBG.SetActive(true);
        VictoryBG.SetActive(false);
        PlayBTN.SetActive(false);
        RetryBTN.SetActive(true);
        QuitBTN.SetActive(true);
    }

    public void ShowVictoryScreen()
    {
        GameOverBG.SetActive(false);
        VictoryBG.SetActive(true);
        PlayBTN.SetActive(false);
        RetryBTN.SetActive(true);
        QuitBTN.SetActive(true);
    }

    public void RetryLevel()
    {
        GameOverBG.SetActive(false);
        VictoryBG.SetActive(false);
        PlayBTN.SetActive(false);
        RetryBTN.SetActive(false);
        QuitBTN.SetActive(false);

        GameOverManager.Instance.gameOver = false;
        GameOverManager.Instance.gameWon = false;

        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
