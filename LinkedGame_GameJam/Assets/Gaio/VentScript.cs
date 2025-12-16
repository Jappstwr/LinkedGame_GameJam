using UnityEngine;

public class VentScript : MonoBehaviour
{
    public NightLogicScript NLS;

    public GameObject Jumpscare;
    public int AiLevel;
    public int currentPosition;
    public float OpportunityTime;
    private float MovementTimer;
    private int Immortality = 1;
    private bool killMode = false;

    public AudioClip advanceSound, retreatSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovementTimer = OpportunityTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (NLS.Alive)
        {
            if (killMode == false)
            {
                MovementTimer -= UnityEngine.Time.deltaTime;
                if (MovementTimer <= 0)
                {
                    Opportunity();
                    MovementTimer = OpportunityTime;
                }
            }
            else
            {
                if (NLS.CameraisOpen || NLS.ComputerisOpen || NLS.VentisOpen)
                {
                    NLS.Camera.SetActive(false);
                    NLS.Computer.SetActive(false);
                    NLS.Office.SetActive(false);
                    Jumpscare.SetActive(true);

                    NLS.Alive = false;
                    SoundEffectsScript.instance.PlaySoundEffect(NLS.yamsSound, 1f);
                }
            }
        }
    }

    public void Opportunity()
    {
        int rng = Random.Range(1, 21);

        if (rng <= AiLevel)
        {
            if (currentPosition == 2)
            {
                if (currentPosition == 2 && NLS.VentisOpen)
                {
                    currentPosition = 0;
                    Immortality = 1;
                    SoundEffectsScript.instance.PlaySoundEffect(retreatSound, 1f);
                }
                else if (Immortality > 0)
                {
                    Immortality--;
                }
                else
                {
                    if (NLS.CameraisOpen || NLS.ComputerisOpen)
                    {
                        NLS.Camera.SetActive(false);
                        NLS.Computer.SetActive(false);
                        NLS.Office.SetActive(false);
                        Jumpscare.SetActive(true);

                        NLS.Alive = false;
                        SoundEffectsScript.instance.PlaySoundEffect(NLS.yamsSound, 1f);
                    }
                    else
                    {
                        killMode = true;
                    }
                }
            }
            else if(currentPosition == 1)
            {
                currentPosition++;
                SoundEffectsScript.instance.PlaySoundEffect(advanceSound, 1f);
            }
            else
            {
                currentPosition++;
                SoundEffectsScript.instance.PlaySoundEffect(advanceSound, 0.2f);
            }
        }
        else if (currentPosition == 2 && NLS.VentisOpen)
        {
            currentPosition = 0;
            Immortality = 1;
            SoundEffectsScript.instance.PlaySoundEffect(retreatSound, 1f);
        }
    }
}
