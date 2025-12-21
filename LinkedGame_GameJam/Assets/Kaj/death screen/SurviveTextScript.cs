using UnityEngine;
using TMPro;
using Assets.Kaj.services;

public class SurviveTextScript : MonoBehaviour
{
    public TMP_Text survivedText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int hours = HourLogicScript.Instance.HoursSurvived;
        StartCoroutine(LeaderboardService.SubmitTime(hours));
        survivedText.text = $"You survived {hours} hours";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
