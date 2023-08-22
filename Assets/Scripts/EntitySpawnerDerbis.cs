using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceShooter
{
    public class EntitySpawnerDerbis : MonoBehaviour
    {
        [SerializeField] private Destructable[] m_DerbisPrefabs;
        [SerializeField] private CircleZone m_Area;
        [SerializeField] private int m_NumDebris;
        [SerializeField] private float m_RandomSpeed;

        
        private void Start()
        {
            for(int i=0;i<m_NumDebris;i++)
            {
                SpawnDebris();            }

        }

        

        private void SpawnDebris()
        {
            int index = Random.Range(0, m_DerbisPrefabs.Length);
            GameObject e = Instantiate(m_DerbisPrefabs[index].gameObject);
            e.transform.position = m_Area.GetRandomInsideZone();
            e.GetComponent<Destructable>().EventOnDeath.AddListener(OnDebrisDead);

            Rigidbody2D rb = e.GetComponent<Rigidbody2D>();

            if(rb!=null&&m_RandomSpeed>0)
            {
                rb.velocity = (Vector2)Random.insideUnitSphere * m_RandomSpeed;
            }

        }

        private void OnDebrisDead()
        {
            SpawnDebris();
        }

        }
    
}

