using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

namespace Bullet
{
    public class BulletEventArgs : EventArgs
    {
        public BulletBehaviour bullet;
    }

    public class BulletBehaviour : NetworkBehaviour
    {
        public delegate void DestroyedEventHandler(object source, BulletEventArgs args);
        public event DestroyedEventHandler Destroyed;

        private Rigidbody2D rb;

        public float bulletVelocity;

        public int maxCollisionCount = 3;
        private int wallCollisions = 0;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector2(-rb.velocity.y, rb.velocity.x));
        }

        /// <summary>
        /// Destroys Bullet on hit with other Bullet/Player
        /// Destroys Bullet on third hit with wall
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Destroy(this.gameObject);
            }
            else if (collision.gameObject.CompareTag("Bullet"))
            {
                Destroy(this.gameObject);
            }
            else if (collision.gameObject.CompareTag("Wall"))
            {
                wallCollisions++;
                if (wallCollisions >= maxCollisionCount)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        public override void OnDestroy()
        {
            if (Destroyed != null)
                Destroyed(this, new BulletEventArgs() { bullet = this });
        }
    }
}
