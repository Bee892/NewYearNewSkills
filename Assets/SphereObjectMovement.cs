using UnityEngine;

public class SphereObjectMovement : MonoBehaviour
{
    public GameObject[] objectPrefab;
    public float sphereRadius = 5f;

    public void CreateMovingObject(Vector3 startPoint, Vector3 endPoint, int index, Transform parent)
    {
        // Instantiate the object at the starting point
        GameObject movingObject = Instantiate(objectPrefab[index], startPoint, Quaternion.identity,parent );
        movingObject.transform.parent = parent;
        // Set the initial position on the sphere surface
        movingObject.transform.position = startPoint.normalized * sphereRadius;

        // Set the rotation to align with the normal of the sphere and the direction of movement
        Vector3 direction = (endPoint - startPoint).normalized;
        movingObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, startPoint.normalized) * Quaternion.LookRotation(direction);

        // Set the sphere as the parent of the moving object
        movingObject.transform.SetParent(transform);

        // Add a component to control movement
        SphereObjectMover mover = movingObject.AddComponent<SphereObjectMover>();
        mover.Initialize(startPoint, endPoint, sphereRadius);
    }
}

public class SphereObjectMover : MonoBehaviour
{
    private Vector3 startPoint;
    private Vector3 endPoint;
    private float sphereRadius;
    private float t = 0f;

    public void Initialize(Vector3 startPoint, Vector3 endPoint, float sphereRadius)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.sphereRadius = sphereRadius;
    }

    private void Update()
    {
        // Move the object along the sphere surface between start and end points
        t += Time.deltaTime * 0.5f; // Adjust the speed as needed
        Vector3 sphericalPosition = Vector3.Slerp(startPoint.normalized, endPoint.normalized, t);
        transform.localPosition = sphericalPosition * sphereRadius;

        // Rotate the object to align the x-axis with the direction of movement
        Vector3 direction = (endPoint - startPoint).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction, startPoint.normalized);
        transform.localRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, 0f);

        // Check if the object has reached the end point
        if (t >= 1.0f)
        {
            Destroy(gameObject); // Optionally destroy the object when it reaches the end point
        }
    }
}