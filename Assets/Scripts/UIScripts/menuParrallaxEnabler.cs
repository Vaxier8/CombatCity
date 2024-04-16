using UnityEngine;

public class menuParrallaxEnabler : MonoBehaviour
{
    public float speed = 5.0f; // Speed at which the camera moves horizontally

    void Update()
    {
        // Move the camera horizontally by changing its X position
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
