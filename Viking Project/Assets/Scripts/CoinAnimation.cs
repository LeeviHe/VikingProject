using UnityEngine;
using System.Collections;

public class CoinAnimation : MonoBehaviour
{
    public float floatSpeed = 1f; // Speed at which the coin floats upwards
    public float rotationSpeed = 100f; // Speed at which the coin rotates
    public float durationBeforeDestroy = 3f; // Duration before the coin is destroyed

    // Start is called before the first frame update
    void Start()
    {
        // Start the floating animation
        StartCoroutine(FloatAndRotate());
    }

    // Coroutine for the floating animation
    public IEnumerator FloatAndRotate()
    {
        float timer = 0f;

        while (timer < durationBeforeDestroy)
        {
            // Move the coin upwards
            transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
            // Rotate the coin
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Increment timer
            timer += Time.deltaTime;

            yield return null;
        }

        // Destroy the coin object after the specified duration
        Destroy(gameObject);
    }
}
