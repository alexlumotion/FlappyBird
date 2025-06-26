using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    [Tooltip("Скільки очок додати при активації")]
    public int scoreAmount = 1;

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
            TriggerScore(); // Викликаємо додавання очок
        }
    }

    // Цей метод можна викликати вручну або з OnTriggerEnter/іншої логіки
    public void TriggerScore()
    {
        if (gameLogic != null)
        {
            gameLogic.AddScore(scoreAmount);
            Debug.Log($"✅ Додано {scoreAmount} очок з об'єкта: {gameObject.name}");
        }
        else
        {
            Debug.LogWarning("⚠️ GameLogicManager не знайдено!");
        }
    }
}
