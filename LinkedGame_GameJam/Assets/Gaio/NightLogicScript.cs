using UnityEngine;
using UnityEngine.SceneManagement;

public class NightLogicScript : MonoBehaviour
{
    public SpriteRenderer LeftSR, RightSR;
    public GameObject[] OfficeParts;
    private GameObject currentPart;
    private int index = 0;
    public GameObject Camera;
    public GameObject Office;
    public bool CameraisOpen = false;
    public Sprite RightClosed, RightOpen, LeftClosed, LeftOpen;
    public bool LeftisClosed = false;
    public bool RightisClosed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPart = OfficeParts[index];
    }

    // Update is called once per frame
    void Update()
    {
        currentPart = OfficeParts[index];
        for (int i = 0; i < OfficeParts.Length; i++)
        {
            if (currentPart == OfficeParts[i])
            {
                OfficeParts[i].SetActive(true);
            }
            else
            {
                OfficeParts[i].SetActive(false);
            }
        }

        if (CameraisOpen == true)
        {
            Camera.SetActive(true);
            Office.SetActive(false);
        }
        else
        {
            Office.SetActive(true);
            Camera.SetActive(false);
        }
    }
    
    public void Right()
    {
        if (index == 3)
        {
            index = 0;
        }
        else
        {
            index++;
        }
    }
    public void Left()
    {
        if (index == 0)
        {
            index = 3;
        }
        else
        {
            index--;
        }
    }
    public void CameraToggle()
    {
        if (CameraisOpen == true)
        {
            CameraisOpen = false;
        }
        else
        {
            CameraisOpen = true;
        }
    }

    public void LeftDoorToggle()
    {
        if (LeftisClosed == true)
        {
            LeftisClosed = false;
            LeftSR.sprite = LeftOpen;
        }
        else
        {
            LeftisClosed = true;
            LeftSR.sprite = LeftClosed;
        }
    }

    public void RightDoorToggle()
    {
        if (RightisClosed == true)
        {
            RightisClosed = false;
            RightSR.sprite = RightOpen;
        }
        else
        {
            RightisClosed = true;
            RightSR.sprite = RightClosed;
        }
    }
}
