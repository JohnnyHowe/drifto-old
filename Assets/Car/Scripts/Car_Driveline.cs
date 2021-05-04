using Unity.Collections;
using UnityEngine;

namespace Car
{
    public class Car_Driveline : MonoBehaviour
    {
        [Header("Body")]
        public float mass = 1000;
        public float drag = 1f;


        [Header("Engine")]
        public float maxDriveForce = 1f;
        public float throttle = 0;  // Between 0 and 1
        public float readKMS;   // Readonly

        Rigidbody _rb;

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.mass = mass;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _rb.AddForce(GetDriveDirection() * GetDriveForce());
            _rb.AddForce(-GetDragForce() * _rb.velocity.normalized);
            readKMS = GetDriveVelocity().magnitude * 3.6f;
        }

        /// <summary>
        /// What direction is the car facing (not steering)
        /// </summary>
        Vector3 GetDriveDirection()
        {
            return transform.forward.normalized;
        }

        /// <summary>
        /// How fast is car in direction of forward
        /// </summary>
        Vector3 GetDriveVelocity() {
            return Vector3.Project(_rb.velocity, transform.forward);
        }

        /// <summary>
        /// How much force is the car applying to itself
        /// Takes into account slip ratio, drive wheel radius, etc
        /// in newtons
        /// </summary>
        float GetDriveForce()
        {
            return throttle * maxDriveForce;
        }

        float GetDragForce() {
            return Mathf.Pow(_rb.velocity.magnitude, 2) * drag;
        }
    }
}