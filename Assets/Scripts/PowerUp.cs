using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RocketPiglet
{
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class PowerUp : Destructable
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Piglet ship = collision.transform.root.GetComponent<Piglet>();

            if(ship!=null)
            {
                OnPickedUp(ship);
                Destroy(gameObject);
            }
        }

        protected abstract void OnPickedUp(Piglet ship);

    }
}
