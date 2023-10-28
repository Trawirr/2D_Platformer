using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_GAME_OVER, GS_LEVELCOMPLETED };

public class GameManager : MonoBehaviour
{
    public GameState currentGameState;
    public static GameManager instance;
    public Canvas inGameCanvas;
    public TMP_Text coinsText;
    private int coins = 0;

    public Image[] keysTab;
    private int keys = 0;

    public Image[] livesTab;
    public int maxLives = 4;
    private int lives = 3;

    private float timer = 0.0f;
    public TMP_Text timerText;

    public TMP_Text enemiesText;
    private int enemies = 0;

    public Canvas pauseMenuCanvas;

    private float changeSceneTimer = 1.0f;

    public void FinishGame()
    {
        if (keys == 3)
        {
            Debug.Log("You won!");
        }
        else
        {
            Debug.Log("You have found " + keys + "/3 keys");
        }
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExitButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateTimer()
    {
        timerText.text = string.Format("{0:00}:{1:00}", (timer - timer % 60) / 60, timer % 60);
    }

    public void AddEnemies()
    {
        enemies++;
        if (enemies < 10) enemiesText.text = '0' + enemies.ToString();
        else enemiesText.text = enemies.ToString();
    }

    public void AddLives()
    {
        lives++;
        UpdateLives();
    }

    public void LoseLives()
    {
        lives--;
        UpdateLives();
    }

    public void UpdateLives()
    {
        for (int i=0; i<maxLives; i++)
        {
            if (i < lives) livesTab[i].enabled = true;
            else livesTab[i].enabled = false;
        }
    }

    public void AddKeys()
    {
        keysTab[keys].color = Color.yellow;
        keys++;
    }

    public void AddCoins()
    {
        coins++;
        if (coins < 10) coinsText.text = '0' + coins.ToString();
        else coinsText.text = coins.ToString();
    }

    void SetGameState(GameState newGameState)
    {
        if (changeSceneTimer < 0.25f) return;
        changeSceneTimer = 0.0f;
        currentGameState = newGameState;
        if (newGameState == GameState.GS_GAME)
        {
            inGameCanvas.enabled = true;
        }
        else
        {
            inGameCanvas.enabled = false;
        }

        if (newGameState == GameState.GS_PAUSEMENU)
        {
            pauseMenuCanvas.enabled = true;
        }
        else
        {
            pauseMenuCanvas.enabled = false;
        }
    }

    void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }

    void Awake()
    {
        instance = this;
        foreach(Image k in keysTab)
        {
            k.color = Color.grey;
        }
        UpdateLives();
    }

    // Start is called before the first frame update
    void Start()
    {
        //PauseMenu();
        InGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (currentGameState == GameState.GS_PAUSEMENU) InGame();
            else if (currentGameState == GameState.GS_GAME) PauseMenu();
        }
        changeSceneTimer += Time.deltaTime;
        if (currentGameState == GameState.GS_GAME)
        {
            timer += Time.deltaTime;
            UpdateTimer();
        }
    }
}
