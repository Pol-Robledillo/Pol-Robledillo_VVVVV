using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{
    public static Character character;
    private bool gravityChanged = false;
    private float gravity = 9.8f;
    private bool falling = false;
    private Vector2 spawn = new Vector2(0, 0);
    private bool isDead = false;

    void Awake()
    {
        if (Character.character != null && Character.character != this)
        {
            Destroy(gameObject);
        }
        Character.character = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        spawn = transform.position;
    }

    private void Update()
    {
        ApplyGravity();
        CheckGrounded();
        if (!isDead)
        {
            Movement();
        }
    }

    private void ApplyGravity()
    {
        float force = gravityChanged ? gravity : -gravity;
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, force);
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal * 10f, GetComponent<Rigidbody2D>().velocity.y);
        //transform.Translate(5f * Time.deltaTime * new Vector3(horizontal, 0.0f, vertical));
        MirrorCharacterHorizontal();
        StartAnimation();
        if (falling)
        {
            ChangeGravity();
        }
    }

    private void MirrorCharacterHorizontal()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, 1);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, 1);
        }
    }

    private void StartAnimation()
    {
        if (!falling)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                GetComponent<Animator>().SetBool("IsRunning", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("IsRunning", false);
            }
        }
    }

    private void ChangeGravity()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gravityChanged = !gravityChanged;
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            Console.WriteLine("Gravity changed");
        }
    }
    private void CheckGrounded()
    {
        RaycastHit2D hitLeft;
        RaycastHit2D hitRight;
        if (gravityChanged)
        {
            hitLeft = Physics2D.Raycast(transform.position - new Vector3(0.3f, 0, 0), Vector2.down, -1f);
            hitRight = Physics2D.Raycast(transform.position + new Vector3(0.3f, 0, 0), Vector2.down, -1f);
        }
        else
        {
            hitLeft = Physics2D.Raycast(transform.position - new Vector3(0.3f, 0, 0), Vector2.down, 1f);
            hitRight = Physics2D.Raycast(transform.position + new Vector3(0.3f, 0, 0), Vector2.down, 1f);
        }
        falling = !((hitLeft.collider != null || hitRight.collider != null) && ((hitLeft.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || hitRight.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) || (hitLeft.collider.gameObject.layer == LayerMask.NameToLayer("Shooter") || hitRight.collider.gameObject.layer == LayerMask.NameToLayer("Shooter"))));
        GetComponent<Animator>().SetBool("IsFalling", falling);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("Death");
        }
    }
    public void Respawn()
    {
        ChangeGravity();
        transform.position = spawn;
        isDead = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("Death");
        }
    }
}
