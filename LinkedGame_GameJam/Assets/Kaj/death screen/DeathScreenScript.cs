using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenScript : MonoBehaviour
{
    public void GoToMainMenu()
    {
        NightsDifficulty.ResetNight();
        CustomNightConfig.Reset();
        SceneManager.LoadScene(0); 
    }


}
