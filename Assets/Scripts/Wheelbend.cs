using UnityEngine;

public class Wheelbend : MonoBehaviour
{

    public CarControllerNew CarControllerNewScript;
    public WheelCollider[] Wheels;
    public float RotateSpeed = 90;

    // Start is called before the first frame update
    void Start()
    {
        CarControllerNewScript = GetComponent<CarControllerNew>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Wheels = CarControllerNewScript.GetComponentsInChildren<WheelCollider>();


        foreach (WheelCollider wheel in Wheels)
        {
            Quaternion q;
            Vector3 p;
            wheel.GetWorldPose(out p, out q);

            Transform WheelBend = wheel.transform.GetChild(0);



            if (wheel.name == "Wheel-F-L" || wheel.name == "Wheel-B-L")
            {

                if (CarControllerNewScript.IsSteering)
                {
                    WheelBend.localRotation = q * Quaternion.AngleAxis(RotateSpeed, Vector3.forward);
                }
                else
                {
                    WheelBend.rotation = q * Quaternion.Euler(0, 180, 90);
                    WheelBend.position = p;
                }
            }
            else
            {

                if (CarControllerNewScript.IsSteering)
                {
                    WheelBend.localRotation = q * Quaternion.AngleAxis(RotateSpeed, Vector3.forward);
                }
                else
                {
                    WheelBend.position = p;
                    WheelBend.rotation = q * Quaternion.Euler(0, 0, 90);
                }
            }
        }

    }
}
