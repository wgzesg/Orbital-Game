using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Tooltip("Image component dispplaying current health")]
    public Image healthFillImage;
    public CanvasGroup healthBar;
    private PlayerAvatar PA;

    Health m_PlayerHealth;

    private void Start()
    {
        PA = PlayerManager.PMinstance.findLocalPlayerAvatar();

        PA.PlayerSpawned += OnPlayerSpawnHandler;
        PA.PlayerDied += OnPlayerDiedHandler;
        m_PlayerHealth = PA.playerAvatar.GetComponent<Health>();
    }

    public void OnPlayerSpawnHandler()
    {
        Debug.Log("health event is carried out");
        m_PlayerHealth = PA.playerAvatar.GetComponent<Health>();
        healthBar.alpha = 1;
        Debug.Log("set to 1");
    }

    public void OnPlayerDiedHandler()
    {
        Debug.Log("health event is carried out");
        m_PlayerHealth = null;
        healthBar.alpha = 0;
        Debug.Log("set to 0");
    }

    void Update()
    {
        if(m_PlayerHealth != null)
        {
            healthFillImage.fillAmount = m_PlayerHealth.currentHealth / m_PlayerHealth.maxHealth;
        }
    }
}
