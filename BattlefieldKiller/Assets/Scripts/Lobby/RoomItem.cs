using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textRoomName;

    private RoomInfo info;

    public void SetUp(RoomInfo m_info)
    {
        info = m_info;
        textRoomName.text = m_info.Name;
    }

    public void JoinRoom()
    {
        LobbyManager.instance.JoinRoom(info);
    }
}