using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class RestartGame : MonoBehaviourPunCallbacks
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
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LeaveRoom();
        Debug.Log("try left room");
        PhotonNetwork.Disconnect();
        Application.Quit();
        SceneManager.LoadScene(0);
        //PhotonNetwork.LoadLevel(0);
    }

    /*public override void OnLeftRoom()
    {

        StartCoroutine(WaitToLeave());
    }

    IEnumerator WaitToLeave()
    {
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(0);
    }

    public void OnDisconnectedFromServer()
    {
        Debug.Log("disconnected");
        SceneManager.LoadScene(0);
    }
    
    public override void OnLeftRoom()
    {
        Debug.Log("left room");
        //PhotonNetwork.LoadLevel(0);
        SceneManager.LoadScene(0);

        base.OnLeftRoom();
    }*/
}
