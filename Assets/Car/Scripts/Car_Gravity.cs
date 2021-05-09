using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car
{
    [RequireComponent(typeof(Car_PlayerMovement))]
    public class Car_Gravity : MonoBehaviour
    {
        public float acceleration = 9.8f;
        public Vector3 direction = Vector3.down;
        Rigidbody _rb;
        Car_PlayerMovement _car;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _car = GetComponent<Car_PlayerMovement>();
        }

        void FixedUpdate()
        {
            if (_car.WheelsGrounded())
            {
                direction = -transform.up.normalized;
            }
            _rb.velocity += direction * acceleration * Time.fixedDeltaTime;
        }
    }
}