using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public int speed;
        public int damage;
        public Vector2 forward;
        protected Action ReleaseAction;
        protected Rigidbody2D Rb2;

        protected virtual void Awake()
        {
            Rb2 = GetComponent<Rigidbody2D>();
        }

        protected virtual void FixedUpdate()
        {
            forward = transform.right.normalized;
            Rb2.velocity = forward * (speed * Time.deltaTime);
        }

        public void SetDeactivateAction(Action ra)
        {
            ReleaseAction = ra;
        }
    }
}