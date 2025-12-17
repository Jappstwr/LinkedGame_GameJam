using System;
using UnityEngine;

public class FadeInScript : MonoBehaviour
{
    CanvasGroup canvas;
    float alpha = 0;
    float timeElapsed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas == null) return;

        if (timeElapsed > 0.005 && timeElapsed != 1)
        {
            alpha += (float)0.002;
            canvas.alpha = alpha;
            timeElapsed = 0;
        }

        timeElapsed += Time.deltaTime;
    }
}
