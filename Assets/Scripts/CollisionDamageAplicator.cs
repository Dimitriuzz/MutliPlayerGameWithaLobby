using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceShooter
{ 
public class CollisionDamageAplicator : MonoBehaviour
    {
        public static string IgnoreTag = "WorldBoundry";

        [SerializeField] private float m_VelocityDamageModifier;
        [SerializeField] private float m_DamageConstant;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == IgnoreTag) return;

            var destructible = transform.root.GetComponent<Destructable>();
            var col = collision.transform.root.GetComponent<Destructable>();

            if(destructible != null)
            {
                if(col==null)
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
                }

            }
        }


    }
}
