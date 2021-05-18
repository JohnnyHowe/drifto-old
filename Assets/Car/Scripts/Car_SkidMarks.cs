using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_SkidMarks : MonoBehaviour
{
    public TrailRenderer[] tyreMarks;
    public TrailRenderer[] frontTyreMarks;
    public Car_PlayerMovement car;
    public float width = 0.2f;

    void FixedUpdate() {
        Do();
    }

    private void Do() {
        foreach (TrailRenderer t in tyreMarks) {
            t.startWidth = width;
            t.endWidth = width;
            t.emitting = car.IsDrifting();
        }
        foreach (TrailRenderer t in frontTyreMarks) {
            t.startWidth = width;
            t.endWidth = width;
            t.emitting = car.IsFrontWheelDrift();
        }
    }
}
