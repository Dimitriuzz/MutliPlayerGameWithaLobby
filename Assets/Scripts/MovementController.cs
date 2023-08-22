using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SpaceShooter
{
    public class MovementController : MonoBehaviour
    {
        public enum ControlMode
        {
            Keyboard,
            Mobile
        }
        private PhotonView _myView;
        [SerializeField] private SpaceShip m_TargetShip;
        public void SetTargetShip(SpaceShip ship) => m_TargetShip = ship;

        [SerializeField] private VirtualJoystick m_MobileJoystick;

        [SerializeField] private PointerClickHold m_MobileFirePrimary;
        [SerializeField] private PointerClickHold m_MobileFireSecondary;

        // [SerializeField] private ControlMode m_ControlMode;

        private void Start()
        {
            m_MobileJoystick = FindObjectOfType<VirtualJoystick>();
            var buttons = FindObjectsOfType<PointerClickHold>();
            if (buttons[0].transform.name== "VirtualButton_FirePrimary")
            {
                m_MobileFirePrimary = buttons[0];
                m_MobileFireSecondary = buttons[1];
            }
            else
            {
                m_MobileFirePrimary = buttons[1];
                m_MobileFireSecondary = buttons[0];
            }

            _myView = GetComponent<PhotonView>();
            
        }

        /*private void Start()
        {
            if(m_ControlMode == ControlMode.Mobile)
            {
                
                m_MobileJoystick.gameObject.SetActive(true);
        m_MobileFirePrimary.gameObject.SetActive(true);
        m_MobileFireSecondary.gameObject.SetActive(true);
            }
            else
            {
                
                m_MobileJoystick.gameObject.SetActive(false);
        m_MobileFirePrimary.gameObject.SetActive(false);
        m_MobileFireSecondary.gameObject.SetActive(false);
            }
        }*/

        private void Update()
        {
            

            if (!_myView.IsMine) return;

            if (m_TargetShip == null) return;

            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0|| Input.GetKey(KeyCode.Space)|| Input.GetKey(KeyCode.X))
            { ControlKeyboard(); }
            else
            { ControlMobile(); }

        }

        private void ControlMobile()
        {
            /*Vector3 dir = m_MobileJoystick.Value;
            var dot = Vector2.Dot(dir, m_TargetShip.transform.up);
            var dot2 = Vector2.Dot(dir, m_TargetShip.transform.right);

            m_TargetShip.ThrustControl = Mathf.Max(0, dot);
            m_TargetShip.TorqueControl = -dot2;*/
            var dir = m_MobileJoystick.Value;
            m_TargetShip.ThrustControl = dir.y;
            m_TargetShip.TorqueControl = -dir.x;

            if (m_MobileFirePrimary.IsHold) m_TargetShip.Fire(TurretMode.Primary);
            if (m_MobileFireSecondary.IsHold) m_TargetShip.Fire(TurretMode.Secondary);


        }
        private void ControlKeyboard()
        {
            float thrust = 0;
            float torque = 0;
            if (Input.GetKey(KeyCode.W)) thrust = 1.0f;
            if (Input.GetKey(KeyCode.S)) thrust = -1.0f;
            if (Input.GetKey(KeyCode.A)) torque = 1.0f;
            if (Input.GetKey(KeyCode.D)) torque = -1.0f;
            if (Input.GetKey(KeyCode.Space)) m_TargetShip.Fire(TurretMode.Primary);
            if (Input.GetKey(KeyCode.X)) m_TargetShip.Fire(TurretMode.Secondary);
            

            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torque;

        }

    }
}
