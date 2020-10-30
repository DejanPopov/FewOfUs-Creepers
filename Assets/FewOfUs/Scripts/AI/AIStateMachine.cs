using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//AI states enumeration
public enum AIStateType
{
    None,
    Idle,
    Alerted,
    Partol,
    Attack,
    Feeding,
    Pursuit,
    Dead
}
//AI Target
public enum AITargetType
{
   None,
   Waypoint,
   Visual_Player,
   Visual_Light,
   Visual_Food,
   Audio
}

public struct AITarget
{
    //This is type of target
    private AITargetType type;
    //Collider
    private Collider collider;
    //Position in the world
    private Vector3 position;
    //Distance from player
    private float distance;
    //Time the target last ping'd
    private float time;

    public AITargetType typeT { get { return type; } }
    public Collider colliderC { get { return collider; } }
    public Vector3 positionP { get { return position; } }
    public float distanceD { get { return distance; }  set {distance = value;}}
    public float timeT { get { return time; } }

    public void Set(AITargetType t, Collider c, Vector3 p, float d)
    {
        type = t;
        collider = c;
        position = p;
        distance = d;
        time = Time.time;
    }

    public void Clear()
    {
        type = AITargetType.None;
        collider = null;
        position = Vector3.zero;
        time = 0.0f;
        distance = Mathf.Infinity;
    }
}
public abstract class AIStateMachine : MonoBehaviour
{
    //Dictionary for state types
    //When zombie enters some state we will store it in Dictionary thus knowing its state
    private Dictionary<AIStateType, AIState> states = new Dictionary<AIStateType, AIState>();

    protected virtual void Start()
    {
        //Array for states
        AIState[] states = GetComponents<AIState>();
        //Loop states and put them ion Dictionary
        foreach  (AIState state in states)
        {
            if (state != null && states.ContainsKey(state.getStateType())) ;
            {
                states[state.GetStateType()] = state;
            }
        }
    }
}
 