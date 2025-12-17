using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    [Header("Camera Setup")]
    [SerializeField] private GameObject[] Cameras; // Assign camera GameObjects in Inspector

    [SerializeField] private int startingCameraIndex = 1; // Camera 1 (index 1)

    private List<Animatronics> animatronics;
    private int activeCamera = 0;

    public bool camerasOpen = false;

    void Start()
    {

        camerasOpen = false; // Animatronics won't freeze

        // Deactivate all cameras first
        foreach (var cam in Cameras)
        {
            if (cam != null)
                cam.SetActive(false);
        }

        // Just assign starting camera index internally; don't activate it
        activeCamera = startingCameraIndex;

        // Update animatronics so they know the "current room" camera index
        UpdateAllAnimatronicVisibility();


        //camerasOpen = false;

        //foreach (var cam in Cameras)
        //{
        //    cam.SetActive(false);
        //}

    }

    void Awake()
    {
        // Automatically find all animatronics in the scene
        Animatronics[] foundAnims = Object.FindObjectsByType<Animatronics>(FindObjectsSortMode.None);
        animatronics = new List<Animatronics>(foundAnims);
    }

    public void SwitchCamera(int camera)
    {
        activeCamera = camera;
        camerasOpen = true;

        for (int i = 0; i < Cameras.Length; i++)
        {
            Cameras[i].SetActive(i == camera);
        }

        UpdateAllAnimatronicVisibility();
    }

    public void CloseCameras()
    {
        camerasOpen = false;

        foreach (var cam in Cameras)
        {
            cam.SetActive(false);
        }
           

        UpdateAllAnimatronicVisibility();
    }

    public void UpdateAnimatronicVisibility(Animatronics anim)
    {
        // Freeze ONLY if cameras are open AND player is watching his room
        anim.isBeingWatched = camerasOpen && anim.currentRoom == activeCamera;

        // Hide/show visually WITHOUT disabling the GameObject
        SpriteRenderer sr = anim.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = (anim.currentRoom == activeCamera);
        }
    }

    private void UpdateAllAnimatronicVisibility()
    {
        foreach (var anim in animatronics)
        {
            UpdateAnimatronicVisibility(anim);
        }
    }



    //[Header("Camera Setup")]
    //[SerializeField] private GameObject[] Cameras;      // Assign camera GameObjects in inspector
    ////[SerializeField] private int stageCameraIndex = 0;  // Index of the Stage camera (optional)

    //private List<Animatronics> animatronics;

    //private int activeCamera = 0;

    //void Awake()
    //{
    //    // Automatically find all animatronics in the scene (modern API)
    //    Animatronics[] foundAnims = Object.FindObjectsByType<Animatronics>(FindObjectsSortMode.None);
    //    animatronics = new List<Animatronics>(foundAnims);
    //}

    //public void SwitchCamera(int camera)
    //{
    //    activeCamera = camera;

    //    for (int i = 0; i < Cameras.Length; i++)
    //    {
    //        bool isActive = (i == camera);
    //        Cameras[i].SetActive(isActive);
    //    }

    //    UpdateAllAnimatronicVisibility();
    //}

    //public void UpdateAnimatronicVisibility(Animatronics anim)
    //{
    //    // Animatronic is being watched only if the **currently active camera's room matches his room**
    //    anim.isBeingWatched = (anim.currentRoom == activeCamera);

    //    // Show the animatronic only if he is in the active camera
    //    anim.gameObject.SetActive(anim.currentRoom == activeCamera);
    //}

    //private void UpdateAllAnimatronicVisibility()
    //{
    //    foreach (var anim in animatronics)
    //    {
    //        UpdateAnimatronicVisibility(anim);
    //    }
    //}










    //[SerializeField] GameObject[] Cameras; // Array of camera GameObjects
    //[SerializeField] GameObject fredrik;   // Fredrik GameObject

    //private int fredrikRoom = 0; // Which room Fredrik is currently in

    //public void test(int camera)
    //{
    //    for (int i = 0; i < Cameras.Length; i++)
    //    {
    //        bool isActive = (i == camera);
    //        Cameras[i].SetActive(isActive);

    //        // Show Fredrik only if he is in this camera's room
    //        if (fredrik != null)
    //        {
    //            fredrik.SetActive(fredrikRoom == camera && isActive);
    //        }
    //    }
    //}

    //// Called by FredrikMovement when he changes rooms
    //public void UpdateFredrikVisibility(int room)
    //{
    //    fredrikRoom = room;

    //    // Refresh camera visibility in case a camera is open
    //    for (int i = 0; i < Cameras.Length; i++)
    //    {
    //        if (Cameras[i].activeSelf)
    //        {
    //            fredrik.SetActive(fredrikRoom == i);
    //        }
    //    }
    //}







    //[SerializeField] GameObject[] Cameras; 



    //public void test(int camera)
    //{
    //    for (int i = 0; i < Cameras.Length; i++)
    //    {
    //        if (i == camera)
    //        {
    //            Cameras[i].SetActive(true); 
    //        }
    //        else
    //        {
    //            Cameras[i].SetActive(false); 
    //        }
    //    }
    //}

}
