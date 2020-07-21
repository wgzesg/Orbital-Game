using UnityEngine;
using UnityEngine.UI;

public class NetFeedbackFlashHUD :FeedbackFlashHUD
{
    public PlayerAvatar PA;

    public override void Start()
    {
        PA = PlayerManager.PMinstance.findLocalPlayerAvatar();

        PA.PlayerSpawned += OnPlayerSpawnHandler;
        PA.PlayerDied += OnPlayerDiedHandler;
        m_PlayerHealth = PA.playerAvatar.GetComponent<Health>();
        m_PlayerHealth.onDamaged += OnTakeDamage;
        m_PlayerHealth.onHealed += OnHealed;

        m_GameFlowManager = FindObjectOfType<GameFlowManager>();
        DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, FeedbackFlashHUD>(m_GameFlowManager, this);
    }

    public void OnPlayerSpawnHandler()
    {
        m_PlayerHealth = PA.playerAvatar.GetComponent<Health>();
        m_PlayerHealth.onDamaged += OnTakeDamage;
        m_PlayerHealth.onHealed += OnHealed;
    }

    public void OnPlayerDiedHandler()
    {
        m_PlayerHealth = null;
    }
}
