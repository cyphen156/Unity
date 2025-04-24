using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction)
    {
        transform.Translate(direction * Time.deltaTime);
    }
    public void Jump(Vector2 jumpDirection, float jumpForce)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpDirection.y * jumpForce);
    }

    public void Warp(Transform targetPoint)
    {
        transform.position = targetPoint.position;
    }
}
