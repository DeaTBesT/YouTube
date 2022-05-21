using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Start game")]
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private float speedRotate;

    [Header("Players")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Map")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private TextMeshProUGUI textScoreboard;

    [Header("Canvases")]
    [SerializeField] private GameObject canvasLobby;
    [SerializeField] private GameObject canvasMenu;
    [SerializeField] private GameObject canvasGame;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textHealth;

    private bool isOpenMenu = false;
    private bool isSpawnedPlayer = false;

    public static GameManager Instance;

    public TextMeshProUGUI TextHealth { get { return textHealth; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdatePlayerList();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, 1 * speedRotate);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateMenu(!isOpenMenu);
        }
    }

    public void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        sceneCamera.gameObject.SetActive(false);

        canvasLobby.SetActive(false);
        canvasGame.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isSpawnedPlayer = true;
    }

    public void PlayerDied()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        sceneCamera.gameObject.SetActive(true);

        canvasLobby.SetActive(true);
        canvasGame.SetActive(false);

        isSpawnedPlayer = false;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        photonView.RPC(nameof(UpdatePlayerListRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void UpdatePlayerListRPC()
    {
        textScoreboard.text = string.Empty;

        List<Player> playersList = new List<Player>();

        foreach (Player m_player in PhotonNetwork.PlayerList)
        {
            playersList.Add(m_player);
        }

        playersList.Sort(delegate(Player p1, Player p2) { return p2.GetScore().CompareTo(p1.GetScore()); });

        foreach (Player m_player in playersList)
        {
            textScoreboard.text += $"{(m_player == PhotonNetwork.LocalPlayer ? "<color=red>You</color>" : m_player.NickName)} : {m_player.GetScore()}\n";
        }
    }

    private void ActivateMenu(bool isActivate)
    {
        isOpenMenu = isActivate;

        canvasMenu.SetActive(isOpenMenu);

        if (isSpawnedPlayer) 
        { 
            canvasGame.SetActive(!isOpenMenu);

            Cursor.lockState = isOpenMenu ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpenMenu ? true : false;
        }
        else 
        {
            canvasLobby.SetActive(!isOpenMenu); 
        }
    }

    public void ButtonContinue()
    {
        ActivateMenu(!isOpenMenu);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel(0);
    }
}
