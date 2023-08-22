using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public enum ImpactEffectType
    { 
        Explosion,
            Plasma
    }

    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float m_LifeTime;
        private float m_Timer;
        [SerializeField] private float m_EffectRange;
        [SerializeField] private ImpactEffectType m_EffectType;
        [SerializeField] private int m_EffectDamage;
        

        private void Start()
        {
            if (m_EffectType == ImpactEffectType.Plasma)
            {
                RaycastHit2D[] hit;
                hit = Physics2D.RaycastAll(transform.position, transform.up, m_EffectRange);
                transform.Translate(0, 3, 0, Space.Self);
                for (int i = 0; i < hit.Length; i++)
                {
                    //Debug.Log("Got :" + hit[i].transform.root.name);
                    Destructable dest = hit[i].collider.transform.root.GetComponent<Destructable>();
                    if (dest != null)
                    {
                        dest.ApplyDamage(m_EffectDamage);
                        //Debug.Log("Got :" + hit[i].transform.root.name+"dealt :"+m_EffectDamage);
                    }
                }
            }

            if (m_EffectType == ImpactEffectType.Explosion)
            {
                Collider2D[] hit;
                hit = Physics2D.OverlapCircleAll(transform.position, m_EffectRange);

                for (int i = 0; i < hit.Length; i++)
                {
                    //Debug.Log("Got :" + hit[i].transform.root.name);
                    Destructable dest = hit[i].transform.root.GetComponent<Destructable>();
                    if (dest != null)
                    {
                        dest.ApplyDamage(m_EffectDamage);
                        Debug.Log("Hit :" + hit[i].transform.root.name);
                    }
                }
            }


            /*SpriteRenderer m_EffectSprite = gameObject.GetComponent<SpriteRenderer>();
            if (m_EffectSprite!=null)
            {
                var m_Ratio = m_EffectSprite.sprite.rect.width / (m_EffectRange*2);
                var m_Ratio = m_EffectSprite.sprite.rect.height / (m_EffectRange*2);
                m_EffectSprite.sprite.rect.width = m_EffectSprite.sprite.rect.width * m_Ratio;
                m_EffectSprite.size.y = m_EffectRange * 2;
            }*/
        }

        void Update()
        {
            if (m_Timer < m_LifeTime)
                m_Timer += Time.deltaTime;
            else
                Destroy(gameObject);
        }

        /*private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_EffectType == ImpactEffectType.Explosion)
            {
                Destructable dest = collision.collider.transform.root.GetComponent<Destructable>();
                dest.ApplyDamage(m_EffectDamage);
            }
        }*/
    }
}
