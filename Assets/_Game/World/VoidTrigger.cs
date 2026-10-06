using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<PlayerMovement>().ResetMovementState();
        Transform transform = collision.GetComponent<Transform>();
        transform.position = new Vector2(transform.position.x, 10);
    }
}
