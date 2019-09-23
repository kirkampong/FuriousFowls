using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public CameraFollow cameraFollow;

    int currentBirdIndex;

    public SlingShot slingShot;

    [HideInInspector]
    public static GameState gameState;

    private List<GameObject> bricks;
    private List<GameObject> birds;
    private List<GameObject> pigs;

    public AudioSource[] audioSource;

    void Awake()
    {
        gameState = GameState.Start;
        slingShot.enabled = false;

        slingShot.slingShootLineRenderer1.enabled = false;
        slingShot.slingShootLineRenderer2.enabled = false;

        bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Brick"));
        birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird"));
        pigs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pig"));
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
                   (BricksBirdsPigsStoppedMoving() || Time.time - slingShot.timeSinceThrown > 5f))
                {
                    slingShot.enabled = false;

                    slingShot.slingShootLineRenderer1.enabled = false;
                    slingShot.slingShootLineRenderer2.enabled = false;

                    AnimateCameraToStartPosition();
                    gameState = GameState.BirdMovingToSlingshot;
                }

                break;

            case GameState.Won:
                audioSource[0].Play();
                StartCoroutine(Delay(3f));
                if (Input.GetMouseButtonDown(0))
                {
                    Application.LoadLevel("LevelMenu");
                }
                break;

            case GameState.Lost:
                audioSource[1].Play();
                StartCoroutine(Delay(3f));
                if (Input.GetMouseButtonDown(0))
                {
                    Application.LoadLevel("LevelMenu");
                }
                break;
        }

    }

    System.Collections.IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
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

    bool BricksBirdsPigsStoppedMoving()
    {

        foreach (var item in bricks.Union(birds).Union(pigs))
        {
            if (item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > GameVariables.MinVelocity)
            {
                return false;
            }
        }
        return true;
    }

    private bool AllPigsAreDestroyed()
    {
        return pigs.All(x => x == null);
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
                if (AllPigsAreDestroyed())
                {
                    gameState = GameState.Won;
                }
                else if (currentBirdIndex == birds.Count - 1)
                {
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

} // GameManager



