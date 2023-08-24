using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace SpaceShooter
{
    public class Server : MonoBehaviourPunCallbacks
    {
        public static Server Instance;

        private Player _server;

        public Character characterPrefab;

        public CharacterSpawn spawns;

        public Transform spawn1;
        public Transform spawn2;


        public ChatManager chatManager;
        private Dictionary<Player, Character> _dicModels = new Dictionary<Player, Character>();
        private bool _enoughPlayers;
        public int PackagesPerSecond { get; private set; }

        private void Start()
        {
            chatManager = FindObjectOfType<ChatManager>();

            if (!chatManager)
            {
                StartCoroutine(SearchChat());
            }



            DontDestroyOnLoad(gameObject);

            if (Instance == null)
            {
                if (photonView.IsMine)
                {
                    PackagesPerSecond = 60;
                    //PhotonNetwork.LoadLevel(1);

                    /*spawns = FindObjectOfType<CharacterSpawn>();
                    if (!spawns)
                    {
                        StartCoroutine(SearchSpawns());
                    }

                    Debug.Log("spawns count" + spawns.spawns.Count);

                    for (int i =0; i<spawns.spawns.Count;i++)
                    { Debug.Log("spawn " + spawns.spawns[i].position); }*/


                    //photonView.RPC("SetServer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, 1);


                    //int pos = 0;
                    StartCoroutine(CheckPlayers());

                    //Transform spawnPosition;



                    //if (!PhotonNetwork.IsMasterClient) spawnPosition = spawn1;
                    // else spawnPosition = spawn2;

                    /* PhotonNetwork.Instantiate(characterPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
                     Character newCharacter = GetComponent<Character>();



                     Debug.Log("added player " + PhotonNetwork.LocalPlayer.NickName);
                     newCharacter.transform.Rotate(newCharacter.transform.up, 90.0f);
                     newCharacter.SetInitialParameters(PhotonNetwork.LocalPlayer);
                     _dicModels.Add(PhotonNetwork.LocalPlayer, newCharacter);
                     RequestUpdatePlayerList();
                     photonView.RPC("SetWaitingScreen", PhotonNetwork.LocalPlayer, true);
                    */


                }
            }
        }

        public void StartGame()
        {

            //SetWaitingScreen(false);
            PhotonNetwork.LoadLevel(2);
        }

        IEnumerator SearchSpawns()
        {
            while (!spawns)
            {
                spawns = FindObjectOfType<CharacterSpawn>();
                Debug.Log("spawn ");
                yield return new WaitForEndOfFrame();
            }

            Debug.Log("consegui spawn");
        }


        IEnumerator SearchChat()
        {
            while (!chatManager)
            {
                chatManager = FindObjectOfType<ChatManager>();
                yield return new WaitForEndOfFrame();
            }

            Debug.Log("consegui chat");
        }

        [PunRPC]
        /*void SetServer(Player serverPlayer, int sceneIndex = 1)
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            _server = serverPlayer;

            PackagesPerSecond = 60;
            PhotonNetwork.LoadLevel(sceneIndex);

            var playerLocal = PhotonNetwork.LocalPlayer;

            if (playerLocal != _server)
            {
                photonView.RPC("AddPlayer", _server, playerLocal);
            }
            else photonView.RPC("AddPlayer", _server, PhotonNetwork.Server);

        }*/

        void SetServer(Player serverPlayer, int sceneIndex = 1)
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            //_server = serverPlayer;

            PackagesPerSecond = 60;
            PhotonNetwork.LoadLevel(sceneIndex);

            //var playerLocal = PhotonNetwork.LocalPlayer;


            photonView.RPC("AddPlayer", serverPlayer);



        }

        /*
        [PunRPC]
        void AddPlayer(Player player)
        {
            StartCoroutine(WaitForLevel(player));
        }

        IEnumerator WaitForLevel(Player player)
        {
            while (PhotonNetwork.LevelLoadingProgress > 0.9f)
            {
                yield return new WaitForEndOfFrame();
            }
            int pos = 0;
            if(!PhotonNetwork.IsMasterClient) pos = 1;

            Character newCharacter = PhotonNetwork
                .Instantiate(characterPrefab.name, spawns.spawns[pos].position, spawns.spawns[pos].rotation)
                .GetComponent<Character>();
            newCharacter.transform.Rotate(newCharacter.transform.up, 90.0f);
            newCharacter.SetInitialParameters(player);
            _dicModels.Add(player, newCharacter);
            RequestUpdatePlayerList();
            photonView.RPC("SetWaitingScreen", player, true);
            Debug.Log("added player " + player.NickName);
        }
        */

        /* void AddPlayer(Player player)
         {
             StartCoroutine(WaitForLevel(player));
         }

         IEnumerator WaitForLevel(Player player)
         {
             while (PhotonNetwork.LevelLoadingProgress > 0.9f)
             {
                 yield return new WaitForEndOfFrame();
             }

             var pos = PhotonNetwork.PlayerList.Length - 2;

             Character newCharacter = PhotonNetwork
                 .Instantiate(characterPrefab.name, spawns.spawns[pos].position, spawns.spawns[pos].rotation)
                 .GetComponent<Character>();
             newCharacter.transform.Rotate(newCharacter.transform.up, 90.0f);
             newCharacter.SetInitialParameters(player);
             _dicModels.Add(player, newCharacter);
             RequestUpdatePlayerList();
             photonView.RPC("SetWaitingScreen", player, true);
             Debug.Log("added player " + player.NickName);
         }
        */

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


        [PunRPC]
        void SetWaitingScreen(bool state)
        {
            if (!state)
                _enoughPlayers = true;
            var screens = FindObjectOfType<ScreenManager>();

            screens.WaitingScreenState(state);
            screens.SetRoomName(PhotonNetwork.CurrentRoom.Name);
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

        public void RequestMove(Player player, Vector3 dir)
        {
            if (!_enoughPlayers)
            {
                return;
            }

            photonView.RPC("Move", _server, player, dir);
        }

        [PunRPC]
        private void Move(Player player, Vector3 dir)
        {
            if (player == null) return;

            if (_dicModels.ContainsKey(player))
            {
                _dicModels[player].Move(dir);
            }

        }

        public void RequestJump(Player player)
        {
            if (!_enoughPlayers) return;
            photonView.RPC("Jump", _server, player);
        }

        [PunRPC]
        private void Jump(Player player)
        {
            if (player == null) return;

            if (_dicModels.ContainsKey(player))
            {
                _dicModels[player].Jump();
            }
        }

        public void RequestShoot(Player player, Vector3 dir)
        {
            if (!_enoughPlayers) return;
            photonView.RPC("Shoot", _server, player, dir);
        }

        [PunRPC]
        private void Shoot(Player player, Vector3 dir)
        {
            if (player == null) return;

            if (_dicModels.ContainsKey(player))
            {
                _dicModels[player].Shoot(dir);
            }
        }

        public void RequestDamage(Player player, int damage)
        {
            if (!_enoughPlayers) return;
            photonView.RPC("Damage", _server, player, damage);
        }

        [PunRPC]
        private void Damage(Player player, int damage)
        {
            if (player == null) return;
            if (_dicModels.ContainsKey(player))
            {
                var myPlayer = _dicModels[player];
                myPlayer.Damage(damage);
                var playerHP = myPlayer.GetHP();
                foreach (var p in _dicModels)
                {
                    photonView.RPC("UpdateLifeBar", p.Key, myPlayer.photonView.ViewID, playerHP);
                }
            }
        }

        [PunRPC]

        void UpdateLifeBar(int photonViewID, float playerHP)
        {
            var characters = FindObjectsOfType<Character>();
            foreach (var c in characters)
            {
                if (c.photonView.ViewID == photonViewID)
                    c.UpdateLifeBar(playerHP);
            }
        }

        public void SetPlayerName(int photonViewID, string name)
        {
            photonView.RPC("SetPlayerNameBuffered", RpcTarget.AllBuffered, photonViewID, name);
        }

        [PunRPC]

        void SetPlayerNameBuffered(int photonViewID, string name)
        {
            var characters = FindObjectsOfType<Character>();

            foreach (var c in characters)
            {
                if (c.photonView.ViewID == photonViewID)
                    c.UpdateName(name);
            }
        }



        public void PlayerLose(Player player)
        {
            PhotonNetwork.Destroy(_dicModels[player].gameObject);
            photonView.RPC("SetDisconnectScreen", player);
            _dicModels.Remove(player);


            if (_dicModels.Count > 1) return;

            foreach (var p in _dicModels)
            {
                if (p.Key != _server)
                {
                    photonView.RPC("SetWinScreen", p.Key);
                }
            }

            StartCoroutine(CloseServer());
        }

        IEnumerator CloseServer()
        {
            while (true)
            {
                if (PhotonNetwork.PlayerList.Length <= 1)
                {

                    photonView.RPC("Disconnect", _server);

                    break;
                }

                yield return new WaitForSeconds(1);
            }
        }


        [PunRPC]
        private void Disconnect()
        {
            //PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel("Menu");
        }


        public void RequestSendText(string text)
        {
            photonView.RPC("SendText", _server, PhotonNetwork.LocalPlayer, text);
        }


        [PunRPC]
        private void SendText(Player player, string text)
        {

            var pos = GetPositionInPlayersList(player);
            foreach (var p in PhotonNetwork.PlayerList)
            {
                photonView.RPC("UpdateChatBox", p, pos, player.NickName, text);
            }
        }


        [PunRPC]
        private void UpdateChatBox(int posInPlayerList, string nickname, string text)
        {
            chatManager.UpdateChatBox(posInPlayerList, nickname, text);
        }


        private void RequestUpdatePlayerList()
        {
            photonView.RPC("CheckPlayersList", _server);
        }


        [PunRPC]
        private void CheckPlayersList()
        {
            Debug.Log("check player server orig");
            Player[] players = PhotonNetwork.PlayerList;

            foreach (var p in PhotonNetwork.PlayerList)
            {
                photonView.RPC("UpdatePlayersList", p, players);
            }
        }


        [PunRPC]
        private void UpdatePlayersList(Player[] players)
        {
            chatManager.UpdatePlayersList(players);
        }


        private int GetPositionInPlayersList(Player player)
        {
            for (int i = 1; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i] == player)
                {
                    return i;
                }
            }

            return 0;
        }


        public void PlayerLeavesRoom(Player player)
        {
            photonView.RPC("RemoveAndDisconnectPlayer", _server, player);
        }


        [PunRPC]
        public void RemoveAndDisconnectPlayer(Player player)
        {
            if (player == null) return;

            if (!_dicModels.ContainsKey(player)) return;


            PhotonNetwork.Destroy(_dicModels[player].gameObject);

            _dicModels.Remove(player);


            photonView.RPC("Disconnect", player);


            StartCoroutine(UpdatePlayerListWithTimer());
        }

        IEnumerator UpdatePlayerListWithTimer()
        {
            yield return new WaitForSeconds(2);
            if (PhotonNetwork.PlayerList.Length > 1)
                RequestUpdatePlayerList();
        }
    }
}
