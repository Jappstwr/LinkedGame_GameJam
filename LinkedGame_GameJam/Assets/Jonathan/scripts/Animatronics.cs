using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Animatronics : MonoBehaviour
{

    [Header("Waypoints")]
    public RoomWaypoint[] waypoints;      // All waypoints for this animatronic

    [Header("Movement Timing")]
    public float minMoveDelay = 5f;
    public float maxMoveDelay = 15f;

    [Header("Door Chance")]
    [Range(0f, 100f)] public float baseDoorChance = 10f;
    public float doorChanceIncrease = 5f;

    private float hallway1DoorChance;
    private float hallway2DoorChance;

    [HideInInspector] public int currentRoom = 0;
    [HideInInspector] public bool isBeingWatched = false;

    private SpriteRenderer sr;
    private MainCameraScript cameraManager;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cameraManager = Object.FindFirstObjectByType<MainCameraScript>();

        hallway1DoorChance = baseDoorChance;
        hallway2DoorChance = baseDoorChance;

        // Start at first waypoint
        if (waypoints.Length > 0)
        {
            MoveToWaypoint(0);
        }

        ScheduleNextMove();
    }

    void ScheduleNextMove()
    {
        float difficulty = GetMinuteDifficultyMultiplier();
        float min = minMoveDelay / difficulty;
        float max = maxMoveDelay / difficulty;

        Invoke(nameof(MoveRandom), Random.Range(min, max));
    }

    void MoveRandom()
    {
        if (waypoints.Length == 0) return;

        Debug.Log($"{name} attempting move. Room: {currentRoom}, isBeingWatched={isBeingWatched}");

        if (isBeingWatched)
        {
            ScheduleNextMove();
            return;
        }

        // Check if we're in a hallway with door probability
        if (currentRoom == 1)
        {
            if (TryMoveToDoor(ref hallway1DoorChance))
            {
                ScheduleNextMove();
                return;
            }
        }
        else if (currentRoom == 2)
        {
            if (TryMoveToDoor(ref hallway2DoorChance))
            {
                ScheduleNextMove();
                return;
            }
        }

        // Move to any random non-door waypoint (all rooms included)
        MoveToRandomNonDoorWaypointAnywhere();
        ScheduleNextMove();
    }

    bool TryMoveToDoor(ref float doorChance)
    {
        float effectiveChance = doorChance * GetMinuteDifficultyMultiplier();
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
                return wp;
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
                i != currentIndex) // exclude current waypoint
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
        if (index < 0 || index >= waypoints.Length) return;
        RoomWaypoint wp = waypoints[index];
        if (wp == null) return;

        Debug.Log($"{name} moving to waypoint {index}, Room {wp.roomIndex}, isDoor={wp.isDoorWaypoint}");

        if (isBeingWatched) return;

        transform.position = wp.transform.position;
        currentRoom = wp.roomIndex;

        if (cameraManager != null)
            cameraManager.UpdateAnimatronicVisibility(this);
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

    float GetMinuteDifficultyMultiplier()
    {
        int minute = NightsDifficulty.CurrentMinute;
        return 1f + (minute * 0.15f); // scales door chance and movement
    }
}