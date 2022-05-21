using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviourPun
{
    [SerializeField] private float health;

    private TextMeshProUGUI textHealth;

    private void Start()
    {
        textHealth = GameManager.Instance.TextHealth;

        textHealth.text = $"{health}%";
    }

    public void TakeDamage(float m_ammount, Player m_owner)
    {
        photonView.RPC(nameof(TakeDamageRPC), RpcTarget.All, m_ammount, m_owner);
    }

    [PunRPC]
    private void TakeDamageRPC(float m_ammount, Player m_owner)
    {
        if (!photonView.IsMine) { return; }

        if (health - m_ammount > 0)
        {
            health -= m_ammount;
            textHealth.text = $"{health}%";
        }
        else
        {         
            int currentScore = m_owner.GetScore();
            currentScore++;

            m_owner.SetScore(currentScore);

            GameManager.Instance.PlayerDied();

            PhotonNetwork.Destroy(gameObject);
        }
    }
}
