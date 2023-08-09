using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonBallScript : MonoBehaviour
{
    private Transform target;
    private float speed, timeToDestory, damage;
    private int pierce;
    [SerializeField] private Material cannonBallExplosion;
    private Vector3 explosionRadius;
    private bool explosion, doContagion;
    public void setTarget(Transform enemy){
        target = enemy;
    }
    void Start()
    {
        pierce = 4;
        speed = 50f;
        timeToDestory = 1.3f;

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
        if(other.gameObject.tag == "Enemy" && pierce > 0){
            explosion = true;
            //Instantiate(cannonBallExplosion, target.position, Quaternion.identity);
            Renderer rend = this.GetComponent<Renderer>(); 
            rend.material = cannonBallExplosion;
            transform.localScale = explosionRadius;
            HealthManager enemyHealth = other.gameObject.GetComponent<HealthManager>();
            if(pierce > 0){
                if(doContagion){
                enemyHealth.SetContagion();
                }
                enemyHealth.UpdateHealth(-damage);
                pierce--;
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
        print("Missed!");
        Destroy(gameObject);
    }

    public void setDamage(float dmg, Vector3 radius, bool contagion){
        damage = dmg;
        explosionRadius = radius;
        doContagion = contagion;
    }
}