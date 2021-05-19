using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_SkidMarks : MonoBehaviour
{
    public TrailRenderer[] tyreMarks;
    public TrailRenderer[] frontTyreMarks;
    public ParticleSystem[] tyreSmoke;
    public Car_PlayerMovement car;
    public float width = 0.2f;
    public float minSmokeAlpha = 0.0f;
    public float maxSmokeAlpha = 0.6f;

    void FixedUpdate() {
        Do();
    }

    private void Do() {
        foreach (TrailRenderer t in tyreMarks) {
            t.startWidth = width;
            t.endWidth = width;
            t.emitting = car.IsDrifting();
        }
        foreach(ParticleSystem ps in tyreSmoke) {
            if (car.IsDrifting()) {
                ps.Play();
                Material mat = ps.gameObject.GetComponent<ParticleSystemRenderer>().material;
                Color color = mat.color;
                float a = Mathf.Max(Mathf.Min(1, (car.GetDriftAngle() / 90)), 0);
                a = a * (maxSmokeAlpha - minSmokeAlpha) + minSmokeAlpha;
                color.a = a;
                mat.color = color;
            } else {
                ps.Stop();
            }
        }
        foreach (TrailRenderer t in frontTyreMarks) {
            t.startWidth = width;
            t.endWidth = width;
            t.emitting = car.IsFrontWheelDrift();
        }
    }
}
