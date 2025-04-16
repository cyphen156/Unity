using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public void Move(Vector2 direction)
    {
        transform.Translate(direction * Time.deltaTime);
    }
}
