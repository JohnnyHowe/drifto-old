using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Car_PlayerMovement : MonoBehaviour
{
    public bool accelerating = true;
    public float drag = 1f;
    public float driveForce = 1;
    public float maxAngularVelocity = 30;    // degrees per second
    public Transform centerOfMass;
    public float screenUse = 0.8f;  // How much of the screen to use for turning? max turn is when touch at screenUse of screen width
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rb.centerOfMass = centerOfMass.localPosition;

        // Speed
        if (accelerating)
        {
            _rb.AddForce(_rb.transform.forward.normalized * driveForce);
        }
        _rb.AddForce(-GetDragForce() * _rb.velocity.normalized);

        // Steering
        float steering = Mathf.Clamp(TouchInput.centeredScreenPosition.x / screenUse, -1, 1);
        _rb.angularVelocity = -Vector3.up * steering * maxAngularVelocity * Mathf.PI / 180;
        // _rb.angularVelocity = new Vector3(_rb.angularVelocity.x, -steering * maxAngularVelocity * Mathf.PI / 180, _rb.angularVelocity.z);
    }

    float GetDragForce()
    {
        return Mathf.Pow(_rb.velocity.magnitude, 2) * drag;
    }
}
