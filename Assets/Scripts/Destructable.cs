using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace SpaceShooter
{
    public class Destructable : Entity
    {

        #region Properties

        [SerializeField] protected bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Начальные хитпоинты
        /// </summary>
        [SerializeField] public int m_HitPoints;
        [SerializeField] GameObject m_Explosion;

        /// <summary>
        /// Текущие хитпоинты
        /// </summary>
        protected int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;
        public int MaxHitPoints => m_HitPoints;



        #endregion

        #region Unity Events
        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
        }
        #endregion

        #region Public API
        /// <summary>
        /// Применение дамага к объекту
        /// </summary>
        /// <param name="damage"> урон наносимый объекту </param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;
            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
            {
                OnDeath();
            }


        }
        #endregion
        protected virtual void OnDeath()
        {
            /*if (TeamId == 1) 
            {
                GamePlayer.AddKill();
               
            }
            if (TeamId != 2)
            {
                GamePlayer.Instance.AddScore(ScoreValue);
               
            }*/
            var ex = Instantiate(m_Explosion);
            ex.transform.position = transform.position;
            Destroy(ex, 2);
            Destroy(gameObject);
            m_EventOnDeath?.Invoke();
        }

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        private static HashSet<Destructable> m_AllDestructibles;

        public static IReadOnlyCollection<Destructable> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
                m_AllDestructibles = new HashSet<Destructable>();
            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;

    }
}
