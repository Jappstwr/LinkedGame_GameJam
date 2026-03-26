using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CustomNightManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Dropdown nightDropdown;   // Normal animatronics
    public TMP_Dropdown ventDropdown;    // Vent animatronic

    // Dropdown options
    private readonly int[] aiValues = { 0, 5, 10, 15, 20 };

    void Start()
    {
        // Set dropdowns to match stored config
        nightDropdown.value = GetIndex(CustomNightConfig.StartingMinute);
        ventDropdown.value = GetIndex(CustomNightConfig.VentAI);

        // Listeners
        nightDropdown.onValueChanged.AddListener(OnNightDropdownChanged);
        ventDropdown.onValueChanged.AddListener(OnVentDropdownChanged);
    }

    void OnNightDropdownChanged(int value)
    {
        CustomNightConfig.StartingMinute = aiValues[value];
        Debug.Log($"[Custom Night] Stored starting minute = {CustomNightConfig.StartingMinute}");
    }

    void OnVentDropdownChanged(int value)
    {
        CustomNightConfig.VentAI = aiValues[value];
        Debug.Log($"[Custom Night] Stored vent AI = {CustomNightConfig.VentAI}");
    }

    int GetIndex(int aiValue)
    {
        for (int i = 0; i < aiValues.Length; i++)
        {
            if (aiValues[i] == aiValue)
                return i;
        }
        return 0;
    }

    //[Header("UI")]
    //public TMP_Dropdown nightDropdown;   // Normal animatronics
    //public TMP_Dropdown ventDropdown;    // Vent animatronic

    //// Dropdown options
    //private readonly int[] aiValues = { 0, 5, 10, 15, 20 };

    //void Start()
    //{
    //    // Default dropdowns
    //    nightDropdown.value = 0;
    //    ventDropdown.value = 0;

    //    // Listeners
    //    nightDropdown.onValueChanged.AddListener(OnNightDropdownChanged);
    //    ventDropdown.onValueChanged.AddListener(OnVentDropdownChanged);

    //    // Apply initial values
    //    ApplyNightFromDropdown();
    //    ApplyVentFromDropdown();
    //}

    //void OnNightDropdownChanged(int value)
    //{
    //    ApplyNightFromDropdown();
    //}

    //void OnVentDropdownChanged(int value)
    //{
    //    ApplyVentFromDropdown();
    //}

    //void ApplyNightFromDropdown()
    //{
    //    int index = nightDropdown.value;
    //    int aiMinute = aiValues[index];

    //    // Advance night time
    //    NightsDifficulty.SetStartingMinute(aiMinute);

    //    // Update animatronics in THIS scene (menu previews, if any)
    //    Animatronics[] allAnims = Object.FindObjectsByType<Animatronics>(
    //        FindObjectsInactive.Include,
    //        FindObjectsSortMode.None
    //    );

    //    foreach (var anim in allAnims)
    //    {
    //        anim.startingMinute = aiMinute;
    //    }

    //    Debug.Log($"Custom Night → Animatronic AI starts at minute {aiMinute}");
    //}

    //void ApplyVentFromDropdown()
    //{
    //    int index = ventDropdown.value;
    //    int aiLevel = aiValues[index];

    //    // Store vent difficulty globally
    //    VentDifficulty.StartingAI = aiLevel;
    //    VentDifficulty.StartingAiTimer = 60f;

    //    Debug.Log($"Custom Night → Vent AI set to {aiLevel}");
    //}
}