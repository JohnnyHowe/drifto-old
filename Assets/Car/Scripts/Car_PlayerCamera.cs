using UnityEngine;
using System.Collections;

public class Car_PlayerCamera : MonoBehaviour
{
    public float distance = 6.0f;
    public float height = 4.0f;
    public float damping = 5.0f;
    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;
    public AnimationCurve fovVelocityCurve = AnimationCurve.Linear(0, 60, 100, 80);
    public Rigidbody carRigidbody;
    public float carAngleEffectOnFOV = 0.1f;

    void FixedUpdate()
    {
        Vector3 wantedPosition;
        if (followBehind)
            wantedPosition = transform.TransformPoint(0, height, -distance);
        else
            wantedPosition = transform.TransformPoint(0, height, distance);

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, wantedPosition, Time.deltaTime * damping);

        if (smoothRotation)
        {
            Quaternion wantedRotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, transform.up);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
        }
        else Camera.main.transform.LookAt(transform, transform.up);

        // Car angle
        float angle = Vector3.Angle(carRigidbody.velocity.normalized, carRigidbody.transform.forward);
        float m = Mathf.Abs(Mathf.Pow(angle / 90, 2)) * Mathf.Sign(angle);
        float carAngleFOVChange = carAngleEffectOnFOV * m * 90;
    }
}