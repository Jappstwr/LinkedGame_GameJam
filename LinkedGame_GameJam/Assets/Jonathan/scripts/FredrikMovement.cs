using UnityEngine;

public class FredrikMovement : MonoBehaviour
{
    //public Transform[] waypoints; // 0 = Stage, 1 = Hallway, 2 = Hallway2, etc.
    //public float moveDelay = 10f;

    //[HideInInspector] public int currentRoom = 0; // Tracks Fredrik's room

    //private MainCameraScript cameraManager;

    //void Start()
    //{
    //    cameraManager = FindObjectOfType<MainCameraScript>();
    //    Invoke(nameof(MoveToNextRoom), moveDelay);
    //}

    //void MoveToNextRoom()
    //{
    //    // Move to the next waypoint
    //    currentRoom++;
    //    if (currentRoom >= waypoints.Length) currentRoom = waypoints.Length - 1;

    //    transform.position = waypoints[currentRoom].position;

    //    // Update camera visibility
    //    if (cameraManager != null)
    //    {
    //        cameraManager.UpdateFredrikVisibility(currentRoom);
    //    }

    //    // Schedule next move if needed
    //    // Invoke(nameof(MoveToNextRoom), moveDelay);
    //}



    //public Transform stagePoint;
    //public Transform hallwayPoint;
    //public Transform hallwayPoint2; 

    ////public RectTransform stagePoint;
    ////public RectTransform hallwayPoint;

    //public float moveDelay = 10f;

    //void Start()
    //{
    //    Invoke(nameof(MoveToHallway), moveDelay); 
    //}
    //void MoveToHallway()
    //{
    //    transform.position = hallwayPoint.position;

    //    //GetComponent<RectTransform>().anchoredPosition = hallwayPoint.anchoredPosition;
    //}
}
