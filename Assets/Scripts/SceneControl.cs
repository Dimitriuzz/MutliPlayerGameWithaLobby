using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class SceneControl : MonoBehaviourPunCallbacks
{
    public static SceneControl instance;
    // Start is called before the first frame update
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
        if (scene.buildIndex==1)
        {
            PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
        }
    }
}
