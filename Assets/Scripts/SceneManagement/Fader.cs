using UnityEngine;
using System.Collections;
using System;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveFade;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        IEnumerator FadeOutIn(float time)
        {
            yield return FadeOut(time);
            yield return FadeIn(time);
        }

        public Coroutine FadeIn(float time) => Fade(0, time);

        public Coroutine FadeOut(float time) => Fade(1, time);

        public Coroutine Fade(float target, float time)
        {
            if (currentActiveFade != null) StopCoroutine(currentActiveFade);
            currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }

        public void FadeOutImmediate()
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
        }
    }
}
