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
public abstract class AIStateMachine : MonoBehaviour
{
    
}
