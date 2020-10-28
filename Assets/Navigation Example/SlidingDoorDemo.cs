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

    //Door states
    private Transform transformT = null;
    private Vector3 openPos = Vector3.zero;
    private Vector3 closedPos = Vector3.zero;
    private DoorState doorState = DoorState.Closed;


    // Start is called before the first frame update
    void Start()
    {
        transformT = transform;
        closedPos = transformT.position;
        //Vector that will be pointing right (open door to that side)
        openPos = closedPos + (transform.right * SlidingDistance);
    }

    // Update is called once per frame
    void Update()
    {
        //Door will open with Space bar key
        if (Input.GetKeyDown(KeyCode.Space) && doorState != DoorState.Animating)
        {
           // StartCoroutine(AnimateDoor(doorState == DoorState.Closed)? DoorState.Open : DoorState.Closed);
        }
    }

    IEnumerator AnimateDoor(DoorState newState)
    {
        doorState = DoorState.Animating;
        //Timer
        float time = 0.0f;
        //Star and end position of interpolation
        Vector3 startPos = (newState == DoorState.Open) ? closedPos : openPos;
        Vector3 endPos = (newState == DoorState.Open) ? openPos : closedPos;

        while (time <= Duration)
        {
            float t = time / Duration;
            transformT.position = Vector3.Lerp(startPos, endPos, JumpCurve.Evaluate(t));
            time += Time.deltaTime;
            yield return null;
        }

        //Check to snap object at end position
        transformT.position = endPos;
        //This will be new state for door
        doorState = newState;
    }
}
