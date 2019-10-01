using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPointAt : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //create a var to hold the raycast hit data
        RaycastHit hit;

        //create a ray from the screen to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if the ray hits something, do this
        if (Physics.Raycast(ray, out hit))
        {
            //create a vector3 var to hold the location of the hit
            Vector3 hitPos = hit.point;

            //create a quaternion to store the rotation for the turret to the mouse pos
            Quaternion hitRot = Quaternion.LookRotation(hitPos - transform.position);

            //set the rotation of the object
            transform.rotation = Quaternion.RotateTowards(transform.rotation, hitRot, Time.deltaTime * 50f);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
    }
}
