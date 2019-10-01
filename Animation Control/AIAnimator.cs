using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimator : MonoBehaviour
{
    private Animator animControl;
    private Rigidbody RB;
    // Start is called before the first frame update
    void Start()
    {
        animControl = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RB.velocity.magnitude > 0)
        {
            animControl.SetBool("Walk_Anim", true);
        }
    }
}
