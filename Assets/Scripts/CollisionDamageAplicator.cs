using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RocketPiglet
{ 
public class CollisionDamageAplicator : MonoBehaviour
    {
        public static string IgnoreTag = "WorldBoundry";

        [SerializeField] private float m_VelocityDamageModifier;
        [SerializeField] private float m_DamageConstant;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == IgnoreTag) return;

            if (collision.transform.tag == "Coin")
            {
                if (TryGetComponent<Piglet>(out Piglet ship))
                {
                    ship.goldCollected++;
                    Destroy(collision.gameObject);
                }
            }
            var destructible = transform.root.GetComponent<Destructable>();
            var col = collision.transform.root.GetComponent<Destructable>();

            if(destructible != null)
            {
                destructible.ApplyDamage(col.damagesOnCollision);
                col.ApplyDamage(destructible.damagesOnCollision);
                /*if(col==null)
                destructible.ApplyDamage((int)m_DamageConstant +
                    (int)(m_VelocityDamageModifier * collision.relativeVelocity.magnitude));
                else
                {
                    if (destructible.TeamId == 2)
                    {
                        destructible.ApplyDamage((int)m_DamageConstant +
                                            (int)(m_VelocityDamageModifier * collision.relativeVelocity.magnitude));

                        col.ApplyDamage((int)m_DamageConstant +
                                                                    (int)(m_VelocityDamageModifier * collision.relativeVelocity.magnitude));
                    }
                }*/



            }

            
        }


    }
}
