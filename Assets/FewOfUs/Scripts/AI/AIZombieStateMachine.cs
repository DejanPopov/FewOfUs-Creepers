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
    [Range(0, 100)]
    int health = 100;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float intelligence = 0.5f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float satisfaction = 1.0f;
}
