using UnityEngine;

public class FredrikMovement : MonoBehaviour
{
    public Transform stagePoint;
    public Transform hallwayPoint;

    //public RectTransform stagePoint;
    //public RectTransform hallwayPoint;

    public float moveDelay = 10f;

    void Start()
    {
        Invoke(nameof(MoveToHallway), moveDelay); 
    }
    void MoveToHallway()
    {
        transform.position = hallwayPoint.position;

        //GetComponent<RectTransform>().anchoredPosition = hallwayPoint.anchoredPosition;
    }
}
