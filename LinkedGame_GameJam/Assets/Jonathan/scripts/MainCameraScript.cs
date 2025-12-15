using System.Linq.Expressions;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    //[SerializeField] GameObject MainSceneCamera; 
    //[SerializeField] GameObject Camera1;
    //[SerializeField] GameObject Camera2;
    //[SerializeField] GameObject Camera3;

    [SerializeField] GameObject[] Cameras; 

    //[SerializeField] GameObject BackButton;

    //[SerializeField] GameObject StageScene; 


    public void test(int camera)
    {
        for (int i = 0; i < Cameras.Length; i++)
        {
            if (i == camera)
            {
                Cameras[i].SetActive(true); 
            }
            else
            {
                Cameras[i].SetActive(false); 
            }
        }
    }

    //public void MainSceneCam()
    //{
    //    MainSceneCamera.SetActive(true);
    //    Camera1.SetActive(false); 

    //}
    //public void CAM1()
    //{
    //    MainSceneCamera.SetActive(true); 
    //    Camera1.SetActive(true);
    //    StageScene.SetActive(true); 

    //    Camera2.SetActive(false);
    //    Camera3.SetActive(false);

    //    Debug.Log("Opened Cam 1"); 
    //}
    //public void CAM2()
    //{

    //    MainSceneCamera.SetActive(true);
    //    StageScene.SetActive(false); 
    //    Camera1.SetActive(false);
    //    Camera2.SetActive(true);
    //    Camera3.SetActive(false);
    //}
    //public void CAM3()
    //{

    //    MainSceneCamera.SetActive(true);
    //    StageScene.SetActive(false); 
    //    Camera1.SetActive(false);
    //    Camera2.SetActive(false);
    //    Camera3.SetActive(true);
    //}
    //public void BackToGame()
    //{
    //    MainSceneCamera.SetActive(true);
    //    StageScene.SetActive(false); 
    //    Camera1.SetActive(false);
    //    Camera2.SetActive(false);
    //    Camera3.SetActive(false);
    //}
}
