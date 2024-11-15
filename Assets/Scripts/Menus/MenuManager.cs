using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    public GameObject GameOverBG;
    public GameObject VictoryBG;
    public GameObject RetryBTN;
    public GameObject QuitBTN;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void ShowGameOverScreen()
    {
        GameOverBG.SetActive(true);
        VictoryBG.SetActive(false);
        RetryBTN.SetActive(true);
        QuitBTN.SetActive(true);
    }

    public void ShowVictoryScreen()
    {
        GameOverBG.SetActive(false);
        VictoryBG.SetActive(true);
        RetryBTN.SetActive(true);
        QuitBTN.SetActive(true);
    }

public void RetryLevel()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}