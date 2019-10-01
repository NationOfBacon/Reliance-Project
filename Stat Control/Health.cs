using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health = 5;
    public int maxHealth = 5;
    private int modifiedMaxHealth;
    public int shield = 0;
    public int maxShield = 3;
    public TextMeshProUGUI HP;
    public Slider healthSlider;
    public Canvas enemyHealthCanvas;
    public FloatingDamage fDMG;
    public Slider shieldSlider;
    public bool isEnemy = false;
    public bool autoHeal = false;
    private bool inHealCycle = false;
    private bool botsCalled = false;

    public bool objHealth = false; //use objHealth to determine if the gameobject using this script is an objective or something else

    private GameManager gameMgr;
    private DefenderStat dStat;
    private RectTransform stateNamesParent;

    private void Awake()
    {
        gameMgr = GameObject.Find("Persistent Object").GetComponent<GameManager>();
        stateNamesParent = GameObject.Find("RunningUI/Horizontal Group States").GetComponent<RectTransform>();
        fDMG = GetComponentInChildren<FloatingDamage>();

        if(isEnemy)
        {
            enemyHealthCanvas = GetComponentInChildren<Canvas>();
            enemyHealthCanvas.enabled = false;
        }

        dStat = GetComponent<DefenderStat>();

        if (!isEnemy && !objHealth)
            shieldSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if(autoHeal == true && health < maxHealth && inHealCycle == false)
        {
            Debug.Log("Auto healing started");
            StartCoroutine(PassiveHeal());
        }

        if(health < maxHealth && botsCalled == false && objHealth == true) //if the health of the obj is lowered, send for help
        {
            SendEnemies();
            botsCalled = true;
        }
    }

    public void SetShieldActive()
    {
        shieldSlider.gameObject.SetActive(true);
        Debug.Log("Set shields to active");
        stateNamesParent.anchoredPosition = new Vector2(19, 190);
    }

    public void HealthReduce() //called when the bot takes damage to update UI and health values
    {
        if(!isEnemy && !objHealth)
        {
            if(dStat.statEnabled && dStat != null)
            {
                if (dStat.OnTakeHitReturn() == false)
                {
                    if (shield == 0)
                    {
                        health--;

                        if (!isEnemy)
                        {
                            fDMG.SpawnDamageNumber(1);
                            healthSlider.value--;
                            HP.text = health + "/" + maxHealth;


                            if (health <= 0)
                            {
                                HP.text = "Dead";
                            }
                        }
                    }
                    else
                    {
                        ShieldReduce();
                    }
                }
                else
                {
                    ShieldIncrease();
                }
            }
            else
            {
                health--;

                if (!isEnemy)
                {
                    fDMG.SpawnDamageNumber(1);
                    healthSlider.value--;
                    HP.text = health + "/" + maxHealth;


                    if (health <= 0)
                    {
                        HP.text = "Dead";
                    }
                }
                else
                {
                    EnemyReduceHealthSliderValue();
                }
            }
        }
        else
        {
            health--;
            EnemyReduceHealthSliderValue();
        }

    }

    public void HealthReduce(int critAmt) //called from attack states when bot scores a crit
    {
        if(!isEnemy && !objHealth)
        {
            if(dStat.statEnabled && dStat != null)
            {
                if (dStat.OnTakeHitReturn() == false)
                {
                    if (shield >= critAmt) //if there is enough shield to negate all damage, just reduce the shield
                    {
                        ShieldReduce(critAmt);
                    }
                    else if (shield >= 1) //if there is only enough shield to negate some of the damage, reduce the shield all the way and send the rest of the damage to the HP
                    {
                        critAmt -= shield;
                        ShieldReduce(critAmt);

                        health -= critAmt;

                        if (!isEnemy)
                        {
                            fDMG.SpawnDamageNumber(critAmt);
                            healthSlider.value -= critAmt;
                            HP.text = health + "/" + maxHealth;


                            if (health <= 0)
                            {
                                HP.text = "Dead";
                            }
                        }
                        else
                        {
                            EnemyReduceHealthSliderValue();
                        }
                    }
                    else if (shield == 0)
                    {
                        health -= critAmt;

                        if (!isEnemy)
                        {
                            fDMG.SpawnDamageNumber(critAmt);
                            healthSlider.value -= critAmt;
                            HP.text = health + "/" + maxHealth;


                            if (health <= 0)
                            {
                                HP.text = "Dead";
                            }
                        }
                        else
                        {
                            EnemyReduceHealthSliderValue();
                        }
                    }
                }
                else
                {
                    ShieldIncrease();
                }
            }
            else
            {
                health -= critAmt;

                if (!isEnemy)
                {
                    fDMG.SpawnDamageNumber(critAmt);
                    healthSlider.value -= critAmt;
                    HP.text = health + "/" + maxHealth;


                    if (health <= 0)
                    {
                        HP.text = "Dead";
                    }
                }
                else
                {
                    EnemyReduceHealthSliderValue();
                }
            }


        }
        else
        {
            health -= critAmt;
            EnemyReduceHealthSliderValue();
        }
    }

    public void ShieldReduce()
    {
        shield--;
        if (shield <= 0)
            shield = 0;

        if (!isEnemy)
        {
            shieldSlider.value--;
        }
    }

    public void ShieldReduce(int damageAmt)
    {
        shield -= damageAmt;
        if (shield <= 0)
            shield = 0;

        if (!isEnemy)
        {
            shieldSlider.value -= damageAmt;
        }
    }

    public void HealthIncrease() //called when the bot is being healed
    {
        health++;
        if (health > maxHealth)
            health = maxHealth;

        if (!isEnemy)
        {
            healthSlider.value++;
            HP.text = health + "/" + maxHealth;
        }
    }

    public void ShieldIncrease()
    {
        shield++;
        if (shield > maxShield)
            shield = maxShield;

        if(!isEnemy)
        {
            shieldSlider.value++;
        }
    }

    public void HealthUpdate(int strMod, bool modifyCurrentHealth) //called when factoring in bot stats to increase max hp
    {
        modifiedMaxHealth = maxHealth + strMod;

        if(modifyCurrentHealth)
            health = modifiedMaxHealth;

        HP.text = health + "/" + modifiedMaxHealth;
        healthSlider.maxValue = modifiedMaxHealth;
        healthSlider.value = health;
    }

    public IEnumerator PassiveHeal()
    {
        inHealCycle = true;

        while (autoHeal && health < maxHealth) //when the auto heal bool is true, increment the bots health by 1 every 10 seconds
        {
            float waitTime = 10;
            int regenDelayReduction = 0;
            int regenValue = GetComponent<BotStats>().SendRegenValue();

            for (int i = 0; i < regenValue; i += 5) //for every five points of the regenerator value, reduce the delay by 1 second
            {
                regenDelayReduction += 1;
            }
            waitTime -= regenDelayReduction;

            HealthIncrease();
            yield return new WaitForSeconds(waitTime);
        }

        inHealCycle = false;
    }

    public void SendEnemies() //when the capture rate starts to go up, this will be called to send enemies to the objective
    {
        for (int i = 0; i < gameMgr.allEnemies.Count; i++)
        {
            if (!gameMgr.allEnemies[i].GetComponent<EnemyAIMachine>().defender)
                gameMgr.allEnemies[i].GetComponent<EnemyAIMachine>().MoveToObjective();
        }
    }

    public void EnemyReduceHealthSliderValue()
    {
        GetComponentInChildren<InformantGetEnemyHP>().ReduceHealth();
    }
}
