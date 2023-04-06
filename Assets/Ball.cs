using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{

    public Rigidbody2D rigidbody2D;
    public float speed;
    public Vector2 velocity;
    public int leftPlayerScore = 0;
    public int rightPlayerScore = 0;
    public TextMeshProUGUI leftPlayerText;
    public TextMeshProUGUI rightPlayerText;
    private bool hasTouchedTrigger = false; // new variable to track whether ball has touched trigger

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        ResetAndSetRandomVelocity();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && hasTouchedTrigger) // check if ball has touched trigger before allowing reset
        {
            ResetAndSetRandomVelocity();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rigidbody2D.velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
        velocity = rigidbody2D.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.x > 0)
        {
            leftPlayerScore += 1;
            leftPlayerText.text = leftPlayerScore.ToString();
            Debug.Log("Left player score: " + leftPlayerScore);
        }

        if (transform.position.x < 0)
        {
            rightPlayerScore = rightPlayerScore + 1;
            rightPlayerText.text = rightPlayerScore.ToString();
            Debug.Log("Right player score: " + rightPlayerScore);
        }
        ResetBall();
        hasTouchedTrigger = true; // set variable to true once ball touches trigger
    }

    private void ResetBall()
    {
        rigidbody2D.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        hasTouchedTrigger = false; // reset variable to false once ball is reset
    }

    private void ResetAndSetRandomVelocity()
    {
        ResetBall();
        rigidbody2D.velocity = GenerateRandomVector2Without0(true) * speed;
        velocity = rigidbody2D.velocity;
    }

    private Vector2 GenerateRandomVector2Without0(bool returnNormalized)
    {
        Vector2 newRandomVector = new Vector2();

        bool shouldXBeLessThanZero = Random.Range(0, 100) % 2 == 0;
        newRandomVector.x = shouldXBeLessThanZero ? Random.Range(-.8f, -.1f) : Random.Range(.1f, .8f);

        bool shouldYBeLessThanZero = Random.Range(0, 100) % 2 == 0;
        newRandomVector.y = shouldYBeLessThanZero ? Random.Range(-.8f, -.1f) : Random.Range(.1f, .8f);

        return returnNormalized ? newRandomVector.normalized : newRandomVector;
    }
}
