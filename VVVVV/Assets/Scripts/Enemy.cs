using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    private Vector2 spawn = new Vector2(0, 0);
    public int patrolDistance;

    // Start is called before the first frame update
    void Start()
    {
        spawn = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Patrol());
    }

    private IEnumerator Patrol()
    {
        rb.velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
        if (transform.position.x >= spawn.x + patrolDistance)
        {
            speed = -5f;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (transform.position.x < spawn.x - patrolDistance)
        {
            speed = 5f;
            transform.localScale = new Vector3(1, 1, 1);
        }
        yield return null;
    }
}
