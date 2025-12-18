using UnityEngine;
using UnityEngine.SceneManagement;
public class JumpscareScript : MonoBehaviour
{
    public float Time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time -= UnityEngine.Time.deltaTime;
    }
}
