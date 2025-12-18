using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenScript : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0); 
    }


}
