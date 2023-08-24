using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;
using TMPro;


namespace SpaceShooter
{
    public class Destructable : MonoBehaviourPunCallbacks, IPunObservable
    {

        #region Properties

        [SerializeField] protected bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Начальные хитпоинты
        /// </summary>
        [SerializeField] public int m_HitPoints;
        [SerializeField] public int damagesOnCollision;
        [SerializeField] GameObject m_Explosion;

        [SerializeField] private TMP_Text HP;
        [SerializeField] private TMP_Text name;
        [SerializeField] private Image HPBar;

        PhotonView photonView;

        public string playerName;


        protected int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;
        public int MaxHitPoints => m_HitPoints;


        
        #endregion

        #region Unity Events
        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
            if(HP!=null) HP.text = m_CurrentHitPoints.ToString();
            if (HPBar != null)
            {
                float hpbarfil = (float)HitPoints / (float)MaxHitPoints;
                HPBar.fillAmount = hpbarfil;
            }
            photonView = GetComponent<PhotonView>();
            playerName = photonView.Owner.NickName;
            if (TryGetComponent<SpaceShip>(out SpaceShip ship)) name.text=playerName;
        }
        #endregion

        #region Public API
        
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;
            gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            
            /*
            if (m_CurrentHitPoints <= 0)
            {
                OnDeath();
            }
            */

        }

        [PunRPC]
        public void TakeDamage(int damage)
        {
            m_CurrentHitPoints -= damage;
            if (m_CurrentHitPoints > m_HitPoints) m_CurrentHitPoints = m_HitPoints;
            if (HPBar != null)
            {
                if (HP != null) HP.text = m_CurrentHitPoints.ToString();
                float hpbarfil = (float)HitPoints / (float)MaxHitPoints;
                HPBar.fillAmount = hpbarfil;
            }

            //var end = FindObjectOfType<RestartGame>();
            //end.ResultsDispaly("zzz", 1);

            if (transform.tag=="Player"&& m_CurrentHitPoints <= 0)
            {

               
                var players = FindObjectsOfType<SpaceShip>();
                Debug.Log("number of players" + players.Length);
                var end = FindObjectOfType<RestartGame>();
                
                foreach (var player in players)
                {
                    Debug.Log(player.playerName + player.HitPoints + " " + player.goldCollected);
                    if (player.HitPoints > 0) end.ResultsDispaly(player.playerName, player.goldCollected);
                    ///Destroy(player);
                    Time.timeScale = 0;
                }

            }

            //healthbar.fillAmount = health / startHealth;


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
            if (m_Explosion != null)
            {
                var ex = Instantiate(m_Explosion);
                ex.transform.position = transform.position;
                Destroy(ex, 2);
            }
            Destroy(gameObject);
            m_EventOnDeath?.Invoke();
        }

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        private static HashSet<Destructable> m_AllDestructibles;

        public static IReadOnlyCollection<Destructable> AllDestructibles => m_AllDestructibles;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(this.m_HitPoints);
                stream.SendNext(this.m_CurrentHitPoints);
            }
            else
            {
                // Network player, receive data
                this.m_HitPoints = (int)stream.ReceiveNext();
                this.m_CurrentHitPoints = (int)stream.ReceiveNext();
            }
        }

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
