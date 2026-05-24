using UnityEngine;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFader : MonoBehaviour
{
    public static CanvasFader Instance;
    private CanvasGroup canvasGroup;

    [Header(" Fade Settings ")]
    [SerializeField] private float fadeDuration = 0.4f;  

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        FadeIn();
    }

    public void FadeIn(Action onComplete = null)
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 1f;  
        canvasGroup.blocksRaycasts = true;  

        LeanTween.alphaCanvas(canvasGroup, 0f, fadeDuration)
            .setEase(LeanTweenType.linear)
            .setOnComplete(() =>
            {
                canvasGroup.gameObject.SetActive(false);
                canvasGroup.blocksRaycasts = false;
                onComplete?.Invoke();
            });
    }

    public void FadeOut(Action onComplete = null)
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 0f; // Şeffaf başla
        canvasGroup.blocksRaycasts = true; // Ekran kararırken tıklamaları kilitle

        LeanTween.alphaCanvas(canvasGroup, 1f, fadeDuration)
            .setEase(LeanTweenType.linear)
            .setOnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }
}