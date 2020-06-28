using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorytellingManager : MonoBehaviour
{
    public GameObject HUDroot;
    public Dialogue[] dialogSet;
    public TMPro.TextMeshProUGUI message;
    public Button yesKey;
    public Button noKey;

    public UnityAction onStartGame;
    string currentDialogueName;
    Dialogue currentActiveDialog;
    int currentDialogIndex = 0;
    void Start()
    {
        HUDroot.SetActive(false);
        StartCoroutine(enterDelay(3f));
        
    }

    IEnumerator enterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (!feedinDialogue("introPage"))
        {
            Debug.Log("The conversation does not exist.");
        }
    }

    bool feedinDialogue(string dialogName)
    {
        foreach(Dialogue d in dialogSet)
        {
            if(d.dialogName == dialogName)
            {
                HUDroot.SetActive(true);
                currentActiveDialog = d;
                currentDialogueName = dialogName;
                displayNext();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                EventSystem.current.SetSelectedGameObject(null);
                return true;
            }
        }
        return false;
    }

    public void onYesClickHandler()
    {
        displayNext();
        feedinDialogue("tutorial");
    }

    public void onNoClickHandler()
    {
        displayNext();
    }

    void displayNext()
    {
        if(currentDialogIndex < currentActiveDialog.conversation.Length)
        {
            message.text = currentActiveDialog.conversation[currentDialogIndex];
            currentDialogIndex++;
        }
        else
        {
            HUDroot.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("Close off the dialog window");
            currentDialogIndex = 0;
            if(currentDialogueName == "tutorial")
            {
                yesKey.gameObject.SetActive(false);
                noKey.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Continue";
                feedinDialogue("background");
            }
            else if(currentDialogueName == "introPage")
            {
                yesKey.gameObject.SetActive(false);
                noKey.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Continue";
                feedinDialogue("background");
            }
            else if(currentDialogueName == "background")
            {
                if(onStartGame != null)
                {
                    onStartGame.Invoke();
                }
            }
                    
        }
    }

}
