using UnityEngine;
using System.Collections;

public class ParallaxScroll : MonoBehaviour
{

    public float parallaxFactor;

    private Vector3 previousCameraPosition;

    void Start()
    {
        previousCameraPosition = Camera.main.transform.position;
    }

    void Update()
    {
        Vector3 delta = Camera.main.transform.position - previousCameraPosition;
        delta.y = 0f;
        delta.z = 0f;
        transform.position += delta / parallaxFactor;

        previousCameraPosition = Camera.main.transform.position;
    }
}
