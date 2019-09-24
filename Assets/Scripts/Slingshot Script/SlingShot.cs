using UnityEngine;
using System.Collections;

public class SlingShot : MonoBehaviour
{

    private Vector3 slingShootMiddleVector;

    [HideInInspector]
    public SlingshotState slingShootState;

    public Transform leftSlingShootOrigin, rightSlingShootOrigin;

    public LineRenderer slingShootLineRenderer1, slingShootLineRenderer2, trajectoryLineRenderer;

    [HideInInspector]
    public GameObject birdToThrow;

    public Transform birdWaitPosition;

    public float throwSpeed;

    [HideInInspector]
    public float timeSinceThrown;

    public delegate void BirdThrown();
    public event BirdThrown birdThrown;

    void Awake()
    {
        slingShootLineRenderer1.sortingLayerName = "Foreground";
        slingShootLineRenderer2.sortingLayerName = "Foreground";
        trajectoryLineRenderer.sortingLayerName = "Foreground";

        slingShootState = SlingshotState.Idle;
        slingShootLineRenderer1.SetPosition(0, leftSlingShootOrigin.position);
        slingShootLineRenderer2.SetPosition(0, rightSlingShootOrigin.position);

        slingShootMiddleVector = new Vector3((leftSlingShootOrigin.position.x + rightSlingShootOrigin.position.x) / 2,
                                              (leftSlingShootOrigin.position.y + rightSlingShootOrigin.position.y) / 2, 0);
    }

    void Update()
    {

        switch (slingShootState)
        {

            case SlingshotState.Idle:

                InitializeBird();
                DisplaySlingshootLineRenderers();

                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    if (birdToThrow.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location))
                    {
                        slingShootState = SlingshotState.UserPulling;
                    }
                }
                break;

            case SlingshotState.UserPulling:

                DisplaySlingshootLineRenderers();

                if (Input.GetMouseButton(0))
                {

                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    location.z = 0f;

                    if (Vector3.Distance(location, slingShootMiddleVector) > 1.5f) //1.5f varies how far you can pull sling
                    {
                        var maxPosition = (location - slingShootMiddleVector).normalized * 1.5f + slingShootMiddleVector;
                        birdToThrow.transform.position = maxPosition;
                    }
                    else
                    {
                        birdToThrow.transform.position = location;
                    }
                    var distance = Vector3.Distance(slingShootMiddleVector, birdToThrow.transform.position);
                    DisplayTrajectoryLineRenderer(distance);
                }
                else
                {
                    SetTrajectoryLineRendererActive(true);
                    timeSinceThrown = Time.time;

                    float distance = Vector3.Distance(slingShootMiddleVector, birdToThrow.transform.position);
                    if (distance > 1)
                    {
                        SetSlingshotLinerenderersActive(false);
                        slingShootState = SlingshotState.BirdFlying;
                        ThrowBird(distance);
                    }
                    else
                    {
                        birdToThrow.transform.positionTo(distance / 10, birdWaitPosition.position);
                        InitializeBird();
                    }
                }

                break;

        }

    }

    private void InitializeBird()
    {
        birdToThrow.transform.position = birdWaitPosition.position;
        slingShootState = SlingshotState.Idle;
        SetSlingshotLinerenderersActive(true);
    }

    void SetSlingshotLinerenderersActive(bool active)
    {
        slingShootLineRenderer1.enabled = active;
        slingShootLineRenderer2.enabled = active;
    }

    void DisplaySlingshootLineRenderers()
    {
        slingShootLineRenderer1.SetPosition(1, birdToThrow.transform.position);
        slingShootLineRenderer2.SetPosition(1, birdToThrow.transform.position);
    }

    void SetTrajectoryLineRendererActive(bool active)
    {
        trajectoryLineRenderer.enabled = active;
    }

    void DisplayTrajectoryLineRenderer(float distance)
    {
        SetTrajectoryLineRendererActive(true);

        Vector3 v2 = slingShootMiddleVector - birdToThrow.transform.position;
        int segmentCount = 15;

        Vector2[] segments = new Vector2[segmentCount];

        segments[0] = birdToThrow.transform.position;

        Vector2 segVelocity = new Vector2(v2.x, v2.y) * throwSpeed * distance;

        for (int i = 1; i < segmentCount; i++)
        {
            float time = i * Time.fixedDeltaTime * 5f;
            segments[i] = segments[0] + segVelocity * time + 0.5f * Physics2D.gravity * Mathf.Pow(time, 2);
        }

        trajectoryLineRenderer.SetVertexCount(segmentCount);
        for (int i = 0; i < segmentCount; i++)
            trajectoryLineRenderer.SetPosition(i, segments[i]);

    }

    private void ThrowBird(float distance)
    {
        Vector3 velocity = slingShootMiddleVector - birdToThrow.transform.position;

        birdToThrow.GetComponent<Bird>().OnThrow();

        birdToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * throwSpeed * distance;

        if (birdThrown != null)
            birdThrown();

    }

} // SlingShot


