using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombieStateMachine : AIStateMachine
{
    [SerializeField]
    [Range(10.0f, 360.0f)]
    float fov = 50.0f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float sight = 0.5f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float hearing = 1.0f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float agression = 1.0f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float health = 1.0f;
}
