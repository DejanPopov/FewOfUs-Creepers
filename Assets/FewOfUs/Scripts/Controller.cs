using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    private Animator animator = null;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Multiply for values are taken from Locomotion blend
        float xAxis = Input.GetAxis("Horizontal") * 2.32f;
        float yAxis = Input.GetAxis("Vertical") * 5.66f;

        //Seting values from inputs
        animator.SetFloat("Horizontal", xAxis, 1.0f, Time.deltaTime);
        animator.SetFloat("Vertical", yAxis, 1.0f, Time.deltaTime);
    }
}
