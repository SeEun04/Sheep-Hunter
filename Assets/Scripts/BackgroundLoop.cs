using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public float width;
    private BoxCollider2D backgroundCollider;

    private void Awake()
    {
        backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x;
    }

    private void Update()
    {
        if (transform.position.x <= -width)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 move = new Vector2(width * 3f, 0);
        transform.position = (Vector2) transform.position + move;
    }
}
