using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class SceneControl : MonoBehaviourPunCallbacks
{
    public static SceneControl instance;
    
    void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

            DontDestroyOnLoad(gameObject);

            instance = this;

        
        
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex==3)
        {
            PhotonNetwork.Instantiate("GamePlayer", Vector3.zero, Quaternion.identity);
        }
    }

    public void GameOver()
    {

    }
}
