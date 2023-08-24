﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace SpaceShooter
{
    public class Connection : MonoBehaviourPunCallbacks
    {
        public GameObject mainScreen;
        public GameObject connectedScreen;
        public GameObject waitingScreen;
        public TMP_InputField nicknameField;
        public TextMeshProUGUI errorText;

        public Server serverPrefab;
        public CharacterControl controllerPrefab;

        public TMP_InputField createField;
        public TMP_InputField joinField;
        public TextMeshProUGUI welcomeText;
        public TextMeshProUGUI lobbyErrorText;
        public string playerName;

        private bool _enoughPlayers;
        public void Connect()
        {
            if (string.IsNullOrWhiteSpace(nicknameField.text))
            {
                SetMainScreenError("Please set a nickname.");
                return;
            }

            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public void CreateRoom()
        {
            if (string.IsNullOrEmpty(createField.text))
            {
                SetLobbyScreenError("Failed to create room. \nPlease introduce a room name.");
            }
            else
            {
                RoomOptions options = new RoomOptions();

                options.MaxPlayers = 5;

                PhotonNetwork.JoinOrCreateRoom(createField.text, options, TypedLobby.Default);
                SetWaitingScreen(true);

                StartCoroutine(CheckPlayers());
            }
        }

        public void JoinRoom()
        {
            if (string.IsNullOrEmpty(joinField.text))
            {
                SetLobbyScreenError("Failed to join room. \nPlease introduce a room name.");
            }
            else
            {
                PhotonNetwork.JoinRoom(joinField.text);
                
            }
        }

        public override void OnCreatedRoom()
        {
            PhotonNetwork.NickName = nicknameField.text;
            Debug.Log("room created");
            //PhotonNetwork.Instantiate(controllerPrefab.name, Vector3.zero, Quaternion.identity);
            //PhotonNetwork.Instantiate(serverPrefab.name, Vector3.zero, Quaternion.identity);
            SetWaitingScreen(true);
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnJoinedRoom()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("room joined");
                SetWaitingScreen(true);
                PhotonNetwork.NickName = playerName;
                //PhotonNetwork.Instantiate(serverPrefab.name, Vector3.zero, Quaternion.identity);
                PhotonNetwork.AutomaticallySyncScene = true;
                //PhotonNetwork.Instantiate(controllerPrefab.name, Vector3.zero, Quaternion.identity);
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            SetLobbyScreenError("Failed to create room '" + createField.text + "'. \nError: " + returnCode + " \n" + message);
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            SetLobbyScreenError("Failed to join room '" + joinField.text + "'. \nError: " + returnCode + " \n" + message);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            connectedScreen.SetActive(false);
            mainScreen.SetActive(true);
        }

        void SetMainScreenError(string text)
        {
            errorText.text = text;
        }

        void SetLobbyScreenError(string text)
        {
            lobbyErrorText.text = text;
        }

        public override void OnJoinedLobby()
        {
            mainScreen.SetActive(false);
            connectedScreen.SetActive(true);
            SetWelcomeText(nicknameField.text);
        }

        public void SetWelcomeText(string nickname)
        {
            playerName = nickname;
            welcomeText.text = "Welcome " + nickname;
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void StartGame()
        {

            //SetWaitingScreen(false);
            PhotonNetwork.LoadLevel(2);
        }

        IEnumerator CheckPlayers()
        {
            while (!_enoughPlayers)
            {
                _enoughPlayers = PhotonNetwork.PlayerList.Length == 5;
                yield return new WaitForSecondsRealtime(1);
            }

            _enoughPlayers = true;
            foreach (var p in PhotonNetwork.PlayerList)
            {
                photonView.RPC("SetWaitingScreen", p, false);
            }

            Debug.Log("enough players: " + _enoughPlayers);
        }


      
        void SetWaitingScreen(bool state)
        {
           
           

            waitingScreen.gameObject.SetActive(state);
            //screens.SetRoomName(PhotonNetwork.CurrentRoom.Name);
        }


        [PunRPC]
        void SetDisconnectScreen()
        {
            var screens = FindObjectOfType<ScreenManager>();

            screens.DisconnectScreen();
        }


        [PunRPC]
        void SetWinScreen()
        {
            var screens = FindObjectOfType<ScreenManager>();

            screens.WinScreen();

            var cc = FindObjectsOfType<CharacterControl>();

            foreach (var c in cc)
            {
                Destroy(c.gameObject);
            }
        }





    }
}
