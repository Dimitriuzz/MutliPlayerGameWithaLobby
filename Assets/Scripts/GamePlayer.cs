using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SpaceShooter
{
    public class GamePlayer : SingletonBase<GamePlayer>
    {
        [SerializeField] private int m_NumLives;
        [SerializeField] private SpaceShip m_Ship;
        [SerializeField] private GameObject m_PlayerShipPrefab;
        public SpaceShip ActiveShip => m_Ship;

        public int NumLives => m_NumLives;

        [SerializeField] private CameraControl m_CameraController;
        [SerializeField] private MovementController m_MovementController;

        protected override void Awake()
        {
            base.Awake();

            if (m_Ship != null)
                Destroy(m_Ship.gameObject);
            
        }

        private void Start()
        {
            Respawn();
            
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
                //Debug.Log("trying to instantiate");
                //var newPlayerShip = Instantiate(m_PlayerShipPrefab);

                var newPlayerShip=PhotonNetwork.Instantiate("Ship", Vector3.zero, Quaternion.identity);

                m_Ship = newPlayerShip.GetComponent<SpaceShip>();

                m_CameraController.SetTarget(m_Ship.transform);
                m_MovementController.SetTargetShip(m_Ship);
                m_Ship.EventOnDeath.AddListener(OnShipDeath);
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
