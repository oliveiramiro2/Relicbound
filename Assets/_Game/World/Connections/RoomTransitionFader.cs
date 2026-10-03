using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoomTransitionFader : MonoBehaviour
{
  [SerializeField] private Image fadeImage;

  private Coroutine currentFade;

  private void Awake()
  {
    if (fadeImage == null)
      fadeImage = GetComponentInChildren<Image>();

    if (fadeImage == null)
    {
      Debug.LogError(
          "RoomTransitionFader requires a UI Image.");

      return;
    }

    SetAlpha(0f);
  }

  public void FadeOut(
      float duration)
  {
    StartFade(
        1f,
        duration);
  }

  public void FadeIn(
      float duration)
  {
    StartFade(
        0f,
        duration);
  }

  public IEnumerator FadeOutRoutine(
      float duration)
  {
    yield return FadeTo(
        1f,
        duration);
  }

  public IEnumerator FadeInRoutine(
      float duration)
  {
    yield return FadeTo(
        0f,
        duration);
  }

  private void StartFade(
      float targetAlpha,
      float duration)
  {
    if (currentFade != null)
      StopCoroutine(currentFade);

    currentFade = StartCoroutine(
        FadeTo(
            targetAlpha,
            duration));
  }

  private IEnumerator FadeTo(
      float targetAlpha,
      float duration)
  {
    if (fadeImage == null)
      yield break;

    float startAlpha =
        fadeImage.color.a;

    duration = Mathf.Max(
        0f,
        duration);

    if (duration <= 0f)
    {
      SetAlpha(targetAlpha);
      yield break;
    }

    float elapsed = 0f;

    while (elapsed < duration)
    {
      elapsed += Time.unscaledDeltaTime;

      float t =
          Mathf.Clamp01(
              elapsed / duration);

      SetAlpha(
          Mathf.Lerp(
              startAlpha,
              targetAlpha,
              t));

      yield return null;
    }

    SetAlpha(targetAlpha);
    currentFade = null;
  }

  private void SetAlpha(
      float alpha)
  {
    if (fadeImage == null)
      return;

    Color color =
        fadeImage.color;

    color.a = alpha;

    fadeImage.color = color;
  }
}