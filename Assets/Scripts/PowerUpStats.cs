using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PowerUpStats : PowerUp
    {
        public enum EffectType
        {
            AddAmmo,
            AddEnergy,
            Invulerability,
            SpeedUp,
            HealthUp
        }

        [SerializeField] private EffectType m_EffectType;
        [SerializeField] private float m_Value1;
        [SerializeField] private float m_Value2;
        protected override void OnPickedUp(SpaceShip ship)
        {
            if (m_EffectType == EffectType.AddEnergy)
                ship.AddEnergy((int)m_Value1);
            if (m_EffectType == EffectType.AddAmmo)
                ship.AddAmmo((int)m_Value1);
            if (m_EffectType == EffectType.Invulerability)
                ship.Invulerability(m_Value1);
            if (m_EffectType == EffectType.SpeedUp)
                ship.SpeedUp(m_Value1,m_Value2);
            if (m_EffectType == EffectType.HealthUp)
                ship.HealthUp((int)m_Value1);
        }
    }
}
