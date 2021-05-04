using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car
{
    public class Car_Steering : MonoBehaviour
    {
        public float maxSteeringSpeed = 50f;
        float speedSteeringScale = 0.1f;

        Rigidbody _rb;

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // float velocitySteeringScale = 1 - 1 / (_rb.velocity.magnitude * speedSteeringScale + 1);
            float velocitySteeringScale = 1;
            _rb.angularVelocity = -Vector3.up * TouchInput.centeredScreenPosition.x * maxSteeringSpeed * velocitySteeringScale;
        }
    }
}