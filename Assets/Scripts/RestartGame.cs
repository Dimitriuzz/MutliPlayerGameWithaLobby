using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private GameObject gameOver;
    [SerializeField] private TMP_Text endGameTextP;
    public void ResultsDispaly(string name, int coins)
    {
        foreach (Transform child in transform) child.gameObject.SetActive(true);
        endGameTextP.text = "Player " + name + " wined with " + coins.ToString() + " gold gathered";
    }
    public void Restart()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
    }
}
