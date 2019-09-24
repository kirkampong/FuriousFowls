using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private AudioSource audioSource;

    public float health = 70f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D target)
    {

        if (target.gameObject.GetComponent<Rigidbody2D>() == null)
            return;

        float damage = target.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10f;

        if (damage > 10)
            audioSource.Play();

        health -= damage;

        if (health <= 0)
            Destroy(gameObject);

    }
}
