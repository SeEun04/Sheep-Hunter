using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    public float speed;
    public float jumpForce;

    private bool isDead = false;
    private float curTime;
    public float coolTime;
    public Transform pos;
    public Vector2 boxSize;

    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private AudioSource playerAudio;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Dead and Stop
        if (isDead)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");

        playerRigidbody.velocity = new Vector2(horizontalInput * speed, playerRigidbody.velocity.y);

        // Jump
        if (Input.GetButtonDown("Jump") && !animator.GetBool("isJumping"))
        {
            playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
            playerAudio.clip = audioJump;
            playerAudio.Play();
        }

        // Direction Change
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Animation
        if (Mathf.Abs(playerRigidbody.velocity.x) < 0.3) 
            animator.SetBool("isWalking", false);
        else
            animator.SetBool("isWalking", true);

        // Attack
        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Entity")
                    {
                        collider.GetComponent<SheepMove>().TakeDamage(1);
                        GameManager.Instance.IncreaseSheepCount();
                    }
                }
                animator.SetTrigger("Attack");
                playerAudio.clip = audioAttack;
                playerAudio.Play();
                curTime = coolTime;

            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    // Die
    private void Die()
    {
        animator.SetTrigger("Die");
        playerAudio.clip = audioDie;
        playerAudio.Play();

        playerRigidbody.velocity = Vector2.zero;
        isDead = true;

        GameManager.Instance.OnPlayerDead();
    }

    private void FixedUpdate()
    {
        // Platform
        if (playerRigidbody.velocity.y < 0)
        {
            Debug.DrawRay(playerRigidbody.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D RayCastHit = Physics2D.Raycast(playerRigidbody.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (RayCastHit.collider != null)
            {
                if (RayCastHit.distance < 1.0f)
                    animator.SetBool("isJumping", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            playerAudio.clip = audioFinish;
            playerAudio.Play();
            Time.timeScale = 0;
            SceneManager.LoadScene("Ending");
        }
    }
}
