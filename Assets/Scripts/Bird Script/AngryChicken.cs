using UnityEngine;
using System.Collections;

public class AngryChicken : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        //Transform.position = Vector3.up * Mathf.Cos(Time.time * frequency ) * intensity
        transform.position += Vector3.up * Mathf.Cos(Time.time * 1f) * 0.01f;
    }
}
