using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Lobby")]
    [SerializeField] private Button buttonHost;

    [SerializeField] private Button buttonConnect;

    [SerializeField] private GameObject canvasLogin;
    [SerializeField] private GameObject canvasLobby;

    [Header("Multiplayer")]
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Transform roomListContainer;
    private List<RoomItem> roomsList;

    private byte m_players = 4;

    public string MaxPlayers
    {
        set
        {
            m_players = byte.Parse(value);
        }
    }

    public string NickName
    {
        set
        {
            PhotonNetwork.NickName = value;
        }
    }

    public static LobbyManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buttonHost.interactable = false;

        roomsList = new List<RoomItem>();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = Application.version;       
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Connect2Lobby()
    {
        buttonConnect.interactable = false;

        PhotonNetwork.ConnectUsingSettings();
    }

    #region Multiplayer

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, new Photon.Realtime.RoomOptions { MaxPlayers = m_players, IsVisible = true });
    }

    public void JoinRoom(RoomInfo m_info)
    {
        PhotonNetwork.JoinRoom(m_info.Name);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
    }

    public override void OnConnectedToMaster()
    {
        canvasLogin.SetActive(false);
        canvasLobby.SetActive(true);

        PhotonNetwork.JoinLobby();  
    }

    public override void OnJoinedLobby()
    {
        buttonHost.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        buttonConnect.interactable = true;
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                Destroy(roomsList[i].gameObject);
                roomsList.Remove(roomsList[i]);
            }
            else
            {
                if (roomList[i].RemovedFromList)
                    continue;
          
                RoomItem room = Instantiate(roomPrefab, roomListContainer).GetComponent<RoomItem>();
                roomsList.Add(room);
                room.SetUp(info);
            }
        }
    }

    #endregion Multiplayer
}
