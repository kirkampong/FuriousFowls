using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    [HideInInspector]
    public Vector3 startingPosition;

    private float minCameraX = 0f, maxCameraX = 14f;

    [HideInInspector]
    public bool isFollowing;

    [HideInInspector]
    public Transform birdToFollow;

    void Awake()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (isFollowing)
        {
            if (birdToFollow != null)
            {
                var birdPosition = birdToFollow.position;
                float x = Mathf.Clamp(birdPosition.x, minCameraX, maxCameraX);
                transform.position = new Vector3(x, startingPosition.y, startingPosition.z);
            }
            else
            {
                isFollowing = false;
            }
        }
    }

} // CameraFollow