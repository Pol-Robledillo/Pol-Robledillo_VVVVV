using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{
    public static Character character;
    public bool gravityChanged = false;
    private float gravity = 9.8f;
    private bool falling = false;
    public Vector2 spawn = new Vector2(0, 0);
    private bool isDead = false;
    private float boxWidth = 0.6f;
    private float boxHeight = 1f;
    public bool advanced = true;
    public AudioClip changeGravitySound;
    public AudioClip walkSound;

    void Awake()
    {
        if (character == null)
        {
            character = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (character != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
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
        bool paused = GameObject.Find("GameManager").GetComponent<GameManager>().isPaused;
        bool gameEnded = GameObject.Find("GameManager").GetComponent<GameManager>().gameFinished;
        if (horizontal != 0 && !falling && !paused && !gameEnded)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().PlayOneShot(walkSound);
            }
            StartAnimation();
        }
        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal * 10f, GetComponent<Rigidbody2D>().velocity.y);
        MirrorCharacterHorizontal();
        if (!falling)
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
            GetComponent<AudioSource>().PlayOneShot(changeGravitySound);
            gravityChanged = !gravityChanged;
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            Console.WriteLine("Gravity changed");
        }
    }
    private void CheckGrounded()
    {
        falling = true;
        Vector2 direction = gravityChanged ? Vector2.up : Vector2.down;
        Vector2 boxCenter = (Vector2)transform.position + direction * 0.1f;
        Vector2 boxSize = new Vector2(boxWidth, boxHeight);
        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0, direction, 0.5f);
        if (hit.collider != null)
        {
            falling = false;
        }
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
        if (Character.character != null)
        {
            if (gravityChanged)
            {
                gravityChanged = false;
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
            transform.position = spawn;
            isDead = false;
        }
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
