using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;




namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructable
    {
        [Header("Space ship")]
        [SerializeField] private float m_Mass;
        [SerializeField] private float m_Thrust;
        [SerializeField] private float m_Mobility;
        [SerializeField] private float m_MaxLinearVelocity;
        [SerializeField] private float m_MaxAngularVelocity;

        private Rigidbody2D m_Rigid;
        private ParticleSystem m_Part;

        private PhotonView photonView;

        public float MaxLinearVelocity => m_MaxLinearVelocity;

       
        public float MaxAngularVelocity => m_MaxAngularVelocity;

        [SerializeField] private Sprite m_PreviewImage;
        public Sprite PreviewImage => m_PreviewImage;


        public float ThrustControl { get; set; }

        public float TorqueControl { get; set; }

        private float m_IndestructableTimer = 0;

        private bool m_IsSpeededUp;
        

        private float m_BaseThrust;
        private float m_SpeededTimer;
        private float m_PowerUpSpeed;

        protected override void Start()
        {
            base.Start();
            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;
            m_Rigid.inertia = 1;
            InitOffencive();
            m_Indestructible = false;
            m_BaseThrust = m_Thrust;
            m_Part = GetComponentInChildren<ParticleSystem>();
            if (m_Part == null) Debug.Log("emmision not found");
            photonView = GetComponent<PhotonView>();

            var cameras = GetComponentsInChildren<Camera>();
            if(!photonView.IsMine)
            {
                foreach (var c in cameras) Destroy(c.gameObject);
            }

            var cameracontrol = GetComponent<CameraControl>();

            cameracontrol.SetTarget(transform);


        }

       
        private void FixedUpdate()
        {
            if (!photonView.IsMine) return;
            UpdateRigidBody();
            UpdateEnergyRegen();
            if (m_IndestructableTimer > 0)
            {
                m_IndestructableTimer -= Time.deltaTime;
            }
            else
            {
                m_Indestructible = false;
                var m_emmision = m_Part.emission;
                //m_Part.Pause();
                m_emmision.enabled = false;
            }


            if (m_SpeededTimer > 0)
            {
                m_SpeededTimer -= Time.deltaTime;
                m_Thrust = m_BaseThrust + m_PowerUpSpeed;
            }
            else
            {
                m_Thrust = m_BaseThrust;
                
            }
        }

        private void UpdateRigidBody()
        {
            Debug.Log("trust " + m_Thrust);
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);
            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust /m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);
            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility/m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

        }

        [SerializeField] private Turret[] m_Turret;

        public void Fire(TurretMode mode)
        {
            for (int i=0; i<m_Turret.Length;i++)
            {
                if(m_Turret[i].Mode==mode)
                {
                    m_Turret[i].Fire();
                }
            }
        }

        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyRegenPerSecond;


        public float m_PrimaryEnergy;
        public int m_SecondaryAmmo;

        public void AddEnergy(int e)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
        }

        public void AddAmmo(int a)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + a, 0, m_MaxAmmo);
        }

        private void InitOffencive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
        }

        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy= Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

        public bool DrawEnergy(int count)
        {
            if (count == 0) return true;

            if(m_PrimaryEnergy>=count)
            {
                m_PrimaryEnergy -= count;
                return true;
            }
            return false;
        }

        public bool DrawAmmo(int count)
        {
            if (count == 0) return true;

            if (m_SecondaryAmmo >= count)
            {
                m_SecondaryAmmo -= count;
                return true;
            }
            return false;
        }

        public void AssignWeapon(TurretProperties props)
        {
            for(int i=0;i<m_Turret.Length;i++)
            {
                m_Turret[i].AssignLoadout(props);
            }
        }

        public void Invulerability(float time)
        {
            m_Indestructible = true;
            m_IndestructableTimer = time;
            var m_emmision = m_Part.emission;
            m_emmision.enabled = true;
        }

        public void SpeedUp(float speed, float time)
        {
            m_PowerUpSpeed = speed;
            m_SpeededTimer = time;

        }

        public void HealthUp (int health)
        {
            
           m_CurrentHitPoints += health;
            if (m_CurrentHitPoints > MaxHitPoints) m_CurrentHitPoints = MaxHitPoints;
        }

    }
}
