using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SpaceShooter
{
    public class CircleZone : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;

        public Vector2 GetRandomInsideZone()
        {
            return (Vector2)transform.position + (Vector2)Random.insideUnitSphere * m_Radius;
        }
#if UNITY_EDITOR
        private static Color GismoColor = new Color(0, 1, 1, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Handles.color = GismoColor;
            Handles.DrawSolidDisc(transform.position, transform.forward, m_Radius);
        }
#endif
    }
}
