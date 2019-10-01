using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton instance { get; private set; } //since this is static, it is the same for every instance of this script

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); //if a new instance of this script is created while one is already around, the new one will destroy itself
        }
    }
}
