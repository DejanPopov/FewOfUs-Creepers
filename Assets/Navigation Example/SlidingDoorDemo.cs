using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Door states enumerator
public enum DoorState
{
    Open,
    Animating,
    Closed
}

public class SlidingDoorDemo : MonoBehaviour
{
    public float SlidingDistance = 4.0f;
    public float Duration = 1.5f;
    public AnimationCurve JumpCurve = new AnimationCurve();

    private Transform transform = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
