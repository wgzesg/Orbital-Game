using UnityEngine;

public class quit : MonoBehaviour
{
    public void onQuitHandler()
    {
        Debug.Log("it has quitted");
        Application.Quit();
        Debug.Log("it really has quitted");
    }
}
