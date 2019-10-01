using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureInfo : MonoBehaviour
{
    public string displayName = "Structure";

    private void Awake()
    {
        transform.parent = null;
    }
}
