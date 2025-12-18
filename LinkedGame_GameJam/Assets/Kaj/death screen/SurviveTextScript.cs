using UnityEngine;
using TMPro;

public class SurviveTextScript : MonoBehaviour
{
    public TMP_Text survivedText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int hours = HourLogicScript.Instance.HoursSurvived;
        survivedText.text = $"You survived {hours} hours";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
