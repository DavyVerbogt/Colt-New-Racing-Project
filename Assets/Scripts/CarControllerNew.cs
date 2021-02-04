using UnityEngine;

public class CarControllerNew : MonoBehaviour
{
    [Header("Motor Torque")]
    public AnimationCurve MotorTorqueCurve = new AnimationCurve(new Keyframe(0, 600), new Keyframe(180, 400), new Keyframe(400, 0));
    public float DiffGearing = 4.0f;

    [Header("Steering Stuff")]
    public AnimationCurve turnInputCurve = AnimationCurve.Linear(-1.0f, -1.0f, 1.0f, 1.0f);
    public float MaxSteeringAngle = 30f;
    public float MaxSteeringSpeed = 1f;
    [HideInInspector]
    public bool IsSteering = false;

    [Header("Top Values")]
    public float MaxRpm;
    public float MaxSpeed;
    public float MaxSpeedBack;

    [Header("Braking")]
    public float BrakeTorque = 30000f;
    public float DecelerationForce = 100f;

    [Header("Wheel look")]
    public GameObject WheelShape;

    [Header("Curent speed")]
    public float speed = 0.0f;

    [Header("Car stuff physical")]
    public float downforce = 1.0f;
    [HideInInspector]
    public WheelCollider[] Wheels;
    private Rigidbody rb;
    public Transform centerOfMass;

    [Header("Debug values")]
    public float CurrentTorque;
    public float CurrentSteer;
    public float CurrentBrake;
    public float CurrentRpm;



    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Wheels = GetComponentsInChildren<WheelCollider>();

        for (int i = 0; i < Wheels.Length; ++i)
        {
            var wheel = Wheels[i];

            // Create wheel shapes only when needed.
            if (WheelShape != null)
            {
                var ws = Instantiate(WheelShape);
                ws.transform.parent = wheel.transform;
            }
        }
        MaxSpeedBack = -MaxSpeedBack;
    }

    void FixedUpdate()
    {
        speed = transform.InverseTransformDirection(rb.velocity).z * 3.6f;

        if (rb != null && centerOfMass != null)
        {
            rb.centerOfMass = centerOfMass.localPosition;
        }

        rb.AddForce(-transform.up * speed * downforce);

        float Steer = turnInputCurve.Evaluate(Input.GetAxis("Horizontal")) * MaxSteeringSpeed * MaxSteeringAngle;

        bool HandBrake = Input.GetKey(KeyCode.Space);

        float Torque = Input.GetAxis("Vertical");

        foreach (WheelCollider wheel in Wheels)
        {
            Steering(Steer);
            Motor(Torque);
            Breaking(Torque, HandBrake);

            CurrentSteer = wheel.steerAngle;
            CurrentBrake = wheel.brakeTorque;
            CurrentRpm = wheel.rpm;

            WheelLocation();
        }
    }
    void Motor(float InputVertical)
    {
        InputVertical = Mathf.Clamp(InputVertical, -1, 1);

        foreach (WheelCollider wheel in Wheels)
        {
            if (wheel.transform.localPosition.z > 0)
            {
                if (Mathf.Abs(wheel.rpm) < MaxRpm)
                {
                    if (Input.GetAxis("Vertical") < 0 && speed < 0)
                    {
                        wheel.motorTorque = speed > MaxSpeedBack ? InputVertical * MotorTorqueCurve.Evaluate(speed) * DiffGearing  / 6f : 0;
                    }
                    else if (Input.GetAxis("Vertical") > 0)
                    {
                        wheel.motorTorque = speed < MaxSpeed ? InputVertical * MotorTorqueCurve.Evaluate(speed) * DiffGearing / 2f : 0;
                        CurrentTorque = wheel.motorTorque;
                    }
                    else
                    {
                        wheel.motorTorque = 0f;
                    }
                }
                else
                {
                    wheel.motorTorque = 0f;
                }

            }
        }
    }
    void Steering(float Steer)
    {
        foreach (WheelCollider wheel in Wheels)
        {
            if (wheel.transform.localPosition.z > 0)
            {
                wheel.steerAngle = Mathf.Lerp(wheel.steerAngle, Steer, MaxSteeringSpeed);
            }
        }
    }
    void Breaking(float BreakVert, bool HandBreak)
    {
        BreakVert = Mathf.Clamp(BreakVert, -1, 1);
        foreach (WheelCollider wheel in Wheels)
        {
            if (Mathf.Abs(wheel.rpm) > MaxRpm)
            {
                wheel.brakeTorque = Mathf.Abs(wheel.rpm);
            }
            if (HandBreak)
            {
                wheel.brakeTorque = BrakeTorque * ((speed > 0f) ? speed : 1f);

            }
            else if (BreakVert > 0 && speed < 0)
            {
                wheel.brakeTorque = BrakeTorque * ((speed > 0f) ? speed : 1f);

            }
            else if (BreakVert < 0 && speed > 0)
            {
                wheel.brakeTorque = BrakeTorque * ((speed > 0f) ? speed : 1f);
            }
            else if (BreakVert == 0)
            {
                wheel.brakeTorque = DecelerationForce;
            }
            else
            {
                wheel.brakeTorque = 0;
            }
        }
    }
    void WheelLocation()
    {
        foreach (WheelCollider wheel in Wheels)
        {
            if (WheelShape)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                Transform WheelTransform = wheel.transform.GetChild(0);

                if (wheel.name == "Wheel-F-L" || wheel.name == "Wheel-B-L")
                {
                    WheelTransform.rotation = q * Quaternion.Euler(0, 180, 90);
                    WheelTransform.position = p;
                }
                else
                {
                    WheelTransform.position = p;
                    WheelTransform.rotation = q * Quaternion.Euler(0, 0, 90);
                }

            }
        }
    }
}