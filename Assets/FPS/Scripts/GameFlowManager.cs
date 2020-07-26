using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("Duration of the fade-to-black at the end of the game")]
    public float endSceneLoadDelay = 3f;
    [Tooltip("The canvas group of the fade-to-black screen")]
    public CanvasGroup endGameFadeCanvasGroup;

    [Header("Win")]
    [Tooltip("This string has to be the name of the scene you want to load when winning")]
    public string winSceneName = "NetWinScene";
    [Tooltip("Duration of delay before the fade-to-black, if winning")]
    public float delayBeforeFadeToBlack = 4f;
    [Tooltip("Duration of delay before the win message")]
    public float delayBeforeWinMessage = 2f;
    [Tooltip("Sound played on win")]
    public AudioClip victorySound;
    [Tooltip("Prefab for the win game message")]
    public GameObject WinGameMessagePrefab;

    [Header("Lose")]
    [Tooltip("This string has to be the name of the scene you want to load when losing")]
    public string loseSceneName = "LoseScene";

    private bool GameProgressMonitor;

    public bool gameIsEnding
    {
        get { return GameProgressMonitor; }
        set
        {
            if (value == GameProgressMonitor)
                return;

            GameProgressMonitor = value;
            if (GameProgressMonitor)
            {
                StartCoroutine(fading());
            }
        }
    }

    public Health m_Player;
    public ObjectiveManager m_ObjectiveManager;
    public float m_TimeLoadEndGameScene;
    public string m_SceneToLoad = "WinScene";
    
    public virtual void Start()
    {
        m_Player = FindObjectOfType<PlayerCharacterController>().GetComponent<Health>();
        DebugUtility.HandleErrorIfNullFindObject<Health, GameFlowManager>(m_Player, this);
        m_Player.onDie += OnAllDiedHandler;

        m_ObjectiveManager = FindObjectOfType<ObjectiveManager>();
		DebugUtility.HandleErrorIfNullFindObject<ObjectiveManager, GameFlowManager>(m_ObjectiveManager, this);
        m_ObjectiveManager.OnAllCompleted += OnAllcompletedHandler;

        AudioUtility.SetMasterVolume(1);
    }

    public virtual IEnumerator fading()
    {
        while(Time.time <= m_TimeLoadEndGameScene)
        {
            float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / endSceneLoadDelay;
            endGameFadeCanvasGroup.alpha = timeRatio;

            AudioUtility.SetMasterVolume(1 - timeRatio);
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("About to load " + m_SceneToLoad);
        SceneManager.LoadScene(m_SceneToLoad);
        gameIsEnding = false;
    }

    public void OnAllcompletedHandler()
    {
        EndGame(true);
    }

    public void OnAllDiedHandler(GameObject source)
    {
        EndGame(false);
    }

    public virtual void EndGame(bool win)
    {
        // unlocks the cursor before leaving the scene, to be able to click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Endgame is run");
        scoreUpdate();
        // Remember that we need to load the appropriate end scene after a delay
        endGameFadeCanvasGroup.gameObject.SetActive(true);
        if (win)
        {
            Debug.Log("The win part of EndGame is run");
            m_SceneToLoad = winSceneName;
            m_TimeLoadEndGameScene = Time.time + endSceneLoadDelay + delayBeforeFadeToBlack;

            // play a sound on win
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = victorySound;
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.HUDVictory);
            audioSource.PlayScheduled(AudioSettings.dspTime + delayBeforeWinMessage);

            // create a game message
            var message = Instantiate(WinGameMessagePrefab).GetComponent<DisplayMessage>();
            if (message)
            {
                message.delayBeforeShowing = delayBeforeWinMessage;
                message.GetComponent<Transform>().SetAsLastSibling();
            }
        }
        else
        {
            Debug.Log("The lose part of EndGame is run");
            m_SceneToLoad = loseSceneName;
            m_TimeLoadEndGameScene = Time.time + endSceneLoadDelay;
        }

        gameIsEnding = true;
    }

    public void scoreUpdate()
    {
        int currentScore = GetComponentInChildren<scorekeeper>().score;
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        int highestScore = PlayerPrefs.GetInt("HighestScore", 0);
        if(currentScore > highestScore)
        {
            PlayerPrefs.SetInt("HighestScore", currentScore);
        }
    }
}
