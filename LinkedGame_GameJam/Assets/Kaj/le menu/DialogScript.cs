using System;
using Assets.Kaj.services;
using TMPro;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    public TMP_InputField InputField;

    public void Submit()
    {
        LeaderboardService.SetUsername(InputField.text);
    }
}
