using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private TextMeshProUGUI textDisplay;

    // Starting time
    double t = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

    const int startingHour = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textDisplay = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // god why do i need to do a null check
        // why cant i annotate as a nullable type i hate c#
        if (textDisplay != null)
        {
            // Debug.Log("pls fucking work");

            var seconds = Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds - t);

            var hours = (Math.Floor((double)(seconds / 60)) + startingHour) % 24;

            var minutes = (seconds % 60) < 10 ? $"0{seconds % 60}" : (seconds % 60).ToString();

            textDisplay.text = $"{(hours < 10 ? $"0{hours}" : hours)}:{minutes}";
        }
        else
        {
            // Debug.Log("it doesnt fucking work");
        }

    }
}
