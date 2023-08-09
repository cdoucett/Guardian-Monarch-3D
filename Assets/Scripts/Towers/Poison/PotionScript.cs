using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionScript : MonoBehaviour
{
    private Transform target;
    private float speed, timeToDestory, damage, slowTime, freezeDuration;
    private int posionTicks;
    [SerializeField] private Material potionDamage, potionPoison, potionSlow, potionFreeze, potionRainbow;
    private int type;
    private bool explosion, rainbowPotions;
    public void SetTarget(Transform enemy){
        target = enemy;
    }
    public void SetDamage(float dmg, float slow, float freezeDur, int posionTick, float time){
        damage = dmg;
        slowTime = slow;
        freezeDuration = freezeDur;
        posionTicks = posionTick;
        timeToDestory = time;
    }

    public void SetType(bool rainbow){
        rainbowPotions = rainbow;
    }

    void Start()
    {
        speed = 20f;
        timeToDestory = 3f;
        explosion = false;
        //Roll for potion type
        if(rainbowPotions){
            Renderer rend = this.GetComponent<Renderer>();
            rend.material = potionRainbow;
        } else {
            type = Random.Range(0,4);
            if(type == 0){
                //Damage
                Renderer rend = this.GetComponent<Renderer>(); 
                rend.material = potionDamage;
            } else if (type == 1){
                //Slow
                Renderer rend = this.GetComponent<Renderer>(); 
                rend.material = potionSlow;
            } else if (type == 2){
                //Freeze
                Renderer rend = this.GetComponent<Renderer>(); 
                rend.material = potionFreeze;
            } else {
                //Poison
                Renderer rend = this.GetComponent<Renderer>();
                damage /= 2;
                rend.material = potionPoison;
            }
        }
        StartCoroutine(DestoryWhenMiss());
    }


    // Update is called once per frame
    void Update()
    {
        if(target == null){
            StartCoroutine(DestoryWhenHit());
            return;
        }
        //Direction
        Vector3 dir = target.position - transform.position;
        if(!explosion){
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Enemy"){
            explosion = true;
            HealthManager enemyHealth = other.gameObject.GetComponent<HealthManager>();
            transform.localScale = new Vector3(2,2,2);
            if(rainbowPotions){
                enemyHealth.UpdateHealth(-damage);
                enemyHealth.UpdateSpeed(slowTime,5f);
                enemyHealth.UpdateSpeed(0f,freezeDuration);
                enemyHealth.UpdateHealth(-damage, posionTicks);
            } else {
                if(type == 0){
                    //Damage
                    enemyHealth.UpdateHealth(-damage);
                } else if (type == 1){
                    //Slow
                    enemyHealth.UpdateSpeed(slowTime,5f);
                } else if (type == 2){
                    //Freeze
                    enemyHealth.UpdateSpeed(0f,freezeDuration);
                } else {
                    //Poison
                    enemyHealth.UpdateHealth(-damage, posionTicks);
                }
            }
            StartCoroutine(DestoryWhenHit());
        }
    }

    private IEnumerator DestoryWhenHit(){
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    private IEnumerator DestoryWhenMiss(){
        yield return new WaitForSeconds(timeToDestory);
        Destroy(gameObject);
    }

    
}