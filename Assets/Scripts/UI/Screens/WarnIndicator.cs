using System;
using System.Collections;
using UnityEngine;

public class WarnIndicator : MonoBehaviour
{
    public string warnKey = "key";

    [SerializeField]
    CanvasGroup canvasGroup;

    [SerializeField, Range(0, 1)]
    float offAlpha = 0.3f;

    [SerializeField, Range(0, 1)]
    float partialOffAlpha = 0.6f;

    [SerializeField, Range(0, 1)]
    float onAlpha = 1f;

    [SerializeField]
    float onTime = 0.3f;

    [SerializeField]
    float offTime = 0.3f;


    private Coroutine flashCoroutine;

    void Awake()
    {
        Hide();
    }

    public void Show()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        canvasGroup.alpha = onAlpha;
    }

    public void Hide()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        canvasGroup.alpha = offAlpha;
    }

    public void Flash()
    {
        if (flashCoroutine != null) return;

        flashCoroutine = StartCoroutine(FlashIndicator());
    }

    [Obsolete("Use Hide and Show for explicit control.")]
    public void StopFlash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        Hide();
    }

    public void FlashForTime(float duration, bool endOn = false)
    {
        if (flashCoroutine != null) return;

        flashCoroutine = StartCoroutine(FlashIndicator(duration, endOn));
    }

    private IEnumerator FlashIndicator()
    {
        while (true)
        {
            canvasGroup.alpha = onAlpha;
            yield return new WaitForSeconds(onTime);
            canvasGroup.alpha = partialOffAlpha;
            yield return new WaitForSeconds(offTime);
        }
    }

    private IEnumerator FlashIndicator(float duration, bool endOn = false)
    {
        yield return new WaitForEndOfFrame(); // sync to frame

        float durationTimer = 0;
        float localTimer = onTime;
        bool isOn = true;

        canvasGroup.alpha = onAlpha;

        while (true)
        {
            if (localTimer > 0) localTimer -= Time.deltaTime;
            else
            {
                if (isOn)
                {
                    isOn = false;
                    localTimer = offTime;
                    canvasGroup.alpha = partialOffAlpha;
                }
                else
                {
                    isOn = true;
                    localTimer = onTime;
                    canvasGroup.alpha = onAlpha;
                }
            }

            durationTimer += Time.deltaTime;

            if (durationTimer >= duration)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        if (endOn)
        {
            canvasGroup.alpha = onAlpha;
        }
        else
        {
            canvasGroup.alpha = offAlpha;
        }

        flashCoroutine = null;
    }
}
