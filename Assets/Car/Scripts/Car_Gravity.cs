using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [RequireComponent(typeof(Car_PlayerMovement))]
    public class Car_Gravity : MonoBehaviour
    {
        public float acceleration = 9.8f;
        public Vector3 direction = Vector3.down;
        public float maxAngle = 30;
        Rigidbody _rb;
        Car_PlayerMovement _car;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _car = GetComponent<Car_PlayerMovement>();
        }

        void FixedUpdate()
        {
            if (_car.WheelsGrounded() || Vector3.Angle(Vector3.down, -transform.up.normalized) < maxAngle)
            {
                direction = -transform.up.normalized;
            }
            _rb.velocity += direction * acceleration * Time.fixedDeltaTime;
        }
    }