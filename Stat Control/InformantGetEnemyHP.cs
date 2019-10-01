using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformantGetEnemyHP : MonoBehaviour
{
    private int health;
    private int maxHealth;
    private Slider healthSlider;

    private void Awake()
    {
        maxHealth = gameObject.transform.parent.GetComponent<Health>().maxHealth;
        health = gameObject.transform.parent.GetComponent<Health>().health;
        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    public void ReduceHealth()
    {
        health = gameObject.transform.parent.GetComponent<Health>().health;
        healthSlider.value = health;
    }
}
