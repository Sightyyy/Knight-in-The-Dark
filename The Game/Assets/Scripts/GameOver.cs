using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("Transition Settings")]
    public Image transitionImage;
    public float transitionTime = 1f;

    AudioCollection audioCollection;

    private void Awake()
    {
        audioCollection = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioCollection>();
    }

    public void Retry(string sceneName)
    {
        Time.timeScale = 1f;
        StartCoroutine(ReloadSceneFresh(sceneName));
    }

    public void ReturnToMainMenu(string sceneName)
    {
        audioCollection.PlaySFX(audioCollection.vo10);
        StartCoroutine(TransitionToScene(sceneName));
    }

    private IEnumerator ReloadSceneFresh(string sceneName)
    {
        yield return StartCoroutine(FadeToBlack());
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        yield return StartCoroutine(FadeToBlack());
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeToBlack()
    {
        if (transitionImage == null)
        {
            Debug.LogError("Transition Image not assigned!");
            yield break;
        }

        transitionImage.gameObject.SetActive(true);

        for (float t = 0.0f; t < transitionTime; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0, 1, t / transitionTime);
            SetImageAlpha(alpha);
            yield return null;
        }

        SetImageAlpha(1);
    }

    private IEnumerator FadeFromBlack()
    {
        if (transitionImage == null)
        {
            Debug.LogError("Transition Image not assigned!");
            yield break;
        }

        for (float t = 0.0f; t < transitionTime; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1, 0, t / transitionTime);
            SetImageAlpha(alpha);
            yield return null;
        }

        SetImageAlpha(0);
        transitionImage.gameObject.SetActive(false);
    }

    private void SetImageAlpha(float alpha)
    {
        if (transitionImage != null)
        {
            Color color = transitionImage.color;
            color.a = alpha;
            transitionImage.color = color;
        }
    }
}
