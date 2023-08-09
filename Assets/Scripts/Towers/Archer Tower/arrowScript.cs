using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowScript : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Material bombArrowExplosion;
    private float speed, timeToDestory, damage;
    private bool bleed, bombArrows, explosion;
    public void setTarget(Transform enemy){
        target = enemy;
    }
    public void setUpgrades(bool canBleed, bool bombPath){
        bleed = canBleed;
        bombArrows = bombPath;
    }
    void Start()
    {
        speed = 50f;
        timeToDestory = 8f;
        explosion = false;

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
            HealthManager enemyHealth = other.gameObject.GetComponent<HealthManager>();
            if(bleed){
                enemyHealth.UpdateHealth(-damage/4, 3, true);
            }
            if(bombArrows){
                explosion = true;
                Renderer rend = this.GetComponent<Renderer>(); 
                rend.material = bombArrowExplosion;
                transform.localScale = new Vector3(2,2,2);
                enemyHealth.UpdateHealth(-damage);
                StartCoroutine(DestoryWhenHit());
            } else {
                enemyHealth.UpdateHealth(-damage);
                Destroy(gameObject);
            }
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

    public void setDamage(float dmg){
        damage = dmg;
    }
}