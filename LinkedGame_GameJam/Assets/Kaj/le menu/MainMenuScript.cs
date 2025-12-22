using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject PlayButton;
    [SerializeField] GameObject ExitButton;
    [SerializeField] GameObject ModefiersMenu;
    [SerializeField] GameObject ModefiersButton; 
    [SerializeField] GameObject LeaderBoardButton;
    [SerializeField] GameObject NameSelection;
    [SerializeField] GameObject GAMEName;



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
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("exit game");
        Application.Quit();
    }

    public void Modefiers()
    {
        PlayButton.SetActive(false);
        ExitButton.SetActive(false);
        ModefiersMenu.SetActive(true);
        LeaderBoardButton.SetActive(false);
        NameSelection.SetActive(false);
        ModefiersButton.SetActive(false);
        GAMEName.SetActive(false); 
    }

    public void BackButtonModefiers()
    {
        PlayButton.SetActive(true);
        ExitButton.SetActive(true);
        ModefiersMenu.SetActive(false);
        LeaderBoardButton.SetActive(true);
        NameSelection.SetActive(false);
        ModefiersButton.SetActive(true);
        GAMEName.SetActive(true); 
    }
}
