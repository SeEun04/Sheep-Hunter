using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMove : MonoBehaviour
{
    private Rigidbody2D sheepRigidbody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;

    public int moving;
    public int HP;

    private void Start()
    {
        sheepRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        Invoke("Action", 5);
    }

    private void Action()
    {
        moving = Random.Range(-1, 2);

        if (moving != 0)
        {
            spriteRenderer.flipX = moving == 1;
        }

        float ActionTime = Random.Range(2f, 5f);
        Invoke("Action", ActionTime);
    }

    private void FixedUpdate()
    {
        sheepRigidbody.velocity = new Vector2(moving, sheepRigidbody.velocity.y);

        // Platform Check
        Vector2 frontVector = new Vector2(sheepRigidbody.position.x + moving*0.3f, sheepRigidbody.position.y);
        Debug.DrawRay(frontVector, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D RayCastHit = Physics2D.Raycast(frontVector, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (RayCastHit.collider == null)
            Turn();
    }

    private void Turn()
    {
        moving *= -1;
        spriteRenderer.flipX = moving == 1;
        CancelInvoke();
        Invoke("Action", 2);
    }

    public void TakeDamage(int damage)
    {
        HP = HP - damage;

        if (HP <= 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            spriteRenderer.flipY = true;
            capsuleCollider.enabled = false;
            sheepRigidbody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            Invoke("DeActive", 5);
        }
    }

    private void DeActive()
    {
        gameObject.SetActive(false);
    }
}
