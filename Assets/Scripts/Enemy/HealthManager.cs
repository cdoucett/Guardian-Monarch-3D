using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private float maxSpeed, maxHealth;
    [SerializeField] private float health = 100f;
    [SerializeField] private int moneyOnDestroy = 25;

    private bool poisoned, bleeding, contagion, speedBuffed; 
    [SerializeField] private GameObject enemy;
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private GameObject contagionExplosion;
    
    void Start()
    {
        maxHealth = health;
        maxSpeed = enemyAnim.speed;
        contagion = false;
        speedBuffed = false;
    }

    public void UpdateHealth(float value){
        health += value;
        if(health <= 0){
            Destroy(enemy);
            return;
        }
    }

    public void UpdateHealth(float value, int ticks){
        if(!poisoned){
            poisoned = true;
            StartCoroutine(PosionDamage(value, ticks));
        }
        if(health <= 0){
            Destroy(enemy);
            return;
        }
    }

    public void UpdateSpeed(float value){
        enemyAnim.speed *= value;
        maxSpeed = enemyAnim.speed;
    }

    public void UpdateSpeed(float value, bool doOnce){
        if(doOnce && !speedBuffed){
            speedBuffed = true;
            enemyAnim.speed *= value;
            maxSpeed = enemyAnim.speed;
        }
    }


    public void UpdateSpeed(float value, float time){
        enemyAnim.speed *= value;
        StartCoroutine(NormalSpeed(time));
    }

    public void SetSpeed(float value){
        enemyAnim.speed = value;
    }

    public void SetContagion(){
        contagion = true;
    }

    public float GetHealth(){
        return health;
    }

    public float GetMaxHealth(){
        return maxHealth;
    }

    public void ScaleHealth(float healthScaler){
        health *= healthScaler;
        if(health > maxHealth){
            maxHealth = health;
        }
    }

    private IEnumerator NormalSpeed(float time){
        yield return new WaitForSeconds(time);
        enemyAnim.speed = maxSpeed;
    }


    private IEnumerator PosionDamage(float damage, int ticks){
        health += damage;
        if(ticks > 0){
            yield return new WaitForSeconds(3f);
            StartCoroutine(PosionDamage(damage, ticks-1));
        } else {
            poisoned = false;
        }
    }

    private IEnumerator BleedDamage(float damage, int ticks){
        health += damage;
        if(ticks > 0){
            yield return new WaitForSeconds(3f);
            StartCoroutine(BleedDamage(damage, ticks-1));
        } else {
            bleeding = false;
        }
    }

    public void UpdateHealth(float value, int ticks, bool bleed){
        if(!bleeding){
            bleeding = true;
            StartCoroutine(BleedDamage(value, ticks));
        }
    }

    void OnDestroy()
    {
        GameManager.instance.enemyCount--;
        GUIManager.instance.ChangeMoney(moneyOnDestroy);
        if(contagion){
            Instantiate(contagionExplosion, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}