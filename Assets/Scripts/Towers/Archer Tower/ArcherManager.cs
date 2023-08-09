using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManager : MonoBehaviour
{
    [SerializeField] private GameObject tower;

    [SerializeField] private GameObject arrow;

    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer line;

    [SerializeField] private GameObject radiusRange;


    private float damage, attackSpeed, range;
    private WaitForSeconds searchTime;
    private Transform target;
    private bool canBleed, bombArrows, buffed;
    public bool path;
    public int upgradeCount;
    void Start()
    {
        damage = 25f;
        attackSpeed = 2f;
        range = 10f;
        searchTime = new WaitForSeconds(1f);
        target = null;

        StartCoroutine(FindTarget());
    }

    private IEnumerator FindTarget(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float firstDistance = Mathf.Infinity;
        GameObject enemyToAttack = null;

        foreach(GameObject enemy in enemies){
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            float enemyPositionToLine = enemy.transform.position.x;
            if (firstDistance > enemyPositionToLine && distanceToEnemy <= range){
                firstDistance = enemyPositionToLine;
                enemyToAttack = enemy;
            }
        
        }
        if(enemyToAttack != null){
            target = enemyToAttack.transform;
            Shoot();
            yield return new WaitForSeconds(attackSpeed);
        } else {
            target = null;
            yield return searchTime;
        }
        StartCoroutine(FindTarget());
    }

    private void Shoot(){
        //yield return new WaitForSeconds(attackSpeed);
        if(target != null){
            GameObject arrowInstance = Instantiate(arrow, firePoint.position, firePoint.rotation);
            arrowScript arrowComponent = arrowInstance.GetComponent<arrowScript>();
            if(arrowComponent != null){
                arrowComponent.setTarget(target);
                arrowComponent.setUpgrades(canBleed, bombArrows);
                arrowComponent.setDamage(damage);
            }
        }
    }

    public void Upgrade(){
        upgradeCount++;
        GUIManager.instance.SetUpgradeTower(gameObject);
        if(upgradeCount == 1){
            range += 5f;
            damage += 5f;
            attackSpeed /= 1.1f;
        } else if (upgradeCount == 2){
            damage += 15f;
        } else if (upgradeCount == 3){
            canBleed = true;
        }
    }

    public void Upgrade(string path_Txt){
        upgradeCount++;
        GUIManager.instance.SetUpgradeTower(gameObject);
        if(path_Txt.Equals("Sniper Arrows")){
            print("Sniper Path chosen");
            range += 10f;
            damage += 20f;
        } else {
            print("Bomb Path chosen");
            bombArrows = true;
        }
    }

    public void DoBuff(){
        if(!buffed){
            buffed = true;
            float oldDamage = damage;
            damage *= 1.25f;
            float oldAttackSpeed = attackSpeed;
            attackSpeed /= 1.25f;
            float oldRange = range;
            range *= 1.25f;
            StartCoroutine(EndBuff(oldDamage, oldAttackSpeed, oldRange));
        }
    }

    private IEnumerator EndBuff(float dmg, float atkspd, float rng){
        yield return new WaitForSeconds(4.9f);
        damage = dmg;
        attackSpeed = atkspd;
        range = rng;
        buffed = false;
    }
    
    public void ShowRange(bool toggle){
        radiusRange.transform.localScale = new Vector3(1,.001f,1)*range*2;
        radiusRange.SetActive(toggle);
    }

    public void Destroy(){
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
