using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 5f;
    private Vector2 spawn = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        spawn = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Patrol());
    }

    private IEnumerator Patrol()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
        if (transform.position.x >= spawn.x + 3)
        {
            speed = -5f;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (transform.position.x < spawn.x - 3)
        {
            speed = 5f;
            transform.localScale = new Vector3(1, 1, 1);
        }
        yield return null;
    }
}
