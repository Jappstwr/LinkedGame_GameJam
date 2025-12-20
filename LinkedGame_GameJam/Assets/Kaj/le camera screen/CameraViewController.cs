using System;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CameraViewController : MonoBehaviour
{
    RectTransform image;
    float time = 0;
    int roation = 90;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0.06)
        {
            roation += 90;
            image.Rotate(new Vector3(0, 0, roation));
            time = 0;

            if (roation == 360) roation = 0;
        }
        time += Time.deltaTime;
    }
}
