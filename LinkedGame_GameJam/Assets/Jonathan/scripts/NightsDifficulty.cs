using System.Threading;
using UnityEngine;

public class NightsDifficulty : MonoBehaviour
{
    public static int CurrentMinute { get; private set; }

    public float minuteLength = 60f;
    private float timer;

    private static NightsDifficulty instance;

    void Awake()
    {
        // Proper singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Start night using custom night config
        CurrentMinute = Mathf.Clamp(CustomNightConfig.StartingMinute, 0, 20);
        timer = 0f;

        Debug.Log($"Night started at minute {CurrentMinute}");
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= minuteLength)
        {
            timer -= minuteLength;

            if (CurrentMinute < 20)
            {
                CurrentMinute++;
                Debug.Log($"Night minute advanced → {CurrentMinute}");
            }
        }
    }

    // Called by Custom Night dropdown
    public static void SetStartingMinute(int value)
    {
        CurrentMinute = Mathf.Clamp(value, 0, 20);

        if (instance != null)
        {
            instance.timer = 0f;
        }
    }

    // Called when dying / returning to menu
    public static void ResetNight()
    {
        CurrentMinute = 0;

        if (instance != null)
        {
            instance.timer = 0f;
        }

        Debug.Log("Night reset");
    }

    //public static int CurrentMinute { get; private set; }

    //public float minuteLength = 60f;
    //private float timer = 0f;

    //private static NightsDifficulty instance; 
    //void Awake()
    //{
    //    if (instance != null || instance != this)
    //    {
    //        Destroy(gameObject);
    //        return; 
    //    }

    //    instance = this;
    //    timer = 0f;
    //    CurrentMinute = 0; 
    //}
    //public static void SetStartingMinute(int value)
    //{
    //    if (instance != null)
    //    {
    //        return; 
    //    }

    //    CurrentMinute = Mathf.Clamp(value, 0, 20);
    //    instance.timer = 0f; //reset advanced time minute (progression timer)
    //}
    //void Update()
    //{
    //    // Always advance time
    //    timer += Time.deltaTime;

    //    if (timer >= minuteLength)
    //    {
    //        timer -= minuteLength;

    //        if (CurrentMinute < 20) 
    //        {
    //            CurrentMinute++;
    //            Debug.Log($"Night minute advanced → {CurrentMinute}");
    //        }
    //    }
    //}

    //public static void ResetNight()
    //{
    //    if(instance != null)
    //    {
    //        return; 
    //    }
    //    CurrentMinute = 0;
    //    instance.timer = 0f; 
    //}
}
