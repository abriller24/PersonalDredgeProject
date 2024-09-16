using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField] private Transform boatMotor;
    [SerializeField] private float boatPower = 10f;
    [SerializeField] private float turnTorque = 1f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float drag = 0.1f;

    private Rigidbody rb;
    private Waves waveScript;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        waveScript = FindFirstObjectByType<Waves>();
    }

    private void FixedUpdate()
    {
        float turnInput = 0f;

        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            movement = Vector3.left;
        if (Input.GetKey(KeyCode.S))
            movement = Vector3.right;
        if (Input.GetKey(KeyCode.A)) turnInput = 1f;  // Turn left
        if (Input.GetKey(KeyCode.D)) turnInput = -1f; // Turn right

        ApplyMovement(movement);
        ApplyDrag();

        // Apply wave forces
        Vector3 boatPosition = transform.position;
        float waveHeight = waveScript.GetHeight(boatPosition);
        Debug.Log($"Boat Position: {boatPosition}, Wave Height: {waveHeight}");
        Vector3 waveForce = new Vector3(0, waveHeight, 0); // Adjust as needed
        rb.AddForce(waveForce);
        // Apply turning torque
        if (turnInput != 0f)
        {
            // Torque for turning left or right
            Vector3 turnTorqueVector = Vector3.up * turnInput * turnTorque;
            rb.AddTorque(turnTorqueVector, ForceMode.Acceleration);
        }

        Debug.Log($"Movement Direction: {movement}, Wave Force: {waveForce}, Linear Velocity: {rb.linearVelocity}");
    }

    private void ApplyMovement(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Vector3 force = direction * boatPower * Time.deltaTime;
            rb.AddForce(force, ForceMode.Acceleration);

            // Limit speed along X-axis only
            Vector3 velocity = rb.linearVelocity;
            if (velocity.x > maxSpeed)
                rb.linearVelocity = new Vector3(maxSpeed, velocity.y, velocity.z);
            else if (velocity.x < -maxSpeed)
                rb.linearVelocity = new Vector3(-maxSpeed, velocity.y, velocity.z);
        }
    }

    private void ApplyDrag()
    {
        rb.linearVelocity *= 1 - (drag * Time.deltaTime);
    }
}
