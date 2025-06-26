using UnityEngine;

public class ChickenMoveController : MonoBehaviour
{
    public AudioClip soundEffect;
    public AudioSource audioSource;
    public Transform targetTransform;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveUp();
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
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
        if (targetTransform.position.y < 3.0f)
        {
            targetTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y + 1f, targetTransform.position.z);
        }
    }

    public void MoveDown()
    {
        if (targetTransform.position.y > 1.0f)
        {
            targetTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y - 1f, targetTransform.position.z);
        }
    }

    public void MoveLeft()
    {
        if (targetTransform.position.x >= -3.0f)
        {
            targetTransform.position = new Vector3(targetTransform.position.x - 1f, targetTransform.position.y, targetTransform.position.z);
        }
    }
    
    public void MoveRight()
    {
        if(targetTransform.position.x <= 3.0f)
        {
            targetTransform.position = new Vector3(targetTransform.position.x + 1f, targetTransform.position.y, targetTransform.position.z);
        }
    }

}
