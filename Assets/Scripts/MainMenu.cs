using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    IEnumerator StartGame(string levelName)
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadScene(levelName);
    }

    public void onLevel1ButtonPressed()
    {
        StartCoroutine(StartGame("Level1"));
    }

    public void OnExitButtonPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
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
