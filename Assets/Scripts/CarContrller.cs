using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarContrller : MonoBehaviour
{

    public float MaxSteeringAngle = 30f;
    public float MaxTorque = 300f;
    public float BrakeTorque = 30000f;
    public GameObject wheelShape;

    public float CriticalSpeed = 5f;
    public int StepsBelow = 5;
    public int StepsAbove = 1;

    public WheelCollider[] Wheels;

    private Rigidbody rb;
    public Transform centerOfMass;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Wheels = GetComponentsInChildren<WheelCollider>();

        for (int i = 0; i < Wheels.Length; ++i)
        {
            var wheel = Wheels[i];

            // Create wheel shapes only when needed.
            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheel.transform;
            }
        }
    }

    void FixedUpdate()
    {
        Wheels[0].ConfigureVehicleSubsteps(CriticalSpeed, StepsBelow, StepsAbove);

        if (rb != null && centerOfMass != null)
        {
            rb.centerOfMass = centerOfMass.localPosition;
        }

        float Steering = MaxSteeringAngle * Input.GetAxis("Horizontal");
        float Torque = MaxTorque * Input.GetAxis("Vertical");

        float handBrake = Input.GetKey(KeyCode.Space) ? BrakeTorque : 0;

        foreach (WheelCollider wheel in Wheels)
        {
            if (wheel.transform.localPosition.z > 0)
                wheel.steerAngle = Steering;

            if (wheel.transform.localPosition.z < 0)
            {
                wheel.brakeTorque = handBrake;
                wheel.motorTorque = Torque;
            }



            if (wheelShape)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                Transform shapeTransform = wheel.transform.GetChild(0);

                if (wheel.name == "a0l" || wheel.name == "a1l" || wheel.name == "a2l")
                {
                    shapeTransform.rotation = q * Quaternion.Euler(0, 0, 0);
                    shapeTransform.position = p;
                }
                else
                {
                    shapeTransform.position = p;
                    shapeTransform.rotation = q;
                }
            }
        }
    }
}