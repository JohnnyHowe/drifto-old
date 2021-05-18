using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_SkidMarks : MonoBehaviour
{
    public TrailRenderer[] tyreMarks;
    public Car_PlayerMovement car;
    public float width = 0.2f;

    void FixedUpdate() {
        SetEmitter(car.IsDrifting());
    }

    private void SetEmitter(bool state) {
        foreach (TrailRenderer t in tyreMarks) {
            t.startWidth = width;
            t.endWidth = width;
            t.emitting = state;
        }
    }
}
