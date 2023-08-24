using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace SpaceShooter
{
    public class EntitySpawner : MonoBehaviour
    {
       public enum SpawnMode
        {
            Start,
            Loop
        }

        [SerializeField] private Destructable[] m_EntityPrefabs;
        [SerializeField] private CircleZone m_Area;
        [SerializeField] private SpawnMode m_SpawnMode;
        [SerializeField] private int m_NumSpawns;
        [SerializeField] private int m_NumCoinsSpawns;
        [SerializeField] private float m_RespawnTime;
        [SerializeField] private float coinRespawnTime;

        private float m_TimerEntity;
        private float m_TimerCoin;

        private void Start()
        {
            if(m_SpawnMode==SpawnMode.Start)
            {
                SpawnEntities();
            }

            m_TimerEntity = m_RespawnTime;
            m_TimerCoin = coinRespawnTime;

        }

        private void Update()
        {
            if (m_TimerEntity > 0)
                m_TimerEntity -= Time.deltaTime;

            if(m_SpawnMode==SpawnMode.Loop&& m_TimerEntity < 0)
            {
                SpawnEntities();

                m_TimerEntity = m_RespawnTime;
            }

            if (m_TimerCoin > 0)
                m_TimerCoin -= Time.deltaTime;

            if (m_SpawnMode == SpawnMode.Loop && m_TimerCoin < 0)
            {
                SpawnCoin();

                m_TimerCoin= coinRespawnTime;
            }
        }

        private void SpawnEntities()
        {
            for(int i=0;i<m_NumSpawns;i++)
            {
                int index = Random.Range(0, m_EntityPrefabs.Length);
                
                PhotonNetwork.Instantiate(m_EntityPrefabs[index].transform.name, m_Area.GetRandomInsideZone(), Quaternion.identity);
                
            }

        }

        private void SpawnCoin()
        {
            for (int i = 0; i < m_NumCoinsSpawns; i++)
            {
                PhotonNetwork.Instantiate("Coin", m_Area.GetRandomInsideZone(), Quaternion.identity);

            }
           
        }
    }
}
