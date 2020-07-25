using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public void Load_SinglePlayerScene()
    {
        Debug.Log("log to Single Player Scene");
        SceneManager.LoadScene(MultiplayerSettingV2.multiplayerSettingV2.singlePlayerScene);
    }
    public void Load_MultiPlayerScene()
    {
        Debug.Log("log to Multi-Player Scene");
        SceneManager.LoadScene(MultiplayerSettingV2.multiplayerSettingV2.menuScene);
    }

    public void QuitGame()
    {
        Debug.Log("Start to quit");
        Application.Quit();
        Debug.Log("you have really quited");
    }
}
