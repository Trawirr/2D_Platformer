using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_GAME_OVER, GS_LEVELCOMPLETED, GS_OPTIONS };

public class GameManager : MonoBehaviour
{
    public GameState currentGameState;
    public static GameManager instance;
    public Canvas inGameCanvas;
    public TMP_Text coinsText;
    private int coins = 0;

    public Image[] keysTab;
    private int keys = 0;
    public int maxKeysNumber = 3;
    public bool keysCompleted = false;

    public Image[] livesTab;
    public int maxLives = 4;
    public int lives = 3;

    private float timer = 0.0f;
    public TMP_Text timerText;

    public TMP_Text enemiesText;
    private int enemies = 0;

    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas gameOverCanvas;
    public TMP_Text finalScoreText;
    public TMP_Text highScoreText;

    private float changeSceneTimer = 1.0f;

    // dodaæ animowanie siê wyniku po LeveLCompleted
    public int scoreAnimationSpeed = 2;
    private bool animatingScore = false;
    private int score = 0;
    private int maxScore = 0;
    private int maxSecsToHighscore = 180;

    public Canvas optionsCanvas;

    public Slider volumeSlider;

    public void SetVolume()
    {
        Debug.Log("Volume set to " + volumeSlider.value);
        AudioListener.volume = volumeSlider.value;
    }

    public void IncreaseQuality()
    {
        Debug.Log("Quality increased");
        QualitySettings.IncreaseLevel();
    }

    public void DecreaseQuality()
    {
        Debug.Log("Quality decreased");
        QualitySettings.DecreaseLevel();
    }

    public void OnOptionsButtonClicked()
    {
        SetGameState(GameState.GS_OPTIONS);
    }

    void AnimateScore()
    {
        score += scoreAnimationSpeed;
        if (score > maxScore)
        {
            score = maxScore;
            animatingScore = false;
        }
        finalScoreText.text = "Score: " + score.ToString();
    }

    public void OnNextLevelButtonClicked()
    {
        SceneManager.LoadScene("Level2");
    }

    public void FinishGame()
    {
        if (keys == maxKeysNumber)
        {
            Debug.Log("You won!");
        }
        else
        {
            Debug.Log("You have found " + keys + "/" + maxKeysNumber + " keys");
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
        if (lives == 0) GameOver();
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
        if (keys == maxKeysNumber) keysCompleted = true;
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
            Time.timeScale = 1.0f;
            inGameCanvas.enabled = true;
        }
        else
        {
            inGameCanvas.enabled = false;
        }

        if (newGameState == GameState.GS_PAUSEMENU)
        {
            //Time.timeScale = 0.0f;
            pauseMenuCanvas.enabled = true;
        }
        else
        {
            pauseMenuCanvas.enabled = false;
        }

        if (newGameState == GameState.GS_LEVELCOMPLETED)
        {
            levelCompletedCanvas.enabled = true;
            float secondsBelowMax = Mathf.Max(0, maxSecsToHighscore - timer);
            maxScore = Mathf.FloorToInt(secondsBelowMax) + 50 * enemies + 25 * lives + 10 * coins;
            animatingScore = true;

            string highScoreKey = "HighScore" + SceneManager.GetActiveScene().name;

            if (maxScore > PlayerPrefs.GetInt(highScoreKey))
            {
                PlayerPrefs.SetInt(highScoreKey, maxScore);
            }

            highScoreText.text = "Highscore: " + PlayerPrefs.GetInt(highScoreKey).ToString();
        }

        if (newGameState == GameState.GS_GAME_OVER)
        {
            gameOverCanvas.enabled = true;
        }
        else
        {
            gameOverCanvas.enabled = false;
        }

        if (newGameState == GameState.GS_OPTIONS)
        {
            //Time.timeScale = 0.0f;
            optionsCanvas.enabled = true;
        }
        else
        {
            optionsCanvas.enabled = false;
        }
    }

    void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void LevelCompleted()
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

        if (!PlayerPrefs.HasKey("HighScoreLevel1"))
        {
            PlayerPrefs.SetInt("HighScoreLevel1", 0);
        }
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

        if (animatingScore)
        {
            AnimateScore();
        }
    }
}
