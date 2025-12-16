using UnityEngine;

public class VentScript : MonoBehaviour
{
    public NightLogicScript NLS;

    public GameObject Jumpscare;
    public int AiLevel;
    public int currentPosition;
    public float OpportunityTime;
    private float MovementTimer;
    private int chance = 1;

    public AudioClip advanceSound, retreatSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovementTimer = OpportunityTime;
        AiLevel = 20;
    }

    // Update is called once per frame
    void Update()
    {
        MovementTimer -= UnityEngine.Time.deltaTime;
        if (MovementTimer <= 0)
        {
            Opportunity();
            MovementTimer = OpportunityTime;
        }
    }

    public void Opportunity()
    {
        int rng = Random.Range(1, 21);

        if (rng <= AiLevel)
        {
            if (currentPosition == 2)
            {
                if (NLS.CameraisOpen || NLS.ComputerisOpen)
                {
                    if (chance > 0)
                    {
                        chance--;
                    }
                    else
                    {
                        //JUMPSCARE
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
    }
}
