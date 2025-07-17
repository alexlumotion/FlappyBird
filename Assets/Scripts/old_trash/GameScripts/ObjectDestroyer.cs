using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public float LifeTime = 30f;
    private bool isDeleted = false;
    void Start()
    {
        //Invoke("DestroyObject", LifeTime);
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.Playing && !isDeleted)
        {
            isDeleted = true;
            Invoke("DestroyObject", LifeTime);
        }
    }

    void DestroyObject()
    {
        if (GameManager.Instance.GameState != GameState.Dead)
            Destroy(gameObject);
    }


}