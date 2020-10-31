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

    private int seeking = 0;
    private bool feeding = false;
    private bool crawling = false;
    private int attackType = 0;

    public float fovF { get { return fov; } }
    public float hearingH { get { return hearing; } }
    public float sightS { get { return sight; } }
    public bool crawlingS { get { return crawling; } }
    public float intelligenceI { get { return intelligence; } }
    public float satisfactionS { get { return satisfaction; } set { satisfaction = value; } }
    public float agressionA { get { return agression; } set { agression = value; } }
    public int healtH { get { return health; } set { health = value; } }
    public int attackTypeA { get { return attackType; } set { attackType = value; } }
    public bool feddingF { get { return feeding; } set { feeding = value; } }
    public int seekingS { get { return seeking; } set { seeking = value; } }
    public float speedS
    {
        get
        {
            return navAgent != null ? navAgent.speed : 0.0f;
        }
        set
        {
            if (navAgent != null)
            {
                navAgent.speed = value;
            }
        }
    }

}
