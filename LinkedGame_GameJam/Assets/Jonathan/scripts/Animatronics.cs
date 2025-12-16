using UnityEngine;
using System.Collections;

public class Animatronics : MonoBehaviour
{
    [Header("Rooms")]
    public Transform[] roomPositions;        // One position per camera room
    public string[] roomSortingLayers;       // Sorting layer per room

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

        // Start on Stage
        currentRoom = 0;
        ApplyRoomState();

        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minMoveDelay, maxMoveDelay);
            yield return new WaitForSeconds(waitTime);

            Debug.Log("Trying to move. isBeingWatched = " + isBeingWatched);

            // If being watched, skip this move attempt
            if (isBeingWatched)
            {
                continue;
            }
                

           

            MoveToRandomRoom();
        }
    }

    void MoveToRandomRoom()
    {
        if (roomPositions.Length < 2)
        {
            return;
        }
            

        int nextRoom;
        do
        {
            nextRoom = Random.Range(0, roomPositions.Length);
        }
        while (nextRoom == currentRoom);

        currentRoom = nextRoom;
        ApplyRoomState();
    }

    void ApplyRoomState()
    {
        // Instantly move to room position
        transform.position = roomPositions[currentRoom].position;

        // Apply sorting layer
        if (sr != null && currentRoom < roomSortingLayers.Length)
        {
            sr.sortingLayerName = roomSortingLayers[currentRoom];
        }
            

        // Update camera visibility + watched state
        if (cameraManager != null)
        {
            cameraManager.UpdateAnimatronicVisibility(this);
        }
            
    }











    //[Header("Movement Settings")]
    //public Transform[] waypoints;         // Rooms the animatronic can move to
    //public string[] roomSortingLayers;    // Sorting Layer for each room
    //public float minMoveDelay = 5f;       // Minimum delay before moving
    //public float maxMoveDelay = 15f;      // Maximum delay before moving
    //public float extraPauseAfterWatched = 1.5f; // Extra pause after being watched

    //[HideInInspector] public int currentRoom = 0;
    //[HideInInspector] public bool isBeingWatched = false;

    //private SpriteRenderer sr;
    //private MainCameraScript cameraManager;

    //void Start()
    //{
    //    sr = GetComponent<SpriteRenderer>();
    //    if (sr == null) Debug.LogWarning("Animatronic missing SpriteRenderer!");

    //    cameraManager = Object.FindFirstObjectByType<MainCameraScript>();

    //    if (waypoints.Length == 0)
    //    {
    //        Debug.LogError("No waypoints set for Animatronic!");
    //        return;
    //    }

    //    currentRoom = 0;
    //    transform.position = waypoints[currentRoom].position;
    //    SetSortingLayerForCurrentRoom();
    //    NotifyCameraManager();

    //    StartCoroutine(MoveRoutine());
    //}

    //IEnumerator MoveRoutine()
    //{
    //    while (true)
    //    {
    //        float waitTime = Random.Range(minMoveDelay, maxMoveDelay);
    //        yield return new WaitForSeconds(waitTime);

    //        // Only move if not being watched
    //        if (!isBeingWatched)
    //        {
    //            MoveToNextRoom();
    //        }
    //        else
    //        {
    //            // If watched, wait a little longer before checking again
    //            yield return new WaitForSeconds(extraPauseAfterWatched);
    //        }
    //    }
    //}

    //void MoveToNextRoom()
    //{
    //    if (waypoints.Length < 2) return;

    //    int nextRoom;
    //    do
    //    {
    //        nextRoom = Random.Range(0, waypoints.Length);
    //    } while (nextRoom == currentRoom);

    //    currentRoom = nextRoom;
    //    transform.position = waypoints[currentRoom].position;
    //    SetSortingLayerForCurrentRoom();
    //    NotifyCameraManager();
    //}

    //void SetSortingLayerForCurrentRoom()
    //{
    //    if (sr == null) return;

    //    if (currentRoom < roomSortingLayers.Length)
    //    {
    //        sr.sortingLayerName = roomSortingLayers[currentRoom];
    //    }
    //    else
    //    {
    //        sr.sortingLayerName = "Default";
    //    }

    //    sr.sortingOrder = 1;
    //}

    //public void NotifyCameraManager()
    //{
    //    if (cameraManager != null)
    //    {
    //        cameraManager.UpdateAnimatronicVisibility(this);
    //    }
    //}



}
