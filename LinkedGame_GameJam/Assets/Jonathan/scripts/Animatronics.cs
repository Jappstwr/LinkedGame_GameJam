using UnityEngine;
using System.Collections;

public class Animatronics : MonoBehaviour
{
    [Header("Waypoints")]
    public RoomWaypoint[] waypoints;      // All waypoints for this animatronic

    [Header("Movement Timing")]
    public float minMoveDelay = 5f;
    public float maxMoveDelay = 15f;

    [HideInInspector] public int currentRoom = 0;
    [HideInInspector] public bool isBeingWatched = false;

    private SpriteRenderer sr;
    private MainCameraScript cameraManager;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cameraManager = Object.FindFirstObjectByType<MainCameraScript>();

        // Start at first waypoint
        if (waypoints.Length > 0)
        {
            MoveToWaypoint(0);
        }
           

        // Begin automatic movement
        Invoke(nameof(MoveRandom), Random.Range(minMoveDelay, maxMoveDelay));
    }

    void MoveRandom()
    {
        if (waypoints.Length == 0) return;

        int nextIndex;
        do
        {
            nextIndex = Random.Range(0, waypoints.Length);
        } while (nextIndex == GetCurrentWaypointIndex());

        MoveToWaypoint(nextIndex);

        // Schedule next move
        Invoke(nameof(MoveRandom), Random.Range(minMoveDelay, maxMoveDelay));
    }

    void MoveToWaypoint(int index)
    {
        if (index < 0 || index >= waypoints.Length) return;

        RoomWaypoint wp = waypoints[index];
        if (wp == null)
        {
            return;
        }

        Debug.Log($"{name} isBeingWatched = {isBeingWatched}");

        // Only move if not being watched
        if (isBeingWatched)
        {
            return;
        }

        // Move to waypoint position
        transform.position = wp.transform.position;

        // Update currentRoom for camera visibility
        currentRoom = wp.roomIndex;

        // Update camera visibility (no need to change layers)
        if (cameraManager != null)
        {
            cameraManager.UpdateAnimatronicVisibility(this);
        }


        Debug.Log($"{name} moved to waypoint {index} at position {wp.transform.position} in room {currentRoom}");
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


    //[Header("Rooms")]
    //public Transform[] roomPositions;        // One position per camera room
    //public string[] roomSortingLayers;       // Sorting layer per room

    //[Header("Movement Timing")]
    //public float minMoveDelay = 5f;
    //public float maxMoveDelay = 15f;

    //[HideInInspector] public int currentRoom = 0;
    //[HideInInspector] public bool isBeingWatched = false;

    //private SpriteRenderer sr;
    //private MainCameraScript cameraManager;

    //void Start()
    //{
    //    sr = GetComponent<SpriteRenderer>();
    //    cameraManager = Object.FindFirstObjectByType<MainCameraScript>();

    //    // Start on Stage
    //    currentRoom = 0;
    //    ApplyRoomState();

    //    StartCoroutine(MoveLoop());
    //}

    //IEnumerator MoveLoop()
    //{
    //    while (true)
    //    {
    //        float waitTime = Random.Range(minMoveDelay, maxMoveDelay);
    //        yield return new WaitForSeconds(waitTime);

    //        Debug.Log("Trying to move. isBeingWatched = " + isBeingWatched);

    //        // If being watched, skip this move attempt
    //        if (isBeingWatched)
    //        {
    //            continue;
    //        }




    //        MoveToRandomRoom();
    //    }
    //}

    //void MoveToRandomRoom()
    //{
    //    if (roomPositions.Length < 2)
    //    {
    //        return;
    //    }


    //    int nextRoom;
    //    do
    //    {
    //        nextRoom = Random.Range(0, roomPositions.Length);
    //    }
    //    while (nextRoom == currentRoom);

    //    currentRoom = nextRoom;
    //    ApplyRoomState();
    //}

    //void ApplyRoomState()
    //{
    //    // Instantly move to room position
    //    transform.position = roomPositions[currentRoom].position;

    //    // Apply sorting layer
    //    if (sr != null && currentRoom < roomSortingLayers.Length)
    //    {
    //        sr.sortingLayerName = roomSortingLayers[currentRoom];
    //    }


    //    // Update camera visibility + watched state
    //    if (cameraManager != null)
    //    {
    //        cameraManager.UpdateAnimatronicVisibility(this);
    //    }

    //}
}