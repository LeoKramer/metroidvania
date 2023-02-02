using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania
{
    public class Jump : Abilities
    {
        [SerializeField]
        protected bool limitAirJumps;
        [SerializeField]
        protected int maxJumps;
        [SerializeField]
        protected float jumpForce;
        [SerializeField]
        protected float holdForce;
        [SerializeField]
        protected float buttonHoldTime;
        [SerializeField]
        protected float distanceToCollider;
        [SerializeField]
        protected float maxJumpSpeed;
        [SerializeField]
        protected float maxFallSpeed;
        [SerializeField]
        protected float acceptedFallSpeed;
        [SerializeField]
        protected LayerMask collisionLayer;

        private bool isJumping;
        private float jumpCountDown;
        private int numberOfJumpsLeft;

        protected override void Initialization()
        {
            base.Initialization();
            numberOfJumpsLeft = maxJumps;
            jumpCountDown = buttonHoldTime;
        }

        protected virtual void Update()
        {
            JumpPressed();
            JumpHeld();
        }

        protected virtual bool JumpPressed()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bool fellFromEdge = !character.isGrounded && numberOfJumpsLeft == maxJumps;
                bool fallingTooFast = limitAirJumps && Falling(acceptedFallSpeed);

                if (fellFromEdge || fallingTooFast)
                {
                    isJumping = false;
                    return false;
                }

                numberOfJumpsLeft--;

                if (numberOfJumpsLeft >= 0)
                {
                    jumpCountDown = buttonHoldTime;
                    isJumping = true;
                }

                return true;
            }
            
            return false;
        }

        protected virtual bool JumpHeld()
        {
            return Input.GetKey(KeyCode.Space);
        }

        protected virtual void FixedUpdate()
        {
            IsJumping();
            GroundCheck();
        }

        protected virtual void IsJumping()
        {
            if (isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce);

                AdditionalAir();
            }
            if (rb.velocity.y > maxJumpSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed);
            }
        }

        protected virtual void AdditionalAir()
        {
            if (JumpHeld())
            {
                jumpCountDown -= Time.deltaTime;

                if (jumpCountDown <= 0)
                {
                    jumpCountDown = 0;
                    isJumping = false;
                }
                else
                {
                    rb.AddForce(Vector2.up * holdForce);
                }
            }
            else
            {
                isJumping = false;
            }
        }

        protected virtual void GroundCheck()
        {
            if (CollisionCheck(Vector2.down, distanceToCollider, collisionLayer) && !isJumping)
            {
                character.isGrounded = true;
                numberOfJumpsLeft = maxJumps;
            }
            else
            {
                character.isGrounded = false;

                if (Falling(0) && rb.velocity.y < maxFallSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
                }
            }
        }
    }
}

