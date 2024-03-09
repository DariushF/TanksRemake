using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        public GameObject upperPart;
        public GameObject lowerPart;

        [SerializeField]
        private float movementSpeed = 5f;

        [SerializeField]
        private float angularSpeed = 200f;

        private Vector2 moveDir = Vector2.zero; // stores direction in which player is currently moving
        private Vector2 lastMoveDirNonZero = Vector2.zero;

        private Rigidbody2D rb;
        private BoxCollider2D col;

        private void Start()
        {
            if (!IsOwner)
                return;

            movementSpeed = 5f;

            angularSpeed = 200f;

            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (!IsOwner) // ensures that only own player is moved
                return;

            HandleMovement();
            HandleRotation();
        }

        public void HandleMovement()
        {
            float xDir = Input.GetAxisRaw("Horizontal");
            float yDir = Input.GetAxisRaw("Vertical");

            moveDir = new Vector2(xDir, yDir).normalized;
            if (moveDir != Vector2.zero)
                lastMoveDirNonZero = moveDir;

            rb.velocity = movementSpeed * moveDir;
        }

        /// <summary>
        /// main camera is tilted to achieve 3d look (x rotation -20 deg) and mouse position needs to be projected onto x-y-plane. 
        /// To achieve that a ray is cast from the mouse position on screen towards x-y plane and intersect point is calculated
        /// </summary>
        public void HandleRotation()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.back, Vector3.zero); // x-y-plane

            Vector3 mousePos = Vector3.zero;

            // this should never be false because of how camera and plane are rotated and positioned
            if (plane.Raycast(ray, out float distance))
            {
                mousePos = ray.GetPoint(distance);
            }

            HandleUpperPartRotation(mousePos);
            HandleLowerPartRotation();
        }

        /// <summary>
        /// Rotates the upper part of the tank towards the mouse position
        /// 
        /// calculates target rotation using trigonometry
        /// </summary>
        public void HandleUpperPartRotation(Vector3 mousePos)
        {
            Quaternion targetRotation = Quaternion.Euler(Mathf.Rad2Deg * Mathf.Atan2(mousePos.y - upperPart.transform.position.y, mousePos.x - upperPart.transform.position.x) * Vector3.forward);

            upperPart.transform.rotation = Quaternion.RotateTowards(upperPart.transform.rotation, targetRotation, Time.deltaTime * angularSpeed); // rotate with constant speed
        }

        /// <summary>
        /// rotates lower part of tank according to movement direction
        /// 
        /// zRotation is changed as lower part is symmetrical so we don't want to make turns greater than 90 deg as 
        /// same can be achieved by rotating less than 90 deg in opposite direction
        /// </summary>
        public void HandleLowerPartRotation()
        {
            float zRot = Mathf.Rad2Deg * Mathf.Atan2(lastMoveDirNonZero.y, lastMoveDirNonZero.x);

            while (zRot - lowerPart.transform.rotation.eulerAngles.z > 90)
                zRot -= 180;
            while (lowerPart.transform.rotation.eulerAngles.z - zRot > 90)
                zRot += 180;

            Quaternion targetRotation = Quaternion.Euler(zRot * Vector3.forward);

            lowerPart.transform.rotation = Quaternion.RotateTowards(lowerPart.transform.rotation, targetRotation, Time.deltaTime * angularSpeed); // rotate with constant speed
        }
    }
}
