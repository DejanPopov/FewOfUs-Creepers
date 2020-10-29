using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    private Animator animator = null;
    private int horizontalHash = 0;
    private int verticalHash = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        horizontalHash = Animator.StringToHash("Horizontal");
        verticalHash = Animator.StringToHash("Vertical");
    }

    // Update is called once per frame
    void Update()
    {
        // Multiply for values are taken from Locomotion blend
        float xAxis = Input.GetAxis("Horizontal") * 2.32f;
        float yAxis = Input.GetAxis("Vertical") * 5.66f;

        //Seting values from inputs
        animator.SetFloat(horizontalHash, xAxis, 1.0f, Time.deltaTime);
        animator.SetFloat(verticalHash, yAxis, 1.0f, Time.deltaTime);
    }
}
