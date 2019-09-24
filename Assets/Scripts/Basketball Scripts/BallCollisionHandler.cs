using UnityEngine;
using System.Collections;

/// <summary>
/// This script handles Collision of Basketball with other GameObjects
/// </summary>
/// 
public class BallCollisionHandler : MonoBehaviour
{
    public EntryPlane plane1, plane2, plane3, plane4, plane5, plane6;
    private bool enteredPlane1;
    private bool enteredPlane2;
    private bool enteredPlane3;
    private bool enteredPlane4;
    private bool enteredPlane5;
    private bool enteredPlane6;
    private Transform ballTransform;
    private Vector3 ballPosition;
    public AudioSource[] audioSource;//audio sources
    public static bool isCoolShot;//do you got cool shot
    public GameObject PlusFiftyEffectPrefab;
    public GameObject PlusTenEffectPrefab;

    //used for initialization
    void Start()
    {
        isCoolShot = true;//by default true
        audioSource = GetComponentsInParent<AudioSource>();//get audio sources

        if (plane1 == null) plane1 = GameObject.Find("Plane1").GetComponent<EntryPlane>();
        if (plane2 == null) plane2 = GameObject.Find("Plane2").GetComponent<EntryPlane>();
        if (plane3 == null) plane3 = GameObject.Find("Plane3").GetComponent<EntryPlane>();
        if (plane4 == null) plane4 = GameObject.Find("Plane4").GetComponent<EntryPlane>();
        if (plane5 == null) plane5 = GameObject.Find("Plane5").GetComponent<EntryPlane>();
        if (plane6 == null) plane6 = GameObject.Find("Plane6").GetComponent<EntryPlane>();
    }

    // update is called once per frame
    void Update()
    {
        ballPosition = transform.position;//get ball position

        //if the ball entered plane1
        if (ballPosition.x > plane1.pos0.x && ballPosition.x < plane1.pos1.x)
        {
            enteredPlane1 = true;
        }
        //if the ball entered plane2
        if (ballPosition.y < plane1.pos0.y && ballPosition.y > plane2.pos1.y)
        {
            enteredPlane2 = true;
        }

        //if the ball entered plane3
        if (ballPosition.x > plane3.pos0.x && ballPosition.x < plane3.pos1.x)
        {
            enteredPlane3 = true;
        }
        //if the ball entered plane4
        if (ballPosition.y < plane3.pos0.y && ballPosition.y > plane4.pos1.y)
        {
            enteredPlane4 = true;
        }
        //if the ball entered plane5
        if (ballPosition.x > plane5.pos0.x && ballPosition.x < plane5.pos1.x)
        {
            enteredPlane5 = true;
        }
        //if the ball entered plane6
        if (ballPosition.y < plane5.pos0.y && ballPosition.y > plane6.pos1.y)
        {
            enteredPlane6 = true;
        }

        //if the ball entered both planes, basket!
        if ((enteredPlane1 && enteredPlane2) ||
            (enteredPlane4 && enteredPlane3) ||
            (enteredPlane5 && enteredPlane6))
        {
            if (ballPosition.x > 15)
            {
                Score.basket3Made = true;
                Score.ApplyScore();
            }
            else if (ballPosition.x > 8)
            {
                Score.basket2Made = true;
                Score.ApplyScore();
            } else
            {
                Score.basket1Made = true;
                Score.ApplyScore();
            }

            //play the net SFX
            audioSource[5].Play();
        }
        enteredPlane1 = false;
        enteredPlane2 = false;
        enteredPlane3 = false;
        enteredPlane4 = false;
        enteredPlane5 = false;
        enteredPlane6 = false;
    }

    //handling Collision with other GameObjects
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Ground")
        {//if the ball hit the grounds
            if (GetComponent<Rigidbody2D>().velocity.magnitude > 1)
            {//if the rigidbody velocity is greater than 3
                audioSource[1].Play();//play ground hit SFX
            }
        }
        else if (col.collider.tag == "BounceCollider" && GetComponent<Rigidbody2D>().velocity.magnitude >= 5)
        {//if the ball hit the Bounce Colliders with the given rigidbody velocity
            audioSource[3].Play();//play basket rim SFX
        }
        else if (col.collider.tag == "BasketBoard" && GetComponent<Rigidbody2D>().velocity.magnitude >= 5)
        {//if the ball hit the Basket Board with the given rigidbody velocity
            audioSource[4].Play();//play board hit SFX
        }
        else if (col.collider.tag == "BasketPipe" && GetComponent<Rigidbody2D>().velocity.magnitude >= 4)
        {//if the ball hit the Basket Metal with the given rigidbody velocity
            audioSource[1].Play();
        }
    }
}

// RedBird Basketball AudioSource Array:
// [0] --> slinghot!
// [1] --> bounce!
// [2] --> yeah!
// [3] --> trim!
// [4] --> backboard!
// [5] --> swish!