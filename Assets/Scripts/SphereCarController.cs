using UnityEngine;

public class SphereCarController : MonoBehaviour
{
    [Header("Move Stuff")]
    public float MoveForward = 10;
    public float MoveBack = 5;
    private float Move;
    private float MoveMax;
    public float MoveConstant;

    [Header("Steering Stuff")]
    public float MaxSteeringSpeed = 1f;

    [Header("Top Values")]
    public float MaxSpeed;
    private float Steering;

    [Header("Braking")]
    public float BrakeTorque = 30000f;
    public float DecelerationForce = 100f;

    [Header("Curent speed")]
    public float speed = 0.0f;

    [Header("Car stuff physical")]
    public float downforce = 1.0f;
    public Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        speed = transform.InverseTransformDirection(rb.velocity).z * 3.6f;
        float HalfVertical = speed <= 0.5 ? Input.GetAxis("Vertical") : 2f;
        Steering = Input.GetAxis("Horizontal") * MaxSteeringSpeed * Time.deltaTime * HalfVertical;

        Move = Input.GetAxis("Vertical");
        Move *= Move > 0 ? MoveForward : MoveBack;
        MoveMax = speed <= MaxSpeed ? Move : 0;

        //MoveConstant = speed <= 0 ? MoveMax : MoveConstant ;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, Steering, 0f));

        transform.position = rb.transform.position;
    }
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * MoveMax);

        //rb.AddForce(transform.forward * MoveConstant);
        




    }
}
