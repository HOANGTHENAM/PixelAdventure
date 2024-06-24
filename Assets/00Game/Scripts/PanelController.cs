using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class PanelController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject selectedCharacter;
    PlayerController playerController;
    [SerializeField] GameObject scoreObj;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text scoreGameOverText;
    [SerializeField] TMP_Text highscoreText;
    [SerializeField] Button replayBtn;
    [SerializeField] GameObject panelGameOver;
    [SerializeField] Button resetBtn;
    [SerializeField] Button quitGameBtn;
    private int score;
    private int highscore = 0;

    public Action OnChildObjectSpawned;
    bool playerDie;

    private void Start()
    {
        highscore = PlayerPrefs.GetInt("Highscore", 0);
        UpdateUI();
        replayBtn.onClick.AddListener(Replay);
        resetBtn.onClick.AddListener(ResetHighScore);
        quitGameBtn.onClick.AddListener(QuitGame);
    }
    private void Update()
    {

        PlayerController player = selectedCharacter.GetComponentInChildren<PlayerController>();
        if (player != null )
        {
            playerController = player;
        }
       
        if (playerController != null)
        {
            score = playerController.score;
            if (gameManager.isGameOver)
            {
                panelGameOver.SetActive(true);
                scoreObj.SetActive(false);
            }
        }
        UpdateHighScore();
        UpdateUI();
    }
    public void UpdateHighScore()
    {
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("Highscore", highscore);
        }
    }
    public void Replay()
    {
        Time.timeScale = 1f;
        playerController.score = 0;
        panelGameOver.SetActive(false);
        SceneManager.LoadScene("Menu");
    }

    public void ResetHighScore()
    {
        score = 0;
        highscore = 0;
        PlayerPrefs.SetInt("Highscore", highscore);
        UpdateUI();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    private void UpdateUI()
    {
        scoreText.text = "SCORE: " + score;
        scoreGameOverText.text = "SCORE: " + score;
        highscoreText.text = "HIGHSCORE: " + highscore;
    }
}
