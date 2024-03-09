using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Bullet;
using System;

namespace Player
{
    public class PlayerShooting : NetworkBehaviour
    {
        public GameObject BulletPrefab;
        public Transform bulletSpawnPoint;

        [SerializeField]
        private float startTimeBtwShoot = 0.3f;
        private float timeBtwShoot = 0;

        [Header("Bullet Settings")]
        public Transform upperPart;
        public float bulletVelocity;

        private LinkedList<BulletBehaviour> bullets = new LinkedList<BulletBehaviour>(); // keep track of bullets alive

        private void Update()
        {
            HandleShootingInput();   
        }

        /// <summary>
        /// Checks that at max 3 bullets are alive at any time
        /// Prevents bullet spaming by setting min interval between shoots to startTimeBtwShoot
        /// </summary>
        public void HandleShootingInput()
        {
            if (timeBtwShoot <= 0)
            {
                if (Input.GetMouseButtonDown(0) && bullets.Count < 3)
                {
                    Shoot();
                    timeBtwShoot = startTimeBtwShoot;
                }
            }
            else
            {
                timeBtwShoot -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Spawns bullet
        /// </summary>
        public void Shoot()
        {
            GameObject _bulletGO = Instantiate(BulletPrefab, bulletSpawnPoint.position, upperPart.rotation);
            _bulletGO.GetComponentInChildren<Rigidbody2D>().velocity = bulletVelocity * _bulletGO.transform.right;
            BulletBehaviour _bullet = _bulletGO.GetComponent<BulletBehaviour>();
            _bullet.bulletVelocity = bulletVelocity;
            bullets.AddLast(_bullet);
            _bullet.Destroyed += OnBulletDestroyed;
        }

        public void OnBulletDestroyed(object source, BulletEventArgs args)
        {
            bullets.Remove(args.bullet);
        }
    }
}
