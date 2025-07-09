using System.Collections;
using UnityEngine;

public class GameObstacleBehaviour : MonoBehaviour
{
    private GameObstacleRowManager poolManager;
    private float returnThresholdZ = 0f;
    private bool canReturnToPool = false;

    private bool isAnimating = false;

    void Update()
    {
        if (canReturnToPool && transform.position.z > returnThresholdZ)
        {
            poolManager.ReturnToPool(gameObject);
        }
    }

    public void Init(GameObstacleRowManager manager, float zThreshold)
    {
        Debug.Log($"🔧 Ініціалізація GameObstacleBehaviour: {gameObject.name}, поріг Z: {zThreshold}");
        poolManager = manager;
        returnThresholdZ = zThreshold;
        canReturnToPool = true;
    }

    // 1️⃣ Зникнення (scale 1 → 1.1 → 0)
    public void PlayDisappearAnimation()
    {
        if (!isAnimating)
            StartCoroutine(DisappearCoroutine());
    }

    private IEnumerator DisappearCoroutine()
    {
        isAnimating = true;

        Vector3 originalScale = transform.localScale;
        Vector3 upScale = originalScale * 1.1f;
        Vector3 zeroScale = Vector3.zero;
        float duration = 0.15f;

        float t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, upScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = upScale;

        t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(upScale, zeroScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = zeroScale;

        isAnimating = false;
    }

    // 2️⃣ Поява (scale 0 → 1.1 → 1)
    public void PlayAppearAnimation()
    {
        if (!isAnimating)
            StartCoroutine(AppearCoroutine());
    }

    private IEnumerator AppearCoroutine()
    {
        isAnimating = true;

        Vector3 finalScale = Vector3.one;
        Vector3 upScale = finalScale * 1.1f;
        Vector3 zeroScale = Vector3.zero;
        float duration = 0.15f;

        transform.localScale = zeroScale;

        float t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(zeroScale, upScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = upScale;

        t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(upScale, finalScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = finalScale;

        isAnimating = false;
    }
}
