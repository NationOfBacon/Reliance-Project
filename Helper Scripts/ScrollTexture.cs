using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float scrollX = 0.5f;
    public float scrollY = 0.5f;

    void Update()
    {
        float offSetX = Time.time * scrollX;
        float offSetY = Time.time * scrollY;

        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offSetX, offSetY);
    }
}
