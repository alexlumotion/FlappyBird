using UnityEngine;

public class TreeMoveInstance : MonoBehaviour
{


    [Header("Speed")]
    public float speed = 1f;

    private bool isMoving = false;

    public Vector3 spawnPosition;


    void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    private void Move()
    {
        // Рухаємо по осі Z вперед
        transform.localPosition -= Vector3.forward * speed * Time.deltaTime;

        if (transform.localPosition.z < -6)
            transform.localPosition = new Vector3(-6, 0, 44); // Повертаємо назад на стартову позицію
    }
}
