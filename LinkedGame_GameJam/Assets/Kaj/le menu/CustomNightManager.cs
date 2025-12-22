using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CustomNightManager : MonoBehaviour
{

    [Header("UI")]
    public TMP_Dropdown nightDropdown;    // Controls the night starting minute for normal animatronics
    public TMP_Dropdown ventDropdown;     // Separate dropdown for Vent

    public VentScript ventAnim;

    // Dropdown options corresponding to starting minutes / AI levels
    private readonly int[] aiValues = { 0, 5, 10, 15, 20 };

    void Start()
    {
        // Set default values
        nightDropdown.value = 0;
        ventDropdown.value = 0;

        // Add listeners
        nightDropdown.onValueChanged.AddListener(OnNightDropdownChanged);
        ventDropdown.onValueChanged.AddListener(OnVentDropdownChanged);

        // Apply initial values
        ApplyNightFromDropdown();
        ApplyVentFromDropdown();
    }

    // Called when the night dropdown changes
    void OnNightDropdownChanged(int value)
    {
        ApplyNightFromDropdown();
    }

    // Called when the Vent dropdown changes
    void OnVentDropdownChanged(int value)
    {
        ApplyVentFromDropdown();
    }

    void ApplyNightFromDropdown()
    {
        int index = nightDropdown.value;
        int aiMinute = aiValues[index];

        // Advance the night timer to match dropdown
        NightsDifficulty.SetStartingMinute(aiMinute);

        // Update animatronics startingMinute
        Animatronics[] allAnims = Object.FindObjectsByType<Animatronics>(
             FindObjectsInactive.Include,    // include inactive objects
             FindObjectsSortMode.None        // unsorted, faster
        );

        foreach (var anim in allAnims)
        {
            anim.startingMinute = aiMinute;
        }

        Debug.Log($"Custom Night AI set → starting minute: {aiMinute}");
    }

    void ApplyVentFromDropdown()
    {
        if (ventAnim == null) return;

        int index = ventDropdown.value;
        int aiLevel = aiValues[index];

        ventAnim.AiLevel = aiLevel;          // set starting AI
        ventAnim.AiTimerValue = 60f;         // reset AI progression timer
        Debug.Log($"Vent AI set → {aiLevel}");
    }
}