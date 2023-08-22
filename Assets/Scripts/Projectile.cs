using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceShooter
{
    public class Projectile : Entity
    {
        [SerializeField] private float m_Velocity;  
        [SerializeField] private float m_Lifetime;  
        [SerializeField] private int m_Damage;  
        [SerializeField] private ImpactEffect m_ImpactEffectPrefab;
        private float m_Timer;

        private void Update()
        {
            float stepLength = Time.deltaTime * m_Velocity;
            Vector2 step = transform.up * stepLength;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);
            if(hit)
            {
                Destructable dest = hit.collider.transform.root.GetComponent<Destructable>();
                //Debug.Log(m_Parent.transform.name + " hit " + dest.name+"teamid1="+ m_Parent.TeamId+ "teamid2="+ dest.TeamId);
                if(dest!=null&&dest!=m_Parent&&m_Parent!=null)
                {
                    if (m_Parent.TeamId != dest.TeamId)
                    {
                        dest.ApplyDamage(m_Damage);
                        //Debug.Log("damage " + m_Damage);
                    }
                    /*if(m_Parent==Player.Instance.ActiveShip)
                    {
                        Player.Instance.AddScore(dest.ScoreValue);
                    }*/
                }
                OnProjectileLifeEnd(hit.collider, hit.point);

                if (m_ImpactEffectPrefab != null)
                {
                   ImpactEffect m_Impact= Instantiate(m_ImpactEffectPrefab);
                    m_Impact.transform.position = transform.position;
                    m_Impact.transform.up = transform.up;
                    
                    
                }
            }

            

            m_Timer += Time.deltaTime;
            if (m_Timer > m_Lifetime)
            {
                /*if (m_ImpactEffectPrefab != null)
                {
                    Instantiate(m_ImpactEffectPrefab, new Vector3(transform.position.x, transform.position.y, 0).normalized, Quaternion.identity);
                }*/
                Destroy(gameObject);
            }
            if (transform.tag == "Rocket")
            {
                RocketMovement();
            }
            else
            {
                transform.position += new Vector3(step.x, step.y, 0);
            }
        }

    private void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {

            Destroy(gameObject);
        }
        private Destructable m_Parent;
        public void SetParentShooter(Destructable parent)
        {
            m_Parent = parent;
        }
    private void RocketMovement()
        {
            Collider2D[] hit;
            hit = Physics2D.OverlapCircleAll(transform.position, m_Velocity*m_Lifetime);
            //Debug.Log("Range :" + m_Velocity * m_Lifetime);
            float m_MinDistance=10000000;
            float m_CurrentDistance;
            int m_nearestIndex=0;
            for (int i = 0; i < hit.Length; i++)
            {
                //Debug.Log("Got :" + hit[i].transform.root.name);
                Destructable dest = hit[i].transform.root.GetComponent<Destructable>();
            
                if (dest != null&&dest!=m_Parent)
                {
                    m_CurrentDistance = (dest.transform.position - transform.position).magnitude;
                    if (m_CurrentDistance < m_MinDistance)
                    {
                        m_MinDistance = m_CurrentDistance;
                        m_nearestIndex = i;
                    }

                    //Debug.Log("Hit :" + hit[i].transform.root.name+" distance :"+m_CurrentDistance+" coord"+hit[i].transform.position);
                }
            }
            //Debug.Log("Nearest :" + hit[m_nearestIndex].transform.root.name + " coord" + hit[m_nearestIndex].transform.position);
            if (hit[m_nearestIndex] != null && hit[m_nearestIndex] != m_Parent)
            {
                transform.up = hit[m_nearestIndex].transform.position - transform.position;
                transform.position = Vector3.MoveTowards(transform.position, hit[m_nearestIndex].transform.position, m_Velocity * Time.deltaTime);
            }
            else
            {
                float stepLength = Time.deltaTime * m_Velocity;
                Vector2 step = transform.up * stepLength;
                transform.position += new Vector3(step.x, step.y, 0);
            }

        }
    }


}
