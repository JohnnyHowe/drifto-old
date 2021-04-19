﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public bool accelerating = true;
    public float accelerationForce = 1;     // nm
    public float maxSpeed = 10;             // m/s
    public float maxAngularVelocity = 30;    // degrees per second
    public float screenUse = 0.8f;  // How much of the screen to use for turning? max turn is when touch at screenUse of screen width
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>(); 
    }

    void FixedUpdate()
    {
        // Speed
        if (accelerating) {
            _rb.AddForce(_rb.transform.forward.normalized * accelerationForce) ;
        }
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed);

        // Steering
        float steering = Mathf.Clamp(TouchInput.centeredScreenPosition.x / screenUse, -1, 1);
        _rb.angularVelocity = -Vector3.up * steering * maxAngularVelocity * Mathf.PI / 180;
    }
}
