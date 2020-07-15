using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Tooltip("Image component dispplaying current health")]
    public Image healthFillImage;
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
        m_PlayerHealth = PA.playerAvatar.GetComponent<Health>();
    }

    public void OnPlayerDiedHandler()
    {
        m_PlayerHealth = null;
    }

    void Update()
    {
        if(m_PlayerHealth != null)
        {
            healthFillImage.fillAmount = m_PlayerHealth.currentHealth / m_PlayerHealth.maxHealth;
        }
    }
}
