using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreLevel1Text;
    public TMP_Text highScoreLevel2Text;

    IEnumerator StartGame(string levelName)
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadScene(levelName);
    }

    public void onLevel1ButtonPressed()
    {
        StartCoroutine(StartGame("Level1"));
    }

    public void onLevel2ButtonPressed()
    {
        StartCoroutine(StartGame("Level2"));
    }

    public void OnExitButtonPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
    }

    void Awake()
    {
        if (PlayerPrefs.HasKey("HighScoreLevel1"))
        {
            highScoreLevel1Text.text = PlayerPrefs.GetInt("HighScoreLevel1").ToString();
        }
        else
        {
            PlayerPrefs.SetInt("HighScoreLevel1", 0);
        }

        if (PlayerPrefs.HasKey("HighScoreLevel2"))
        {
            highScoreLevel2Text.text = PlayerPrefs.GetInt("HighScoreLevel2").ToString();
        }
        else
        {
            PlayerPrefs.SetInt("HighScoreLevel2", 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
