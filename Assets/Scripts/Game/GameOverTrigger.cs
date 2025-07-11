using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private GameLogicManager gameLogic;

    public bool isTrigger = false; // Чи використовувати тригер


    void Start()
    {
        gameLogic = GameLogicManager.Instance;
    }

    void Update()
    {
        if (isTrigger)
        {
            isTrigger = false;
            TriggerGameOver();
        }
    }

    public void TriggerGameOver()
    {
        if (gameLogic != null)
        {
            Debug.Log("❌ Гра закінчена через об'єкт: " + gameObject.name);
            gameLogic.OnGameOverButtonPressed();
        }
        else
        {
            Debug.LogWarning("⚠️ GameLogicManager не знайдено!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TriggerGameOver();
        }
    }
}
