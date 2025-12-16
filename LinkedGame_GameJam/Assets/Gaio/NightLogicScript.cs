using UnityEngine;
using UnityEngine.SceneManagement;

public class NightLogicScript : MonoBehaviour
{
    public bool Alive = true;

    public SpriteRenderer LeftSR, RightSR;
    public GameObject[] OfficeParts;
    private GameObject currentPart;
    private int index = 0;
    public GameObject Camera;
    public GameObject Office;
    public GameObject Computer;
    public GameObject Ventilation;
    public bool CameraisOpen = false;
    public bool ComputerisOpen = false;
    public bool VentisOpen = false;
    private bool isHacking = false;
    public Sprite RightClosed, RightOpen, LeftClosed, LeftOpen;
    private bool LeftisClosed = false;
    private bool RightisClosed = false;
    public float DoorCooldown;
    private float LeftCooldown, RightCooldown;

    public float HackTime;
    private float HackTimer;
    public Sprite computerSprite, hackingSprite1, hackingSprite2, hackingSprite3, hackingSprite4, hackingSprite5, doneSprite;

    public AudioClip doorSound, turnSound, yamsSound, hackSound, clickSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPart = OfficeParts[index];
        LeftCooldown = DoorCooldown;
        RightCooldown = DoorCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Alive)
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
            else if (ComputerisOpen == true)
            {
                Computer.SetActive(true);
                Office.SetActive(false);
            }
            else if (VentisOpen == true)
            {
                Ventilation.SetActive(true);
                Office.SetActive(false);
            }
            else
            {
                Office.SetActive(true);
                Camera.SetActive(false);
                Computer.SetActive(false);
                Ventilation.SetActive(false);
            }
            LeftCooldown -= UnityEngine.Time.deltaTime;
            RightCooldown -= UnityEngine.Time.deltaTime;

            if (isHacking == true)
            {
                HackTimer -= UnityEngine.Time.deltaTime;
                UpdateHacking();
            }
        }        
    }
    public void UpdateHacking()
    {
        if (HackTimer >= 12)
        {
            Computer.GetComponent<SpriteRenderer>().sprite = hackingSprite1;
        }
        else if (HackTimer >= 9)
        {
            Computer.GetComponent<SpriteRenderer>().sprite = hackingSprite2;
        }
        else if (HackTimer >= 6)
        {
            Computer.GetComponent<SpriteRenderer>().sprite = hackingSprite3;
        }
        else if (HackTimer >= 3)
        {
            Computer.GetComponent<SpriteRenderer>().sprite = hackingSprite4;
        }
        else if (HackTimer >= 0)
        {
            Computer.GetComponent<SpriteRenderer>().sprite = hackingSprite5;
        }
        else
        {
            Computer.GetComponent<SpriteRenderer>().sprite = doneSprite;
            isHacking = false;
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
        SoundEffectsScript.instance.PlaySoundEffect(turnSound, 0.5f);
        
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
        SoundEffectsScript.instance.PlaySoundEffect(turnSound, 0.5f);
        
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

    public void VentToggle()
    {
        if (VentisOpen == true)
        {
            VentisOpen = false;
        }
        else
        {
            VentisOpen = true;
        }
    }
    public void ComputerToggle()
    {
        if (ComputerisOpen == true && isHacking == false)
        {
            ComputerisOpen = false;
        }
        else
        {
            ComputerisOpen = true;
            Computer.GetComponent<SpriteRenderer>().sprite = computerSprite;
        }
    }

    public void Hack()
    {
        if (isHacking == false)
        {
            isHacking = true;
            HackTimer = HackTime;
            SoundEffectsScript.instance.PlaySoundEffect(hackSound, 1f);
        }
        else
        {

        }
    }
    public void LeftDoorToggle()
    {
        if (LeftCooldown <= 0)
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

            SoundEffectsScript.instance.PlaySoundEffect(doorSound, 1f);
            LeftCooldown = DoorCooldown;
        }
    }

    public void RightDoorToggle()
    {
        if (RightCooldown <= 0)
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

            SoundEffectsScript.instance.PlaySoundEffect(doorSound, 1f);
            RightCooldown = DoorCooldown;
        }
    }
}
