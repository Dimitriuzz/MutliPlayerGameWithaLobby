using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListItem : MonoBehaviourPunCallbacks
{

    public TMP_Text _playerName;
    private Player _player;

    

    public void SetUp(Player player)
    {
        _player = player;
        _playerName.text = player.NickName;
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //base.OnPlayerLeftRoom(otherPlayer);
        if (_player == otherPlayer) Destroy(gameObject);
    }

    public override void OnLeftRoom()
    {
        //base.OnLeftRoom();
        Destroy(gameObject);
    }
}
