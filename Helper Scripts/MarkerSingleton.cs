using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerSingleton : MonoBehaviour
{
    private static MarkerSingleton instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = GetComponent<MarkerSingleton>();
    }

    // Update is called once per frame
    void Update()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
 
        instance = this;
    }
}
