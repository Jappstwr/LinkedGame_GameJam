using UnityEngine;

public class AnimatronicsManager : MonoBehaviour
{
    public static AnimatronicsManager Instance;
    public Animatronics fredrik;
    public Animatronics benny;

    void Awake()
    {
        Instance = this;
    }
}
