using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    private void Start()
    {
        {
            rb = GetComponent<Rigidbody>();         
        }
    }

    public void Move(Vector2 direction)
    {
        transform.Translate(direction * Time.deltaTime);
    }

    public void Jump(Vector2 direction)
    {
        rb.AddForce(direction * Time.deltaTime);
    }
}
