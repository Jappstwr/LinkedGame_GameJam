using UnityEngine;

public class NightsDifficulty : MonoBehaviour
{
    public static int CurrentMinute { get; private set; }

    public float minuteLength = 60f;
    private float timer = 0f;

    void Update()
    {
        // Always advance time
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
}
