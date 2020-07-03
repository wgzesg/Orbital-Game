using System.Collections;
using UnityEngine;

public class DisplayMessage : MonoBehaviour
{
    [Tooltip("The text that will be displayed")]
    [TextArea]
    public string message;
    [Tooltip("Prefab for the message")]
    public GameObject messagePrefab;
    [Tooltip("Delay before displaying the message")]
    public float delayBeforeShowing;

    DisplayMessageManager m_DisplayMessageManager;

    void Start()
    {
        m_DisplayMessageManager = FindObjectOfType<DisplayMessageManager>();
        DebugUtility.HandleErrorIfNullFindObject<DisplayMessageManager, DisplayMessage>(m_DisplayMessageManager, this);

        StartCoroutine(DisplayFunction());
    }

    IEnumerator DisplayFunction()
    {
        yield return new WaitForSeconds(delayBeforeShowing);

        var messageInstance = Instantiate(messagePrefab, m_DisplayMessageManager.DisplayMessageRect);
        var notification = messageInstance.GetComponent<NotificationToast>();
        if (notification)
        {
            notification.Initialize(message);
        }
    }
}
