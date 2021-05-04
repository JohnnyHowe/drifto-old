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
    public float drag = 1f;
    [Header("Engine")]
    public float driveForce = 1;
    [Header("Suspension and Steering")]
    public float activeVisualSteeringAngleEffect = 1;
    public float maxVisualSteeringSpeed = 1;
    public float maxAngularVelocity = 30;    // degrees per second
    public List<Transform> steeringWheels;
    public List<Transform> driveWheels;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Point wheels
        float passiveAngle = -Vector3.Angle(_rb.velocity.normalized, GetDriveDirection()) * Vector3.Cross(_rb.velocity.normalized, GetDriveDirection()).y;
        float activeAngle = -GetSteering() * activeVisualSteeringAngleEffect;
        float wheelAngle = passiveAngle + activeAngle;
        PointDriveWheelsAt(wheelAngle);
    }

    void FixedUpdate()
    {
        // Body
        _rb.centerOfMass = centerOfMass.localPosition;  // Doing each frame allows it to be changed in inspector
        _rb.AddForce(-GetDragForce() * _rb.velocity.normalized);

        // Engine
        _rb.AddForce(GetDriveDirection() * GetDriveForce());

        // Steering
        _rb.angularVelocity = -Vector3.up * GetSteeringAngularVelocity();
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
            // wheel.localEulerAngles = new Vector3(0, Mathf.Clamp(newAngle, -maxVisualSteeringAngle, maxVisualSteeringAngle), 0);
        }
    }

    float nfmod(float a,float b)
{
    return a - b * Mathf.Floor(a / b);
}

    /// How fast do we spin car?
    float GetSteeringAngularVelocity()
    {
        return GetSteering() * maxAngularVelocity * Mathf.PI / 180;
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
