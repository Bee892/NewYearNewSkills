using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        RotateObject();
    }

    void RotateObject()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the rotation angles based on input
        float rotationX = verticalInput * rotationSpeed * Time.deltaTime;
        float rotationY = horizontalInput * rotationSpeed * Time.deltaTime;

        // Apply rotation to the GameObject
        transform.Rotate(-rotationX, rotationY, 0f);
    }
}