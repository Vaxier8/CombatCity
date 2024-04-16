using UnityEngine;
using System.Collections;

public class ScaleUI : MonoBehaviour
{
    public float cycleTime = 2f; // Duration in seconds for one complete cycle (scale up and down)
    public Transform uiElement; // Reference to the UI element's transform
    public float maxScaleFactor = 1.3f; // Factor to scale up
    public float minScaleFactor = 0.7f; // Factor to scale down

    private void Start()
    {
        if (uiElement == null)
        {
            Debug.LogError("UI Element is not assigned!");
            return;
        }

        // Start the scaling coroutine
        StartCoroutine(ScaleElement());
    }

    private IEnumerator ScaleElement()
    {
        Vector3 originalScale = uiElement.localScale;
        Vector3 maxScale = originalScale * maxScaleFactor; // Scale up factor
        Vector3 minScale = originalScale * minScaleFactor; // Scale down factor

        while (true) // Loop indefinitely
        {
            // Scale up from original to max
            yield return StartCoroutine(ScaleOverTime(uiElement, maxScale, cycleTime / 2));
            // Scale down from max to min
            yield return StartCoroutine(ScaleOverTime(uiElement, minScale, cycleTime / 2));
        }
    }

    private IEnumerator ScaleOverTime(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 startScale = target.localScale;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            target.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            yield return null;
        }

        target.localScale = targetScale;
    }
}
