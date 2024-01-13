using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;

    void Update()
    {
        // Zoom In/Out
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(zoomInput);

        // Move Camera
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        MoveCamera(horizontalInput, verticalInput);

        // Rotate Camera
        float rotateInput = Input.GetAxis("Rotation");
        RotateCamera(rotateInput);
    }

    void ZoomCamera(float zoomInput)
    {
        float newZoom = Mathf.Clamp(transform.position.y - zoomInput * zoomSpeed, 3f, 10f);
        transform.position = new Vector3(transform.position.x, newZoom, transform.position.z);
    }

    void MoveCamera(float horizontalInput, float verticalInput)
    {
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    void RotateCamera(float rotateInput)
    {
        transform.Rotate(Vector3.up, rotateInput * rotationSpeed);
    }
}