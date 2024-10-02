using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private static Character character;
    private bool gravityChanged = false;
    private float gravity = 9.8f;
    private bool falling = false;

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

    }

    private void Update()
    {
        ApplyGravity();
        Movement();
        CheckGrounded();
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
        ChangeGravity();
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
        falling = !((hitLeft.collider != null && hitRight.collider != null) && (hitLeft.collider.gameObject.layer == LayerMask.NameToLayer("Ground") && hitRight.collider.gameObject.layer == LayerMask.NameToLayer("Ground")));
        GetComponent<Animator>().SetBool("IsFalling", falling);
        Debug.Log(falling ? "Falling Colliding: " + hitLeft.collider : "Grounded Colliding: " + hitLeft.collider);
    }
}
