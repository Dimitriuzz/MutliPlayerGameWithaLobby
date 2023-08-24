using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomsListButton : MonoBehaviour
{

    [SerializeField] private TMP_Text roomName;
    public RoomInfo info;
    public void SetUp(RoomInfo roomInfo)
    {
        info = roomInfo;
        roomName.text = info.Name;
    }

    public void OnClick()
    {
        Launcher.instance.JoinNewRoom(info);
    }
    
}
