using UnityEngine;

public class IconMovement : MonoBehaviour
{
    public float minHeight = 0f; // The minimum height of the object
    public float maxHeight = 3f; // The maximum height of the object
    public float upDownSpeed = 1f; // The speed at which the object moves up and down
    public bool enableUpDownMovement = true; // Enable/disable the up and down movement

    public float rotationSpeed = 50f; // The speed at which the object rotates
    public bool enableRotation = true; // Enable/disable the rotation

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (enableUpDownMovement)
        {
            // Calculate the new position using a sine wave to create an up and down motion
            float newY = startPosition.y + Mathf.Sin(Time.time * upDownSpeed) * (maxHeight - minHeight) / 2f;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        if (enableRotation)
        {
            // Rotate the object around its up axis
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
