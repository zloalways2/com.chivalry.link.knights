using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class BackgroundView : MonoBehaviour
{
    [SerializeField] private GameObject _loadingCanvas;
    public Slider _slider;

    void Start()
    {
        StartCoroutine(ChangeSliderValueOverTime(1f, 1.5f)); 
    }

    IEnumerator ChangeSliderValueOverTime(float targetValue, float duration)
    {
        float startValue = _slider.value;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            _slider.value = Mathf.Lerp(startValue, targetValue, timeElapsed / duration);

            yield return null;
        }

        _slider.value = targetValue; 


        CanvasGroup canvasGroup = _loadingCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            float startAlpha = canvasGroup.alpha;
            float targetAlpha = 0f;
            float alphaDuration = 1f;
            float alphaTimeElapsed = 0f;

            while (alphaTimeElapsed < alphaDuration)
            {
                alphaTimeElapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, alphaTimeElapsed / alphaDuration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
            _loadingCanvas.SetActive(false);
        }
    }
}
