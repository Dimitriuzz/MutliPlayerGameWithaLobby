using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace RocketPiglet
{
    public class Asteroid : Destructable
    {
        private enum AsteroidSize
        {
            Big,
            Small
        }
        [SerializeField] private AsteroidSize m_asteroidSize;
        [SerializeField] private float m_Speed;
       // [SerializeField] private CircleZone m_Zone;
       // private Vector2 m_Target;
        // Start is called before the first frame update
        

        

        protected override void OnDeath()
        {
           if (m_asteroidSize==AsteroidSize.Big)
            {
                SpawnAsteroids();
                
            }
            base.OnDeath();
        }

        private void SpawnAsteroids()
        {
            int j = 1;
            m_asteroidSize= m_asteroidSize - 1;
            for (int i = 0; i < 2; i++)
            {
                Vector3 pos = new Vector3(this.transform.position.x + j, this.transform.position.y, 0);
                var asteroid = PhotonNetwork.Instantiate("Asteroid", pos, Quaternion.identity);   
                
                asteroid.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                //asteroid.m_HitPoints = asteroid.MaxHitPoints / 2;
                
                Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();

                if (rb != null && m_Speed > 0)
                {
                    rb.velocity = (Vector2)Random.insideUnitSphere * m_Speed;
                }
                j = -1;
                
            }

        }
    }
}
