using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RocketPiglet
{
    public class LevelBoundryLimiter : MonoBehaviour
    {
        private void Update()
        {
            if (LevelBoundry.Instance == null) return;

            var lb = LevelBoundry.Instance;
            var r = lb.Radius;

            if(transform.position.magnitude>r)

            {
                if (lb.LimitMode == LevelBoundry.Mode.Limit)
                {
                    transform.position = transform.position.normalized * r;
                }
                if (lb.LimitMode == LevelBoundry.Mode.Teleport)
                {
                    transform.position = -transform.position.normalized * r;
                }

            }


        }

    }
}
