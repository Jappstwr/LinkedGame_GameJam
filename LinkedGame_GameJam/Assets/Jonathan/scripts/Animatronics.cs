using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Animatronics : MonoBehaviour
{
    public NightLogicScript NLS;
    public GameObject Jumpscare;
    public bool IsFredrik;

    [Header("Golden Fredrik")]
    public bool isGoldenFredrik;
    public float goldenAttackCooldown = 60f; //360
    public float goldenKillDelay = 7f;

    [Header("Golden Fredrik – Attack Control")]
    public float goldenCheckIntervalMin = 60f; //360
    public float goldenCheckIntervalMax = 120f; // 600
    [Range(0f, 3f)] public float goldenBaseAttackChance = 15f;
    public float goldenChanceIncreasePerMinute = 3f;
    private float goldenRetreatCooldown = 60f; //360
    private bool canGoldenAttack = true;

    //private bool goldenAttackScheduled = false;
    private bool goldenIsAttacking = false;

    [HideInInspector] public bool isFrozenAtDoor = false;
    [HideInInspector] public bool isAtDoor = false;

    [Header("Waypoints")]
    public RoomWaypoint[] waypoints;

    [Header("Movement Timing")]
    public float minMoveDelay = 5f;
    public float maxMoveDelay = 15f;

    [Header("Door Chance")]
    [Range(0f, 100f)] public float baseDoorChance = 10f;
    public float doorChanceIncrease = 5f;
    private float hallway1DoorChance;
    private float hallway2DoorChance;

    [Header("Ignore Camera")]
    [Range(0f, 100f)] public float baseIgnoreCameraChance = 0f;
    public float ignoreCameraIncreasePerMinute = 3f;

    [HideInInspector] public int currentRoom = 0;
    [HideInInspector] public bool isBeingWatched = false;

    private SpriteRenderer sr;
    private MainCameraScript cameraManager;

    public AudioClip stepSound;

    [Header("Custom AI")]
    public int startingMinute = 0;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cameraManager = Object.FindFirstObjectByType<MainCameraScript>();

        hallway1DoorChance = baseDoorChance;
        hallway2DoorChance = baseDoorChance;

        if (waypoints.Length > 0)
        {
            MoveToWaypoint(0);
        }

        if (isGoldenFredrik)
        {
            // Removed InvokeRepeating to prevent guaranteed attacks
            // InvokeRepeating(nameof(GoldenAttack), goldenAttackCooldown, goldenAttackCooldown);

            // Start chance-based attack schedule
            ScheduleGoldenCheck();
        }
        else
        {
            StartCoroutine(WaitForAIStartThenMove());
        }
    }
    private IEnumerator WaitForAIStartThenMove()
    {
        // Wait until night logic actually updates
        while (NightsDifficulty.CurrentMinute < Mathf.Max(startingMinute, 1))
        {
            yield return null;
        }

        ScheduleNextMove();
    }
    //private IEnumerator WaitForNightStartThenMove()
    //{

    //    while (NightsDifficulty.CurrentMinute < 1)
    //    {
    //        yield return null;
    //    }


    //    ScheduleNextMove();
    //}

    void ScheduleNextMove()
    {
        //if (NightsDifficulty.CurrentMinute == 0)
        //{
        //    Invoke(nameof(ScheduleNextMove), 60f);
        //    return;
        //}

        float difficulty = GetDifficultyMultiplier();
        float min = minMoveDelay / difficulty;
        float max = maxMoveDelay / difficulty;

        Invoke(nameof(MoveRandom), Random.Range(min, max));
    }

    void ScheduleGoldenCheck()
    {
        if (NightsDifficulty.CurrentMinute == 0)
        {
            Invoke(nameof(ScheduleGoldenCheck), 60f);
            return;
        }

        float delay = Random.Range(goldenCheckIntervalMin, goldenCheckIntervalMax);
        Invoke(nameof(GoldenAttackCheck), delay);
    }

    bool ShouldIgnoreCamera()
    {
        int minute = Mathf.Min(NightsDifficulty.CurrentMinute, 20);
        float ignoreChance = baseIgnoreCameraChance + (minute * ignoreCameraIncreasePerMinute);
        ignoreChance = Mathf.Clamp(ignoreChance, 0f, 50f); // never too extreme

        float roll = Random.Range(0f, 100f);
        Debug.Log($"{name} ignore camera roll: {roll} / {ignoreChance}");

        return roll < ignoreChance;
    }

    void MoveRandom()
    {
        if (waypoints.Length == 0 || isGoldenFredrik)
        {
            return;
        }

        // Only prevent movement if frozen at door
        if (isFrozenAtDoor)
        {
            return;
        }

        Debug.Log($"{name} attempting move. Room: {currentRoom}, isBeingWatched={isBeingWatched}");

        // Camera freeze check
        if (isBeingWatched && !ShouldIgnoreCamera())
        {
            Debug.Log($"{name} frozen by camera");
            ScheduleNextMove();
            return;
        }
        else if (isBeingWatched)
        {
            Debug.Log($"{name} IGNORED camera");
        }

        // Hallway / Door logic
        bool moved = false;
        if (currentRoom == 1)
        {
            moved = TryMoveToDoor(ref hallway1DoorChance);
        }
        else if (currentRoom == 2)
        {
            moved = TryMoveToDoor(ref hallway2DoorChance);
        }

        // Stage logic: more likely to leave as difficulty ramps up
        if (!moved && currentRoom == 0)
        {
            float difficulty = GetMinuteDifficultyMultiplier();
            if (Random.value < difficulty * 0.3f)
            {
                MoveToRandomNonDoorWaypointAnywhere();
                moved = true;
            }
        }

        // Normal random movement if no door/hallway movement
        if (!moved)
        {
            MoveToRandomNonDoorWaypointAnywhere();
        }

        ScheduleNextMove();
    }

    bool TryMoveToDoor(ref float doorChance)
    {
        float effectiveChance = doorChance * GetMinuteDifficultyMultiplier();

        // Stage → hallway bonus
        if (currentRoom == 0)
            effectiveChance *= 1.5f;

        float roll = Random.Range(0f, 100f);
        Debug.Log($"{name} door roll: {roll} / {effectiveChance}");

        if (roll <= effectiveChance)
        {
            RoomWaypoint door = GetDoorWaypointForCurrentRoom();
            if (door != null)
            {
                Debug.Log($"{name} SUCCESS → moving to DOOR");
                doorChance = baseDoorChance;
                MoveToWaypoint(System.Array.IndexOf(waypoints, door));
                return true;
            }
        }

        doorChance += doorChanceIncrease;
        return false;
    }

    RoomWaypoint GetDoorWaypointForCurrentRoom()
    {
        foreach (var wp in waypoints)
        {
            if (wp != null && wp.roomIndex == currentRoom && wp.isDoorWaypoint)
            {
                return wp;
            }
        }
        return null;
    }

    void MoveToRandomNonDoorWaypointAnywhere()
    {
        var valid = new List<int>();
        int currentIndex = GetCurrentWaypointIndex();

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] != null &&
                !waypoints[i].isDoorWaypoint &&
                i != currentIndex)
            {
                valid.Add(i);
            }
        }

        if (valid.Count == 0)
        {
            Debug.Log($"{name} has no other non-door waypoint to move to");
            return;
        }

        int choice = valid[Random.Range(0, valid.Count)];
        MoveToWaypoint(choice);
    }

    void MoveToWaypoint(int index)
    {
        // Safety check
        if (index < 0 || index >= waypoints.Length)
        {
            return;
        }

        RoomWaypoint wp = waypoints[index];
        if (wp == null)
        {
            return;
        }

        transform.position = wp.transform.position;
        currentRoom = wp.roomIndex;

        if (!string.IsNullOrEmpty(wp.roomLayer))
        {
            int layer = LayerMask.NameToLayer(wp.roomLayer);
            if (layer != -1)
            {
                gameObject.layer = layer;
            }
        }

        if (cameraManager != null)
        {
            cameraManager.RefreshAllAnimatronics();
        }

        if (wp.isDoorWaypoint)
        {
            isAtDoor = true;

            if (!isGoldenFredrik)
            {
                StartCoroutine(DoorCountdownUnified());
            }
        }
        else
        {
            isAtDoor = false;
        }
    }
    private IEnumerator DoorCountdownUnified()
    {
        if (NLS == null)
            yield break;

        float countdownTime = 10f; // normal animatronics
        float timer = 0f;

        isAtDoor = true;

        while (timer < countdownTime)
        {
            bool doorClosed = IsFredrik ? NLS.IsRightClosed : NLS.IsLeftClosed;

            if (doorClosed)
            {
                SoundEffectsScript.instance.PlaySoundEffect(stepSound, 1f);
                isAtDoor = false;
                MoveToRandomNonDoorWaypointAnywhere();
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        isAtDoor = false;
        TriggerJumpscare();
    }
    private IEnumerator GoldenDoorRoutine()
    {
        goldenIsAttacking = true;

        // Move to door
        RoomWaypoint door = GetAnyDoorWaypoint();
        if (door == null) yield break;

        transform.position = door.transform.position;
        currentRoom = door.roomIndex;
        isAtDoor = true;

        float timer = 0f;

        while (timer < goldenKillDelay)
        {
            bool doorClosed = NLS.IsRightClosed;

            if (doorClosed)
            {
                // Player blocked him → retreat
                SoundEffectsScript.instance.PlaySoundEffect(stepSound, 1f);
                ResetGoldenFredrik();
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // If door never closed → jumpscare
        TriggerJumpscare();
    }

    public void CloseDoorForAnimatronic()
    {
        isAtDoor = false;
    }

    void TriggerJumpscare()
    {
        NLS.Camera.SetActive(false);
        NLS.Computer.SetActive(false);
        NLS.Office.SetActive(true);
        Jumpscare.SetActive(true);

        NLS.Alive = false;
        SoundEffectsScript.instance.PlaySoundEffect(NLS.jumpscareSound, 1f);
    }

    int GetCurrentWaypointIndex()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] != null &&
                waypoints[i].roomIndex == currentRoom &&
                waypoints[i].transform.position == transform.position)
            {
                return i;
            }
        }
        return -1;
    }

    void GoldenAttack()
    {
        if (goldenIsAttacking || !NLS.Alive)
        {
            return;
        }


        Debug.Log("Golden Fredrik attacks!");

        // Start golden-specific door routine
        StartCoroutine(GoldenDoorRoutine());
    }

    void GoldenAttackCheck()
    {
        if (!NLS.Alive || !canGoldenAttack || goldenIsAttacking)
        {
            ScheduleGoldenCheck();
            return;
        }

        int minute = NightsDifficulty.CurrentMinute;

        // Calculate chance, min 0%, max 5%
        float chance = goldenBaseAttackChance + (minute * goldenChanceIncreasePerMinute);
        chance = Mathf.Clamp(chance, 0f, 5f);

        float roll = Random.Range(0f, 100f);
        Debug.Log($"Golden Fredrik roll: {roll} / {chance}");

        if (roll <= chance)
        {
            GoldenAttack(); // attack only if roll succeeds
        }

        // Schedule the next check with random interval
        ScheduleGoldenCheck();
    }

    //IEnumerator GoldenDoorCountdown()
    //{
    //    float timer = 0f;
    //    while (timer < goldenKillDelay)
    //    {
    //        if (NLS.IsRightClosed) // Golden Fredrik always right door
    //        {
    //            Debug.Log("Golden Fredrik blocked!");
    //            ResetGoldenFredrik();
    //            yield break;
    //        }
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //    TriggerJumpscare();
    //}

    RoomWaypoint GetAnyDoorWaypoint()
    {
        foreach (var wp in waypoints)
        {
            if (wp != null && wp.isDoorWaypoint)
            {
                return wp;
            }
        }
        return null;
    }

    void ResetGoldenFredrik()
    {
        goldenIsAttacking = false;
        isAtDoor = false;

        // Find a safe non-door waypoint to move him to
        RoomWaypoint safeWaypoint = null;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] != null && !waypoints[i].isDoorWaypoint)
            {
                safeWaypoint = waypoints[i];
                break; // pick the first valid one
            }
        }

        if (safeWaypoint != null)
        {
            transform.position = safeWaypoint.transform.position;
            currentRoom = safeWaypoint.roomIndex;
            isAtDoor = false;

            // Refresh camera so movement is visible immediately
            if (cameraManager != null)
            {
                cameraManager.RefreshAllAnimatronics();
            }

        }

        // Start normal movement, ignoring camera for first move
        StartCoroutine(ResumeNormalMovementAfterRetreat());
    }
    private IEnumerator ResumeNormalMovementAfterRetreat()
    {
        yield return null; // wait one frame to ensure camera refresh

        if (!goldenIsAttacking)
        {
            // Ignore camera check for the first movement
            MoveRandomIgnoringCamera();
            ScheduleNextMove();
        }
    }
    void MoveRandomIgnoringCamera()
    {
        if (waypoints.Length == 0 || isGoldenFredrik)
        {
            return;
        }
        if (isFrozenAtDoor)
        {
            return;
        }

        bool moved = false;

        if (currentRoom == 1)
        {
            moved = TryMoveToDoor(ref hallway1DoorChance);
        }

        else if (currentRoom == 2)
        {
            moved = TryMoveToDoor(ref hallway2DoorChance);
        }


        if (!moved && currentRoom == 0)
        {
            float difficulty = GetMinuteDifficultyMultiplier();
            if (Random.value < difficulty * 0.3f)
            {
                MoveToRandomNonDoorWaypointAnywhere();
                moved = true;
            }
        }

        if (!moved)
        {
            MoveToRandomNonDoorWaypointAnywhere();
        }

    }
    void SetGoldenReady()
    {
        goldenIsAttacking = false;
    }
    void StartNormalMovement()
    {
        if (!goldenIsAttacking)
        {
            MoveRandom();
            ScheduleNextMove();
        }
    }
    private IEnumerator GoldenRetreatCooldownRoutine()
    {
        canGoldenAttack = false;
        yield return new WaitForSeconds(goldenRetreatCooldown);
        canGoldenAttack = true;
    }
    float GetMinuteDifficultyMultiplier()
    {
        int minute = NightsDifficulty.CurrentMinute;
        return 1f + (minute * 0.15f);
    }

    float GetDifficultyMultiplier()
    {
        int minute = Mathf.Min(NightsDifficulty.CurrentMinute, 20);
        return 1f + (minute * 0.15f);
    }





    //// =========================
    //// References
    //// =========================
    //public NightLogicScript NLS;
    //public GameObject Jumpscare;
    //public bool IsFredrik;

    //private SpriteRenderer sr;
    //private MainCameraScript cameraManager;

    //// =========================
    //// Golden Fredrik
    //// =========================
    //[Header("Golden Fredrik")]
    //public bool isGoldenFredrik;
    //public float goldenKillDelay = 7f; // 7-second kill timer

    //[Header("Golden Fredrik – Attack Control")]
    //[Range(0f, 5f)] public float goldenBaseAttackChance = 0.1f; // 0.1% at 1 AM
    //public float goldenChanceIncreasePerMinute = 0.1f;            // 0.2% at 2 AM, etc.

    //private bool goldenIsAttacking = false;
    //private bool canGoldenAttack = true;

    //// =========================
    //// Movement State
    //// =========================
    //[HideInInspector] public bool isFrozenAtDoor = false;
    //[HideInInspector] public bool isAtDoor = false;
    //[HideInInspector] public int currentRoom = 0;
    //[HideInInspector] public bool isBeingWatched = false;

    //// =========================
    //// Waypoints
    //// =========================
    //[Header("Waypoints")]
    //public RoomWaypoint[] waypoints;

    //// =========================
    //// Movement Timing
    //// =========================
    //[Header("Movement Timing")]
    //public float minMoveDelay = 5f;
    //public float maxMoveDelay = 15f;

    //// =========================
    //// Door Logic
    //// =========================
    //[Header("Door Chance")]
    //[Range(0f, 100f)] public float baseDoorChance = 10f;
    //public float doorChanceIncrease = 5f;
    //private float hallway1DoorChance;
    //private float hallway2DoorChance;

    //// =========================
    //// Camera Ignore Logic
    //// =========================
    //[Header("Ignore Camera")]
    //[Range(0f, 100f)] public float baseIgnoreCameraChance = 0f;
    //public float ignoreCameraIncreasePerMinute = 3f;

    //// =========================
    //// Audio
    //// =========================
    //public AudioClip stepSound;

    //// =========================
    //// Custom AI
    //// =========================
    //[Header("Custom AI")]
    //public int startingMinute = 0;

    //// =========================
    //// Unity Events
    //// =========================
    //void Start()
    //{
    //    sr = GetComponent<SpriteRenderer>();
    //    cameraManager = Object.FindFirstObjectByType<MainCameraScript>();

    //    hallway1DoorChance = baseDoorChance;
    //    hallway2DoorChance = baseDoorChance;

    //    if (waypoints.Length > 0)
    //        MoveToWaypoint(0);

    //    if (isGoldenFredrik)
    //    {
    //        ScheduleGoldenCheck();
    //    }
    //    else
    //    {
    //        StartCoroutine(WaitForAIStartThenMove());
    //    }
    //}

    //// =========================
    //// Normal Animatronic Logic
    //// =========================
    //private IEnumerator WaitForAIStartThenMove()
    //{
    //    while (NightsDifficulty.CurrentMinute < Mathf.Max(startingMinute, 1))
    //        yield return null;

    //    ScheduleNextMove();
    //}

    //void ScheduleNextMove()
    //{
    //    float difficulty = GetDifficultyMultiplier();
    //    float min = minMoveDelay / difficulty;
    //    float max = maxMoveDelay / difficulty;

    //    Invoke(nameof(MoveRandom), Random.Range(min, max));
    //}

    //void MoveRandom()
    //{
    //    if (isGoldenFredrik || waypoints.Length == 0 || isFrozenAtDoor)
    //        return;

    //    if (isBeingWatched && !ShouldIgnoreCamera())
    //    {
    //        ScheduleNextMove();
    //        return;
    //    }

    //    bool moved = false;

    //    if (currentRoom == 1)
    //        moved = TryMoveToDoor(ref hallway1DoorChance);
    //    else if (currentRoom == 2)
    //        moved = TryMoveToDoor(ref hallway2DoorChance);

    //    if (!moved && currentRoom == 0)
    //    {
    //        if (Random.value < GetMinuteDifficultyMultiplier() * 0.3f)
    //        {
    //            MoveToRandomNonDoorWaypointAnywhere();
    //            moved = true;
    //        }
    //    }

    //    if (!moved)
    //        MoveToRandomNonDoorWaypointAnywhere();

    //    ScheduleNextMove();
    //}

    //bool ShouldIgnoreCamera()
    //{
    //    int minute = Mathf.Min(NightsDifficulty.CurrentMinute, 20);
    //    float chance = Mathf.Clamp(
    //        baseIgnoreCameraChance + minute * ignoreCameraIncreasePerMinute,
    //        0f, 50f
    //    );

    //    return Random.Range(0f, 100f) < chance;
    //}

    //// =========================
    //// Door Helpers
    //// =========================
    //bool TryMoveToDoor(ref float doorChance)
    //{
    //    float effectiveChance = doorChance * GetMinuteDifficultyMultiplier();
    //    if (currentRoom == 0) effectiveChance *= 1.5f;

    //    if (Random.Range(0f, 100f) <= effectiveChance)
    //    {
    //        RoomWaypoint door = GetDoorWaypointForCurrentRoom();
    //        if (door != null)
    //        {
    //            doorChance = baseDoorChance;
    //            MoveToWaypoint(System.Array.IndexOf(waypoints, door));
    //            return true;
    //        }
    //    }

    //    doorChance += doorChanceIncrease;
    //    return false;
    //}

    //RoomWaypoint GetDoorWaypointForCurrentRoom()
    //{
    //    foreach (var wp in waypoints)
    //        if (wp != null && wp.roomIndex == currentRoom && wp.isDoorWaypoint)
    //            return wp;

    //    return null;
    //}

    //void MoveToRandomNonDoorWaypointAnywhere()
    //{
    //    List<int> valid = new List<int>();
    //    int currentIndex = GetCurrentWaypointIndex();

    //    for (int i = 0; i < waypoints.Length; i++)
    //    {
    //        if (waypoints[i] != null && !waypoints[i].isDoorWaypoint && i != currentIndex)
    //            valid.Add(i);
    //    }

    //    if (valid.Count > 0)
    //        MoveToWaypoint(valid[Random.Range(0, valid.Count)]);
    //}

    //void MoveToWaypoint(int index)
    //{
    //    if (index < 0 || index >= waypoints.Length) return;
    //    RoomWaypoint wp = waypoints[index];
    //    if (wp == null) return;

    //    transform.position = wp.transform.position;
    //    currentRoom = wp.roomIndex;

    //    if (!string.IsNullOrEmpty(wp.roomLayer))
    //    {
    //        int layer = LayerMask.NameToLayer(wp.roomLayer);
    //        if (layer != -1) gameObject.layer = layer;
    //    }

    //    cameraManager?.RefreshAllAnimatronics();
    //}

    //int GetCurrentWaypointIndex()
    //{
    //    for (int i = 0; i < waypoints.Length; i++)
    //        if (waypoints[i] != null &&
    //            waypoints[i].roomIndex == currentRoom &&
    //            waypoints[i].transform.position == transform.position)
    //            return i;

    //    return -1;
    //}

    //// =========================
    //// Golden Fredrik Logic
    //// =========================
    //void ScheduleGoldenCheck()
    //{
    //    if (NightsDifficulty.CurrentMinute < 1)
    //    {
    //        Invoke(nameof(ScheduleGoldenCheck), 60f); // wait for 1 AM
    //        return;
    //    }

    //    Invoke(nameof(GoldenAttackCheck), 60f); // always every 60 seconds
    //}

    //void GoldenAttackCheck()
    //{
    //    Debug.Log("[Golden Fredrik] Attack Check");

    //    if (!NLS.Alive || goldenIsAttacking || !canGoldenAttack)
    //    {
    //        ScheduleGoldenCheck();
    //        return;
    //    }

    //    int minute = NightsDifficulty.CurrentMinute;
    //    float chance = Mathf.Clamp(
    //        goldenBaseAttackChance + (minute - 1) * goldenChanceIncreasePerMinute,
    //        0.1f, 0.2f
    //    );

    //    if (Random.Range(0f, 100f) <= chance)
    //    {
    //        GoldenAttack();
    //    }

    //    ScheduleGoldenCheck(); // schedule next 60s check
    //}

    //void GoldenAttack()
    //{
    //    if (goldenIsAttacking || !NLS.Alive) return;
    //    StartCoroutine(GoldenDoorRoutine());
    //}

    //IEnumerator GoldenDoorRoutine()
    //{
    //    goldenIsAttacking = true;

    //    RoomWaypoint door = GetAnyDoorWaypoint();
    //    if (door == null)
    //    {
    //        Debug.Log("[Golden Fredrik] Failed to move to door!");
    //        yield break;
    //    }

    //    transform.position = door.transform.position;
    //    currentRoom = door.roomIndex;
    //    isAtDoor = true;

    //    Debug.Log("[Golden Fredrik] Successfully moved to door!");

    //    float timer = 0f;
    //    while (timer < goldenKillDelay)
    //    {
    //        if (NLS.IsRightClosed)
    //        {
    //            SoundEffectsScript.instance.PlaySoundEffect(stepSound, 1f);
    //            ResetGoldenFredrik();
    //            yield break;
    //        }

    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    TriggerJumpscare();
    //}

    //RoomWaypoint GetAnyDoorWaypoint()
    //{
    //    foreach (var wp in waypoints)
    //        if (wp != null && wp.isDoorWaypoint)
    //            return wp;

    //    return null;
    //}

    //void ResetGoldenFredrik()
    //{
    //    goldenIsAttacking = false;
    //    isAtDoor = false;

    //    Debug.Log("[Golden Fredrik] Attack repelled, entering cooldown");

    //    StartCoroutine(GoldenRetreatCooldownRoutine());

    //    foreach (var wp in waypoints)
    //    {
    //        if (wp != null && !wp.isDoorWaypoint)
    //        {
    //            transform.position = wp.transform.position;
    //            currentRoom = wp.roomIndex;
    //            break;
    //        }
    //    }

    //    cameraManager?.RefreshAllAnimatronics();
    //}

    //IEnumerator GoldenRetreatCooldownRoutine()
    //{
    //    canGoldenAttack = false;
    //    yield return new WaitForSeconds(60f); // 1-minute cooldown
    //    canGoldenAttack = true;
    //}

    //void TriggerJumpscare()
    //{
    //    NLS.Camera.SetActive(false);
    //    NLS.Computer.SetActive(false);
    //    NLS.Office.SetActive(true);
    //    Jumpscare.SetActive(true);

    //    NLS.Alive = false;
    //    SoundEffectsScript.instance.PlaySoundEffect(NLS.jumpscareSound, 1f);
    //}

    //float GetMinuteDifficultyMultiplier()
    //{
    //    return 1f + NightsDifficulty.CurrentMinute * 0.15f;
    //}

    //float GetDifficultyMultiplier()
    //{
    //    return 1f + Mathf.Min(NightsDifficulty.CurrentMinute, 20) * 0.15f;
    //}

}