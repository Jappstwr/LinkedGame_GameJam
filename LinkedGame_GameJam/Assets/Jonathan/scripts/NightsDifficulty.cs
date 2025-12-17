using UnityEngine;

public class NightsDifficulty : MonoBehaviour
{
   public static int CurrentMinute { get; private set; }

    public float minuteLength = 60f;
    public float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= minuteLength)
        {
            timer -= minuteLength;

            if (CurrentMinute < 20) // Cap at 20 minutes
            {
                CurrentMinute++;
                Debug.Log($"Night minute advanced → {CurrentMinute}");
            }
        }
    }
}
