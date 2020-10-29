using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{

    public float Speed = 0.0f;

    //Speed variable needs to talk to animator
    private Animator controller = null;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        controller.SetFloat("Speed", Speed);
    }
}
