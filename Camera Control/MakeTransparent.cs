using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTransparent : MonoBehaviour
{
    private Renderer wall;
    public Material transparentMat;
    public Material originalMat;

    
    void Update() //when behind a wall that has been made transparent, the player cannot aim through the wall
    {
        RaycastHit hit;
        var camera = Camera.main.transform;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(camera.position, camera.forward * 200, Color.red);

        if(Physics.Raycast(ray, out hit))
        {
            if (wall != null)
            {
                if (wall.gameObject != hit.collider.gameObject)
                {
                    wall.material = originalMat;
                    wall = null;
                }
            }

            if (hit.collider.CompareTag("Wall"))
            {
                wall = hit.collider.gameObject.GetComponent<Renderer>();
                wall.material = transparentMat;
            }
            else
            {
                if(wall != null)
                {
                    wall.material = originalMat;
                    wall = null;
                }
            }
        }
    }
}
