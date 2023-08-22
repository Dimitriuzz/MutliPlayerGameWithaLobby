using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AI_Controller : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            RandomPatrol,
            PointPatrol
        }

        [SerializeField] private AIBehaviour m_AIBehaviour;
        [SerializeField] private AIPointPatrol m_AIPatrolPoint;

        [SerializeField] private GameObject[] m_PatrolPoints;
        [SerializeField] private int m_PatrolPointsNumber;
        private int m_CurrentPatrolPointNumber = 0;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;

        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLength;

        private SpaceShip m_SpaceShip;

        private Vector3 m_MovePosition;

        private Destructable m_SelectedTarget;

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();

            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();

        
            
        }

        public void SetRandomPatrolBehaviour(AIPointPatrol point)
        { 
            m_AIBehaviour = AIBehaviour.RandomPatrol;
            m_AIPatrolPoint = point;

            

        }

        private void UpdateAI()
        {
            if(m_AIBehaviour==AIBehaviour.Null)
            {

            }

            else
            {
                UpdateBehaviourPatrol();
            }

        }

        private void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionFire();
            ActionEvadeCollision();


        }

        private void ActionFindNewMovePosition()
        {
            if(m_AIBehaviour==AIBehaviour.RandomPatrol)
                {
                if(m_SelectedTarget!=null)
                {
                    m_MovePosition = m_SelectedTarget.transform.position;
                }
                else
                {
                    if(m_AIPatrolPoint!=null)
                    {
                        bool isInsidePatrolZone = (m_AIPatrolPoint.transform.position - transform.position).sqrMagnitude < m_AIPatrolPoint.Radius * m_AIPatrolPoint.Radius;
                        
                        if(isInsidePatrolZone==true)
                        {
                            if (m_RandomizeDirectionTimer.IsFinished==true)
                            {
                                Vector2 newPoint = transform.position;
                                m_MovePosition = newPoint;
                                m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
                            }
                        }
                        else
                        {
                            m_MovePosition = m_AIPatrolPoint.transform.position;
                        }

                    }
                    
                }

                }
            if (m_AIBehaviour == AIBehaviour.PointPatrol)
            {
                if (m_SelectedTarget != null)
                {
                    var rig = m_SelectedTarget.transform.root.GetComponent<Rigidbody2D>();
                    Vector3 vel = rig.velocity;
                    m_MovePosition = m_SelectedTarget.transform.position+vel;
                }
                else
                {
                    //Debug.Log(transform.position+" number " + m_CurrentPatrolPointNumber + " " + m_PatrolPoints[m_CurrentPatrolPointNumber].transform.position);
                    if (m_PatrolPoints[m_CurrentPatrolPointNumber].transform.position != null)
                    {
                        
                            
                                Vector2 newPoint = m_PatrolPoints[m_CurrentPatrolPointNumber].transform.position;
                                m_MovePosition = newPoint;
                                float dist = Vector2.Distance(m_SpaceShip.transform.position, newPoint);
                                if (dist<1) m_CurrentPatrolPointNumber++;
                                if (m_CurrentPatrolPointNumber >= m_PatrolPoints.Length) m_CurrentPatrolPointNumber = 0;
                            
                        
                    }

                }

            }
        }
        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;
            m_SpaceShip.TorqueControl = ComputeAlignTorqueNormalized(m_MovePosition, m_SpaceShip.transform)*m_NavigationAngular;
        }

        private const float MAX_ANGLE = 45.0f;
        private static float ComputeAlignTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);
            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);
            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE)/MAX_ANGLE;
            return -angle;
        }

        private void ActionEvadeCollision()
        {
            if(Physics2D.Raycast(transform.position, transform.up,m_EvadeRayLength)==true)
            {
                m_MovePosition = transform.position + transform.right * 200f;
            }

        }
        private void ActionFindNewAttackTarget()
        {
           
                if (m_FindNewTargetTimer.IsFinished == true)
                {
                    m_SelectedTarget = FindNearestDestructableTarget();
                    m_FindNewTargetTimer.Start(m_ShootDelay);
                }
            

        }
        private void ActionFire()
        {
            if(m_SelectedTarget!=null)
            {
                if(m_FireTimer.IsFinished==true)
                {
                    m_SpaceShip.Fire(TurretMode.Primary);
                    m_SpaceShip.Fire(TurretMode.Secondary);
                    m_FireTimer.Start(m_ShootDelay);
                }
            }

        }

        private Destructable FindNearestDestructableTarget()
        {
            float maxDist = float.MaxValue;
            Destructable potentialTarget = null;

            foreach(var v in Destructable.AllDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;
                if (v.TeamId == Destructable.TeamIdNeutral) continue;
                if (v.TeamId == m_SpaceShip.TeamId) continue;

                float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);

                if(dist<maxDist)
                {
                    maxDist = dist;
                    potentialTarget = v;
                }
            }

            return potentialTarget;
        }



        #region Timers

        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FireTimer = new Timer(m_ShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
        }

        #endregion

    }
}
