using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Suspension : MonoBehaviour
{
    public Transform FLWheel;
    float FLLastDistance = 0;
    public Transform FRWheel;
    float FRLastDistance = 0;
    public Transform BLWheel;
    float BLLastDistance = 0;
    public Transform BRWheel;
    float BRLastDistance = 0;

    public float maxDistance = 0.5f;
    public float minDistance = -0.5f;
    public float springConstant = 1;
    public float dampingConstant = 0.1f;

    Rigidbody _rb;
    void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Physics.Raycast(FLWheelOrigin.position, -transform.up, wheelRadius);
        ApplyWheelSupport(FLWheel, FLLastDistance);
        ApplyWheelSupport(FRWheel, FRLastDistance);
        ApplyWheelSupport(BLWheel, BLLastDistance);
        ApplyWheelSupport(BRWheel, BRLastDistance);

        FLLastDistance = GetWheelDistanceFromRest(FLWheel);
        FRLastDistance = GetWheelDistanceFromRest(FRWheel);
        BLLastDistance = GetWheelDistanceFromRest(BLWheel);
        BRLastDistance = GetWheelDistanceFromRest(BRWheel);
    }

    void ApplyWheelSupport(Transform wheel, float lastDistance) {
        Vector3 supportForce = GetWheelSupportForce(wheel, lastDistance);
        _rb.AddForceAtPosition(supportForce, wheel.position);
    }

    Vector3 GetWheelSupportForce(Transform wheel, float lastDistance) {
        float currentDistance = GetWheelDistanceFromRest(wheel);
        float dampingForce = ((currentDistance - lastDistance) / Time.fixedDeltaTime) * dampingConstant;
        float magnitude = Mathf.Max(0, -currentDistance * springConstant - dampingForce);
        return magnitude * GetUpDir();
    }

    float GetWheelDistanceFromRest(Transform wheel) {
        RaycastHit hit;
        float rayLength = maxDistance - minDistance;
        Vector3 rayOrigin = wheel.position - GetUpDir() * minDistance;
        bool hasHit = Physics.Raycast(rayOrigin, -GetUpDir(), out hit, rayLength);
        float distanceFromRest = hasHit? hit.distance + minDistance: 0;
        return distanceFromRest;
    }

    Vector3 GetUpDir() {
        return transform.up;
    }
}
