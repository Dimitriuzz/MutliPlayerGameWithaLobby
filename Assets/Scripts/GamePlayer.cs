using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace RocketPiglet
{
    public class GamePlayer : MonoBehaviourPunCallbacks
    {
        [SerializeField] private int m_NumLives;
        [SerializeField] private Piglet m_Ship;
        [SerializeField] private GameObject m_PlayerShipPrefab;
        public Piglet ActiveShip => m_Ship;
        private PhotonView photonView;

        public int NumLives => m_NumLives;

        [SerializeField] private CameraControl m_CameraController;
        [SerializeField] private MovementController m_MovementController;

       /* protected override void Awake()
        {
            base.Awake();

            if (m_Ship != null)
                Destroy(m_Ship.gameObject);
            
        }*/

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            if(photonView.IsMine) Respawn();
            
         }

        private void OnShipDeath()
        {
            m_NumLives--;
            //StartCoroutine(MakeDelay());
            if (m_NumLives > 0) Respawn(); //Invoke("Respawn", 2);
            //else
                //LevelSequenceController.Instance.FinishCurrentLevel(false);
        }

       
        private void Respawn()
        {
           Debug.Log("respawn");

            //if (LevelSequenceController.PlayerShip != null)
            {
                Debug.Log(PhotonNetwork.IsMasterClient);
                //var newPlayerShip = Instantiate(m_PlayerShipPrefab);
                var spawns = GameObject.FindGameObjectsWithTag("Spawn");
                Vector3 spawnpos = Vector3.zero;
                for (int i = 0; i < spawns.Length; i++)
                {
                    if (PhotonNetwork.IsMasterClient&&spawns[i].name=="MasterPlayer")
                    {
                        spawnpos= spawns[i].transform.position;
                       
                    }
                    if (PhotonNetwork.IsMasterClient! && spawns[i].name == "JoinedPlayer")
                    {
                        spawnpos = spawns[i].transform.position;
                    }
                }

                var newPlayerShip = PhotonNetwork.Instantiate("Piglet", spawnpos, Quaternion.identity);
                Debug.Log("pig spawned");
                m_Ship = newPlayerShip.GetComponent<Piglet>();


                /*m_CameraController.SetTarget(m_Ship.transform);
                m_MovementController.SetTargetShip(m_Ship);
                m_Ship.EventOnDeath.AddListener(OnShipDeath);*/
            }
        }

        public int Score { get; private set; }

        public int NumKills { get; private set; }

        

        private int m_PlayerKills;
        private int m_PlayerScore;

        

        public void AddKill()
        {
            NumKills++;
            

        }

        public void AddScore(int num)
        {
            Score += num;
           
           // Debug.Log("pref score" + m_PlayerScore);
        }
    }
}
