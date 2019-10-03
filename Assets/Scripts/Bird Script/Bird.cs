using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{

    public BirdState birdState { set; get; }

    private TrailRenderer lineRenderer;
    private Rigidbody2D myBody;
    private CircleCollider2D myCollider;
    private AudioSource audioSource;

    void Awake()
    {
        InitializeVariables();
    }

    void FixedUpdate()
    {
        if (birdState == BirdState.Thrown && myBody.velocity.sqrMagnitude <= GameVariables.MinVelocity)
        {
            StartCoroutine(DestroyAfterDelay(2f));
        }
    }

    void InitializeVariables()
    {
        lineRenderer = GetComponent<TrailRenderer>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();

        lineRenderer.enabled = false;
        //lineRenderer.sortingLayerName = "Foreground";

        myBody.isKinematic = true;
        myCollider.radius = GameVariables.BirdColliderRadiusBig;

        birdState = BirdState.BeforeThrown;
    }

    public void OnThrow()
    {
        audioSource.Play();
        lineRenderer.enabled = true;
        myBody.isKinematic = false;
        myCollider.radius = GameVariables.BirdColliderRadiusNormal;
        birdState = BirdState.Thrown;
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

} // Bird

