using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania
{
    public class Jump : Abilities
    {
        [SerializeField]
        protected float jumpForce;
        [SerializeField]
        protected float distanceToCollider;
        [SerializeField]
        protected LayerMask collisionLayer;

        private bool isJumping;

        protected virtual void Update()
        {
            JumpPressed();
        }

        protected virtual bool JumpPressed()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void FixedUpdate()
        {
            IsJumping();
            GroundCheck();
        }

        protected virtual void IsJumping()
        {
            if (!character.isGrounded)
            {
                isJumping = false;
            }
            else if (isJumping)
            {
                rb.AddForce(Vector2.up * jumpForce);
            }
        }

        protected virtual void GroundCheck()
        {
            character.isGrounded = CollisionCheck(Vector2.down, distanceToCollider, collisionLayer);
        }
    }
}

