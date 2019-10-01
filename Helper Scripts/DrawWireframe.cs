using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWireframe : MonoBehaviour
{
    public bool enableWireframe;
    public float drawRange;
    public Color frameColor;

    public void OnDrawGizmos()
    {
        if (enableWireframe)
        {
            Gizmos.color = frameColor;
            Gizmos.DrawWireSphere(transform.position, drawRange);
        }
    }
}
