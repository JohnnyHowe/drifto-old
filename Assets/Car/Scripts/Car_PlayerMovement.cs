using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Car_PlayerMovement : MonoBehaviour
{
    [Header("Control")]
    public float throttle = 1;
    public float screenUse = 0.8f;  // How much of the screen to use for turning? max turn is when touch at screenUse of screen width
    [Header("Body")]
    public Transform centerOfMass;
    public Transform groundTrigger;
    public LayerMask wheelCollidables;
    public float drag = 1f;
    [Header("Engine")]
    public float driveForce = 1;
    [Header("Suspension and Steering")]
    public float activeVisualSteeringAngleEffect = 1;
    public float maxVisualSteeringSpeed = 1;
    public float maxVisualSteeringAngle = 30;
    public float maxAngularAcceleration = 30;    // degrees per second
    public List<Transform> steeringWheels;
    public List<Transform> driveWheels;
    private Rigidbody _rb;
    public float driftAngleThreshold = 10.0f;
    public float straighteningStrength = 0.5f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Point wheels
        float wheelAngle = -Vector3.Angle(_rb.velocity.normalized, GetDriveDirection()) * Vector3.Cross(_rb.velocity.normalized, GetDriveDirection()).y;
        wheelAngle = Mathf.Min(Mathf.Max(-maxVisualSteeringAngle, wheelAngle), maxVisualSteeringAngle);
        PointDriveWheelsAt(wheelAngle);
    }

    private float GetRawDriftAngle()
    {
        if (!WheelsGrounded()) return 0;
        return Vector3.Angle(_rb.velocity.normalized, GetDriveDirection()) * Vector3.Cross(_rb.velocity.normalized, GetDriveDirection()).y;
    }

    public float GetDriftAngle()
    {
        return GetRawDriftAngle();
    }

    public bool IsFrontWheelDrift()
    {
        return Mathf.Abs(GetDriftAngle()) > maxVisualSteeringAngle;
    }

    public bool IsDrifting()
    {
        return Mathf.Abs(GetDriftAngle()) > driftAngleThreshold;
    }

    void FixedUpdate()
    {
        // turn in travel direction
        if (Mathf.Abs(TouchInput.centeredScreenPosition.x) < 0.2f)
        {
            if (Mathf.Abs(GetRawDriftAngle()) < 2)
            {
                float s = -GetRawDriftAngle() * straighteningStrength * 50;
                _rb.angularVelocity += transform.up * s;
            }
            else
            {
                float t = (90 - Mathf.Abs(GetRawDriftAngle()));
                float s = -Mathf.Sign(GetRawDriftAngle()) * straighteningStrength * t;
                _rb.angularVelocity += transform.up * s;
            }
        }

        // Body
        _rb.centerOfMass = centerOfMass.localPosition;  // Doing each frame allows it to be changed in inspector
        _rb.AddForce(-GetDragForce() * _rb.velocity.normalized);

        // If rear wheels on ground
        if (WheelsGrounded())
        {
            // Engine
            _rb.AddForce(GetDriveDirection() * GetDriveForce());

            // Steering
            _rb.angularVelocity += -transform.up * GetSteeringAngularAcceleration() * Time.fixedDeltaTime;
        }
    }

    /// Point the drive wheels at angle
    /// angle relative to car direction
    /// angle = 0 means wheels point forward
    /// Does it smoothly
    void PointDriveWheelsAt(float targetAngle)
    {
        foreach (Transform wheel in steeringWheels)
        {
            float currentAngle = wheel.localEulerAngles.y;
            float change = ((((targetAngle - currentAngle) % 360) + 540) % 360) - 180;
            float newAngle = currentAngle + change * Time.deltaTime * maxVisualSteeringSpeed;
            wheel.localEulerAngles = new Vector3(0, newAngle, 0);
        }
    }

    /// Are the drive wheels grounded
    /// Can the car accelerate?
    public bool WheelsGrounded()
    {
        return Physics.OverlapBox(groundTrigger.position, groundTrigger.localScale / 2, Quaternion.identity, wheelCollidables).Length > 0;
    }

    /// How fast do we spin car?
    float GetSteeringAngularAcceleration()
    {
        return GetSteering() * maxAngularAcceleration * Mathf.PI / 180;
    }

    /// How much should we be turning?
    /// Between -1 and 1
    float GetSteering()
    {
        return Mathf.Clamp(TouchInput.centeredScreenPosition.x / screenUse, -1, 1);
    }

    /// What way car pointing
    /// Is normalized
    Vector3 GetDriveDirection()
    {
        return _rb.transform.forward.normalized;
    }

    /// How many beans will the car push itself with
    /// in newtown
    float GetDriveForce()
    {
        return driveForce * throttle;
    }

    /// Magnitude of drag
    /// velocity squared times drag coefficient
    /// Uses overall velocity, doesn't care about what direction car pointing
    float GetDragForce()
    {
        return Mathf.Pow(_rb.velocity.magnitude, 2) * drag;
    }
}
