using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class InteriorPanning : MonoBehaviour
{
    // Time when animation should start
    const double startingTime = 0.6;
    float panningAmount = 0;
    float timePassed = 0;
    bool showImage = false;
    PanningDirection panningDirection = PanningDirection.Left;
    RectTransform image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<RectTransform>();
        Debug.Log(image.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (timePassed > startingTime || showImage)
        {
            showImage = true;

            if (timePassed > 0.0002)
            {
                switch (panningAmount)
                {
                    case > 45:
                        panningDirection = PanningDirection.Left;
                        break;
                    case < -45:
                        panningDirection = PanningDirection.Right;
                        break;
                }

                switch (panningDirection)
                {
                    case PanningDirection.Left:
                        panningAmount -= (float)0.085;
                        break;
                    case PanningDirection.Right:
                        panningAmount += (float)0.085;
                        break;
                }

                image.position = new Vector3(1920 + panningAmount, 1080, 0);
                timePassed = 0;
            }
        }

        timePassed += Time.deltaTime;

    }

    private enum PanningDirection { Left, Right }
}
