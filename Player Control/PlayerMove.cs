using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float forwardSpeed;
    public float reverseSpeed;
    public float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Vertical") && Input.GetAxis("Vertical") > 0) //if pressing w
        {
            transform.Translate((Input.GetAxis("Vertical") * forwardSpeed) * Time.deltaTime, 0, 0);
        }

        if (Input.GetButton("Vertical") && Input.GetAxis("Vertical") < 0) //if pressing s
        {
            transform.Translate((Input.GetAxis("Vertical") * forwardSpeed) * Time.deltaTime, 0, 0);
        }

        if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0) //if pressing a
        {
            transform.Rotate(0, (Input.GetAxis("Horizontal") * turnSpeed) * Time.deltaTime, 0);
        }

        if (Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0) //if pressing d
        {
            transform.Rotate(0, (Input.GetAxis("Horizontal") * turnSpeed) * Time.deltaTime, 0);
        }

        transform.position = new Vector3(transform.position.x, 0, transform.position.z); //set the players y position to 0 after every frame to make sure the player cannot go underground or fly away

    }
}
