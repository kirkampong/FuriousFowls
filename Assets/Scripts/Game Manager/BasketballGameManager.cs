using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BasketballGameManager : MonoBehaviour
{

    public CameraFollow cameraFollow;
    public SlingShot slingShot;
    private List<GameObject> birds;
    [HideInInspector]
    public static GameState gameState;
    int currentBirdIndex;
    public AudioSource[] audioSource;

    void Awake()
    {
        gameState = GameState.Start;
        slingShot.enabled = false;

        slingShot.slingShootLineRenderer1.enabled = false;
        slingShot.slingShootLineRenderer2.enabled = false;

        birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird"));
        audioSource = GetComponentsInParent<AudioSource>();
    }

    void OnEnable()
    {
        slingShot.birdThrown += SlingShotBirdThrown;
    }

    void OnDisable()
    {
        slingShot.birdThrown -= SlingShotBirdThrown;
    }

    void Update()
    {

        switch (gameState)
        {

            case GameState.Start:
                if (Input.GetMouseButtonUp(0))
                {
                    AnimateBirdToSlingshot();
                }
                break;

            case GameState.Playing:

                if (slingShot.slingShootState == SlingshotState.BirdFlying &&
                   (BirdsStoppedMoving() || Time.time - slingShot.timeSinceThrown > 5f))
                {
                    slingShot.enabled = false;

                    slingShot.slingShootLineRenderer1.enabled = false;
                    slingShot.slingShootLineRenderer2.enabled = false;

                    AnimateCameraToStartPosition();
                    gameState = GameState.BirdMovingToSlingshot;
                }

                break;

            case GameState.Won:
                if (Input.GetMouseButtonDown(0))
                {
                    Application.LoadLevel("LevelMenu");
                }
                break;

            case GameState.Lost:
                if (Input.GetMouseButtonDown(0))
                {
                    Application.LoadLevel("LevelMenu");
                }
                break;
        }

    }

    bool BirdsStoppedMoving()
    {

        foreach (var item in birds)
        {
            if (item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > GameVariables.MinVelocity)
            {
                return false;
            }
        }
        return true;
    }

    void AnimateBirdToSlingshot()
    {
        gameState = GameState.BirdMovingToSlingshot;
        birds[currentBirdIndex].transform.positionTo(Vector2.Distance(birds[currentBirdIndex].transform.position / 10,
                                                                      slingShot.birdWaitPosition.position) / 10,
                                                     slingShot.birdWaitPosition.position).
            setOnCompleteHandler((x) => {
                x.complete();
                x.destroy();

                gameState = GameState.Playing;
                slingShot.enabled = true;

                slingShot.slingShootLineRenderer1.enabled = true;
                slingShot.slingShootLineRenderer2.enabled = true;

                slingShot.birdToThrow = birds[currentBirdIndex];
            });
    }



    private bool AllBasketsMade()
    {
        return (Score.basket1Made && Score.basket2Made && Score.basket3Made);
    }

    private void AnimateCameraToStartPosition()
    {
        float duration = Vector2.Distance(Camera.main.transform.position, cameraFollow.startingPosition) / 10f;
        if (duration == 0.0f)
            duration = 0.1f;

        Camera.main.transform.positionTo(duration,
                                         cameraFollow.startingPosition).
            setOnCompleteHandler((x) => {
                cameraFollow.isFollowing = false;
                if (AllBasketsMade())
                {
                    audioSource[0].Play();
                    gameState = GameState.Won;
                }
                else if (currentBirdIndex == birds.Count - 1)
                {
                    audioSource[1].Play();
                    gameState = GameState.Lost;
                }
                else
                {
                    slingShot.slingShootState = SlingshotState.Idle;
                    currentBirdIndex++;
                    AnimateBirdToSlingshot();
                }
            });
    }

    private void SlingShotBirdThrown()
    {
        cameraFollow.birdToFollow = birds[currentBirdIndex].transform;
        cameraFollow.isFollowing = true;
    }

}