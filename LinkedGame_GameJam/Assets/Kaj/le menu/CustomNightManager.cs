using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CustomNightManager : MonoBehaviour
{
    //[Header("UI")]
    //public Transform dropdownContainer; // Parent object that holds the dropdowns (for layout)
    //public TMP_Dropdown dropdownTemplate; // Template dropdown you can duplicate

    //// AI levels corresponding to dropdown options
    //private readonly int[] aiValues = { 0, 5, 10, 20 };

    //private List<Animatronics> animatronicsList = new List<Animatronics>();
    //private Dictionary<Animatronics, TMP_Dropdown> dropdownMap = new Dictionary<Animatronics, TMP_Dropdown>();

    //void Start()
    //{
    //    // Find all animatronics in the scene
    //    Animatronics[] allAnims = Object.FindObjectsByType<Animatronics>(
    //         FindObjectsInactive.Include, // include inactive
    //         FindObjectsSortMode.None     // unsorted, faster
    //    );

    //    foreach (var anim in allAnims)
    //    {
    //        if (!animatronicsList.Contains(anim))
    //        {
    //            animatronicsList.Add(anim);
    //            CreateDropdownForAnim(anim);
    //        }
    //    }
    //}

    //void CreateDropdownForAnim(Animatronics anim)
    //{
    //    TMP_Dropdown dropdown;
    //    if (dropdownTemplate != null)
    //    {
    //        // Instantiate a copy of the template
    //        TMP_Dropdown newDropdown = Instantiate(dropdownTemplate, dropdownContainer);
    //        newDropdown.gameObject.SetActive(true);
    //        dropdown = newDropdown;
    //    }
    //    else
    //    {
    //        Debug.LogError("Dropdown template not set!");
    //        return;
    //    }

    //    // Set dropdown options
    //    dropdown.ClearOptions();
    //    List<string> options = new List<string>();
    //    foreach (int value in aiValues)
    //        options.Add($"AI {value}");
    //    dropdown.AddOptions(options);

    //    // Set initial value based on animatronic's startingMinute
    //    dropdown.value = GetDropdownIndex(anim.startingMinute);

    //    // Add listener
    //    dropdown.onValueChanged.AddListener((int val) =>
    //    {
    //        anim.startingMinute = aiValues[val];
    //        Debug.Log($"{anim.name} AI set to {aiValues[val]}");
    //    });

    //    // Map it for reference
    //    dropdownMap[anim] = dropdown;
    //}

    //int GetDropdownIndex(int minute)
    //{
    //    for (int i = 0; i < aiValues.Length; i++)
    //    {
    //        if (aiValues[i] == minute) return i;
    //    }
    //    return 0;
    //}

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