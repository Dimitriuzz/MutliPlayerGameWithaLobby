using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private Transform _roomsList;
    [SerializeField] private GameObject roomButtonPrefab;
    [SerializeField] private Transform _playersList;
    [SerializeField] private GameObject playerListPrefab;
    [SerializeField] private GameObject startGameButton;

    void Start()
    {
        instance = this;
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            StartCoroutine(WaitToLeave());
        }
            PhotonNetwork.ConnectUsingSettings();
        Debug.Log("connected");
        //MenuManager.instance.OpenMenu("nameenter");
    }

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("connected to master");
    }

    public override void OnJoinedLobby()
    {
        //base.OnJoinedLobby();
        if(string.IsNullOrEmpty(PhotonNetwork.NickName))
            MenuManager.instance.OpenMenu("nameenter");
        else
        MenuManager.instance.OpenMenu("title");
        Debug.Log("connected to lobby");

    }
    
    public void NameEnter()
    {
        if (!string.IsNullOrEmpty(PhotonNetwork.NickName)) MenuManager.instance.OpenMenu("title");
        //if (string.IsNullOrEmpty(playerName.text)) return;
        PhotonNetwork.NickName = playerName.text;
        MenuManager.instance.OpenMenu("title");
    }
    public void CreateNewRoom()
    {
        if (string.IsNullOrEmpty(inputField.text)) return;

        PhotonNetwork.CreateRoom(inputField.text);
        MenuManager.instance.OpenMenu("loading");
    }

    public void LeaveCurrentRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("title");
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.instance.OpenMenu("roommenu");

        Player[] players = PhotonNetwork.PlayerList;

        for(int i=0;i<_playersList.childCount;i++)
        {
            Destroy(_playersList.GetChild(i).gameObject);
        }

        for(int i=0;i<players.Length;i++)
        {
            Instantiate(playerListPrefab, _playersList).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //base.OnMasterClientSwitched(newMasterClient);
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        MenuManager.instance.OpenMenu("error");
        errorText.text = message;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomsList)
    {
        //base.OnRoomListUpdate(roomList);
        for(int i=0; i< _roomsList.childCount; i++)
        {
            Destroy(_roomsList.GetChild(i).gameObject);
        }

        for(int i=0; i<roomsList.Count;i++)
        {
            if (roomsList[i].RemovedFromList) continue;
            Instantiate(roomButtonPrefab, _roomsList).GetComponent<RoomsListButton>().SetUp(roomsList[i]);
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // base.OnPlayerEnteredRoom(newPlayer);
        Instantiate(playerListPrefab, _playersList).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void JoinNewRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
    }

    IEnumerator WaitToLeave()
    {
        while (PhotonNetwork.IsConnected)
            yield return null;
        
    }
}
