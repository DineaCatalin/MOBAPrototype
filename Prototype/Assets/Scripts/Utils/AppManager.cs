using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    string GAME_SCENE_NAME = "GameLevel";
    string LOBBY_SCENE_NAME = "GameLobby";

    string sceneName;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        HandleExit();
    }

    void HandleExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();

            //sceneName = SceneManager.GetActiveScene().name;

            //if (sceneName == LOBBY_SCENE_NAME)
            //    Application.Quit();
            //else if (sceneName == GAME_SCENE_NAME)
            //    SceneManager.LoadScene(LOBBY_SCENE_NAME);
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Screenmanager Resolution Width", 800);
        PlayerPrefs.SetInt("Screenmanager Resolution Height", 600);
        PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
    }
}
