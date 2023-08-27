using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RocketPiglet
{
    public class PowerUpWeapon : PowerUp
       
    {
        [SerializeField] private TurretProperties m_Properties;
        protected override void OnPickedUp(Piglet ship)
        {
            ship.AssignWeapon(m_Properties);
        }
    }
}
