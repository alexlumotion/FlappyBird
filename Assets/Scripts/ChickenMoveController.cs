using UnityEngine;

public class ChickenMoveController : MonoBehaviour
{
    public Transform targetTransform;
    public float moveSpeed = 10f;

    private Vector3 targetPosition;

    void Start()
    {
        // Початкова позиція
        targetPosition = targetTransform.position;
    }

    void Update()
    {
        HandleInput();

        // Плавне переміщення до цілі
        targetTransform.position = Vector3.MoveTowards(
            targetTransform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
    }

    public void MoveUp()
    {
        if (targetPosition.y < 3.0f)
        {
            targetPosition += Vector3.up;
        }
    }

    public void MoveDown()
    {
        if (targetPosition.y > 1.0f)
        {
            targetPosition += Vector3.down;
        }
    }

    public void MoveLeft()
    {
        if (targetPosition.x > -3.0f)
        {
            targetPosition += Vector3.left;
        }
    }

    public void MoveRight()
    {
        if (targetPosition.x < 3.0f)
        {
            targetPosition += Vector3.right;
        }
    }
}
