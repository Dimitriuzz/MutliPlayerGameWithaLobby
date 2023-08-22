using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class AsteroidMovement : MonoBehaviour
    {
        [SerializeField] Rigidbody2D asteroid;
        [SerializeField] private float m_Mass;
        [SerializeField] private Vector2 m_Thrust;
        void Start()
        {
            asteroid.mass = m_Mass;
            
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            asteroid.AddForce(m_Thrust, ForceMode2D.Force);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            m_Thrust.x = -m_Thrust.x;
            m_Thrust.y = -m_Thrust.y;
        }
    }
}
