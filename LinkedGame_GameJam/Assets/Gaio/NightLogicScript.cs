using UnityEngine;
using UnityEngine.SceneManagement;

public class NightLogicScript : MonoBehaviour
{
    public bool Alive = true;

    public SpriteRenderer LeftSR, RightSR, FrontSR;
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

    // Public getters to expose them
    public bool IsLeftClosed => LeftisClosed;
    public bool IsRightClosed => RightisClosed;

    public float DoorPower;
    private float LeftPower, RightPower;
    public float DoorCooldown;
    private float LeftCooldown, RightCooldown;
    public float ErrorCooldown;
    private float ErrorTimer;
    public bool Error;

    public float HackTime;
    private float HackTimer;
    public Sprite frontSprite, errorSprite, computerSprite, hackingSprite1, hackingSprite2, hackingSprite3, hackingSprite4, hackingSprite5, doneSprite;

    public AudioClip doorSound, turnSound, yamsSound, hackSound, clickSound, pressSound, jumpscareSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPart = OfficeParts[index];
        LeftCooldown = DoorCooldown;
        RightCooldown = DoorCooldown;
        LeftPower = DoorPower;
        RightPower = DoorPower;
        ErrorTimer = ErrorCooldown;
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

            if (RightisClosed)
            {
                RightPower -= UnityEngine.Time.deltaTime;
                if (RightPower <= 0)
                {
                    RightisClosed = false;
                    RightSR.sprite = RightOpen;
                    SoundEffectsScript.instance.PlaySoundEffect(doorSound, 1f);
                }
            }
            else
            {
                RightPower += UnityEngine.Time.deltaTime / 2;
            }

            if (LeftisClosed)
            {
                LeftPower -= UnityEngine.Time.deltaTime;
                if (LeftPower <= 0)
                {
                    LeftisClosed = false;
                    LeftSR.sprite = LeftOpen;
                    SoundEffectsScript.instance.PlaySoundEffect(doorSound, 1f);
                }
            }
            else
            {
                LeftPower += UnityEngine.Time.deltaTime / 2;
            }

            if (CameraisOpen)
            {
                ErrorTimer -= UnityEngine.Time.deltaTime;
            }
            if (ErrorTimer <= 0)
            {
                Error = true;
                FrontSR.sprite = errorSprite;
            }

            if (CameraisOpen && ErrorTimer <= 0)
            {
                CameraToggle();
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
            FrontSR.sprite = frontSprite;
            isHacking = false;
            Error = false;
            ErrorTimer = ErrorCooldown;
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
        SoundEffectsScript.instance.PlaySoundEffect(pressSound, 0.2f);
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
        SoundEffectsScript.instance.PlaySoundEffect(pressSound, 0.2f);
    }
    public void CameraToggle()
    {
        if (CameraisOpen == false && Error == false)
        {
            CameraisOpen = true;
            SoundEffectsScript.instance.PlaySoundEffect(pressSound, 0.2f);
            ErrorTimer -= 3;
        }
        else if (CameraisOpen == true)
        {
            CameraisOpen = false;
            SoundEffectsScript.instance.PlaySoundEffect(pressSound, 0.2f);
        }
    }

    public void VentToggle()
    {
        VentisOpen = !VentisOpen;
        SoundEffectsScript.instance.PlaySoundEffect(pressSound, 0.2f);
    }
    public void ComputerToggle()
    {
        if (ComputerisOpen == true && isHacking == false)
        {
            ComputerisOpen = false;
            SoundEffectsScript.instance.PlaySoundEffect(pressSound, 0.2f);
        }
        else if (ComputerisOpen == false && Error == true)
        {
            ComputerisOpen = true;
            Computer.GetComponent<SpriteRenderer>().sprite = computerSprite;
            SoundEffectsScript.instance.PlaySoundEffect(pressSound, 0.2f);
        }
    }

    public void Hack()
    {
        if (isHacking == false)
        {
            isHacking = true;
            HackTimer = HackTime;
            SoundEffectsScript.instance.PlaySoundEffect(clickSound, 1f);
            SoundEffectsScript.instance.PlaySoundEffect(hackSound, 1f);
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
                SoundEffectsScript.instance.PlaySoundEffect(doorSound, 0.2f);
            }
            else if (LeftPower >= 5)
            {
                LeftisClosed = true;
                LeftSR.sprite = LeftClosed;
                SoundEffectsScript.instance.PlaySoundEffect(doorSound, 1f);
            }

            SoundEffectsScript.instance.PlaySoundEffect(pressSound, 0.2f);
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
                SoundEffectsScript.instance.PlaySoundEffect(doorSound, 1f);
            }
            else if (RightPower >= 5)
            {
                RightisClosed = true;
                RightSR.sprite = RightClosed;
                SoundEffectsScript.instance.PlaySoundEffect(doorSound, 1f);
            }

            SoundEffectsScript.instance.PlaySoundEffect(pressSound, 0.2f);
            RightCooldown = DoorCooldown;
        }
    }
}
