using Assets.Kaj.services;
using TMPro;
using UnityEngine;

public class LeaderBoardScript : MonoBehaviour
{
    public TMP_Text UsernameRow;
    public TMP_Text TimeRow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LeaderboardService.GetRows(UsernameRow, TimeRow));
    }
}
