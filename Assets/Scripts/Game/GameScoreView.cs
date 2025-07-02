using UnityEngine;
using TMPro;

public class GameScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text multiplierText;

    private Vector3 originalScoreScale;
    private Vector3 originalMultiplierScale;

    private Coroutine scoreAnimCoroutine;
    private Coroutine multiplierAnimCoroutine;

    void Start()
    {
        if (scoreText != null)
            originalScoreScale = scoreText.transform.localScale;

        if (multiplierText != null)
            originalMultiplierScale = multiplierText.transform.localScale;

        if (GameScoreManager.Instance != null)
        {
            GameScoreManager.Instance.OnScoreChanged += UpdateScore;
            GameScoreManager.Instance.OnComboMultiplierChanged += UpdateMultiplier;
        }
    }

    private void UpdateScore(int newScore)
    {
        if (scoreText == null) return;

        scoreText.text = newScore.ToString();

        // Запускаємо анімацію
        if (scoreAnimCoroutine != null)
            StopCoroutine(scoreAnimCoroutine);
        scoreAnimCoroutine = StartCoroutine(AnimateScale(scoreText.transform, originalScoreScale, 1.2f));
    }

    private void UpdateMultiplier(int multiplier)
    {
        if (multiplierText == null) return;

        multiplierText.gameObject.SetActive(multiplier > 1);
        multiplierText.text = $"x{multiplier}";

        if (multiplier > 1)
        {
            if (multiplierAnimCoroutine != null)
                StopCoroutine(multiplierAnimCoroutine);
            multiplierAnimCoroutine = StartCoroutine(AnimateScale(multiplierText.transform, originalMultiplierScale, 1.3f));
        }
    }

    private System.Collections.IEnumerator AnimateScale(Transform target, Vector3 original, float maxScale)
    {
        float time = 0f;
        float duration = 0.1f;
        Vector3 targetScale = original * maxScale;

        // Scale up
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            target.localScale = Vector3.Lerp(original, targetScale, t);
            yield return null;
        }

        // Scale down
        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            target.localScale = Vector3.Lerp(targetScale, original, t);
            yield return null;
        }

        target.localScale = original;
    }
}
