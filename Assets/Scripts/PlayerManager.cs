using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
    
{
    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if(photonView.IsMine)
        {
            CreatePlayer();
        }
        
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("Character"), Vector3.zero, Quaternion.identity);
    }

    
}
