using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformantEnemyHealthLookAt : MonoBehaviour
{
    private RectTransform rectT;

    private void Awake()
    {
        rectT = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectT.eulerAngles = new Vector3(40, 0, 0);
    }
}
