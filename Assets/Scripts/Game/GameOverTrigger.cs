using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    // Посилання на GameLogicManager (можна прив'язати в інспекторі або знайти автоматично)
    private GameLogicManager gameLogic;

    public bool isTrigger = false; // Чи використовувати тригер

    void Awake()
    {
        gameLogic = FindObjectOfType<GameLogicManager>();
    }

    void Update()
    {
        if (isTrigger)
        {
            isTrigger = false; // Вимикаємо тригер після першого виклику
            TriggerGameOver(); // Викликаємо гру закінчено
        }
    }

    // Цей метод можна викликати вручну або через події (OnCollision, OnTrigger)
    public void TriggerGameOver()
    {
        if (gameLogic != null)
        {
            Debug.Log("❌ Гра закінчена через об'єкт: " + gameObject.name);
            gameLogic.OnGameOverButtonPressed(); // централізовано
        }
        else
        {
            Debug.LogWarning("⚠️ GameLogicManager не знайдено!");
        }
    }
}
