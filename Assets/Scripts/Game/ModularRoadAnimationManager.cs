using UnityEngine;

public class ModularRoadAnimationManager : MonoBehaviour
{
    [Header("Objects to animate")]
    public Transform object1; // Стартова позиція X = +17
    public Transform object2; // Стартова позиція X = -19

    public Transform root;
    public Transform currentPiece;

    void Start()
    {
        currentPiece = object1;
    }

    private int lastStep = -1; // Оголоси це в класі, перед Update()

    void Update()
    {
        float currentAngle = root.localEulerAngles.x;

        // Нормалізуємо до 0–360
        currentAngle = currentAngle % 360f;

        // Округлюємо до найближчого кратного 36
        int step = Mathf.FloorToInt(currentAngle / 36f);

        // Пропускаємо перший раз
        if (lastStep == -1)
        {
            lastStep = step;
            return;
        }

        if (step != lastStep)
        {
            lastStep = step;
            int snappedAngle = step * 36;
            //Debug.Log($"🌀 Потрапили на новий кут: {snappedAngle}°");

            // Твоя логіка
            currentPiece.SetParent(null);
            currentPiece.localEulerAngles = new Vector3(-19f, 0f, -90f);
            currentPiece.SetParent(root);
            currentPiece = (currentPiece == object1) ? object2 : object1;
        }
    }
}