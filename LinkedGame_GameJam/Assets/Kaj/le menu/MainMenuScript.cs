using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Empty
    }

    // Update is called once per frame
    void Update()
    {
        // Empty
    }

    public void PlayGame()
    {
        Debug.Log("omg you are now in the game");
        // TODO
    }

    public void ExitGame()
    {
        Debug.Log("exit game");
        Application.Quit();
    }
}
