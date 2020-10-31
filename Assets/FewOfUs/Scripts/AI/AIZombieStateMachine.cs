using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombieStateMachine : AIStateMachine
{
    [SerializeField]
    [Range(10.0f, 360.0f)]
    float fov = 50.0f;

    [SerializeField]
    [Range(10.0f, 360.0f)]
    float sight = 50.0f;

    [SerializeField]
    [Range(10.0f, 360.0f)]
    float hearing = 50.0f;
}
