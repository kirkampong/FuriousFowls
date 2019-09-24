using UnityEngine;
using System.Collections;

public class Pig : MonoBehaviour
{

    private AudioSource audioSource;

    public float health = 150f;
    public Sprite spriteShownWhenHurt;
    private float changeSpriteHealth;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        changeSpriteHealth = health - 30f;
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.GetComponent<Rigidbody2D>() == null)
            return;

        if (target.gameObject.tag == "Bird")
        {
            audioSource.Play();
            Destroy(gameObject);
        }
        else
        {
            float damage = target.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10f;

            if (damage >= 10)
                audioSource.Play();

            health -= damage;

            if (health < changeSpriteHealth)
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteShownWhenHurt;

            if (health <= 0)
                Destroy(gameObject);

        }
    }

} // Pig