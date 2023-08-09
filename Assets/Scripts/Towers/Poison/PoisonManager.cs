using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonManager : MonoBehaviour
{

    [SerializeField] private GameObject potion;

    [SerializeField] private GameObject potionBuff;

    [SerializeField] private Transform firePoint;

    [SerializeField] private GameObject radiusRange;


    private float damage, attackSpeed, range, slow, lastingEffect, freezeTime;
    private int poisonTicks;
    private WaitForSeconds searchTime;
    private Transform target;
    private GameObject  tower, targetTower;
    private string towerName;
    private bool buffPotions, rainbow, cooldown, buffed;
    public bool path;

    public int upgradeCount;
    void Start()
    {
        //Damage
        damage = 20f;
        slow = .6f;
        lastingEffect = 0.2f;
        freezeTime = 0.5f;
        poisonTicks = 4;

        attackSpeed = 2f;
        range = 10f;

        searchTime = new WaitForSeconds(1f);
        target = null;
        tower = null;
        cooldown = false;

        StartCoroutine(FindTarget());
    }

    private IEnumerator FindTarget(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject enemyToAttack = null;
        
        foreach(GameObject enemy in enemies){
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (shortestDistance > distanceToEnemy){
                shortestDistance = distanceToEnemy;
                enemyToAttack = enemy;
            }
        }

        if(enemyToAttack != null && shortestDistance <= range){
            target = enemyToAttack.transform;
            Shoot();
            yield return new WaitForSeconds(attackSpeed);
        } else {
            target = null;
            yield return searchTime;
        }
        StartCoroutine(FindTarget());
    }

    private IEnumerator FindTower(){
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Obstruction");
        List<GameObject> actualTowers = new List<GameObject>();
        foreach(GameObject towerTemp in towers){
            towerName = towerTemp.transform.parent.name;
            if(towerName.Equals("Archer(Clone)") || towerName.Equals("Cannon(Clone)") || towerName.Equals("Mage(Clone)") || towerName.Equals("Poison(Clone)")){
                if(towerTemp != gameObject && Vector3.Distance(transform.position, towerTemp.transform.position) <= range){
                    actualTowers.Add(towerTemp);
                }
            }
        }
        if(actualTowers.Count != 0){
            tower = actualTowers[Random.Range(0, actualTowers.Count)];
        }
        if(tower != null){
            targetTower = tower;
        } else {
            targetTower = null;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(FindTower());
    }

    private void Update(){
        if(buffPotions){
            if(targetTower == null){
                cooldown = false;
            }
            else if (!cooldown){
                cooldown = true;
                StartCoroutine(ShootBuff());
            }
        }
    }

    public void Upgrade(){
        upgradeCount++;
        GUIManager.instance.SetUpgradeTower(gameObject);
        if(upgradeCount == 1){
            range += 10f;
            attackSpeed -= 1f;
        } else if (upgradeCount == 2){
            damage += 10f;
            slow = 0.5f;
            freezeTime = 1f;
            poisonTicks = 5;
        } else if (upgradeCount == 3){
            lastingEffect = 1.5f;
        }
    }

    public void Upgrade(string path){
        upgradeCount++;
        GUIManager.instance.SetUpgradeTower(gameObject);
        if(path.Equals("Buff potions")){
            buffPotions = true;
            StartCoroutine(FindTower());
        } else {
            rainbow = true;
        }
    }

    private void Shoot(){
        if(target != null){
            GameObject potionInstance = Instantiate(potion, firePoint.position, firePoint.rotation);
            PotionScript potionComponent = potionInstance.GetComponent<PotionScript>();
            if(potionComponent != null){
                potionComponent.SetTarget(target);
                potionComponent.SetDamage(damage, slow, freezeTime, poisonTicks, lastingEffect);
                potionComponent.SetType(rainbow);
            }
        }
    }

    private IEnumerator ShootBuff(){
        if(targetTower != null){
            GameObject buffInstance = Instantiate(potionBuff, firePoint.position, firePoint.rotation);
            PotionBuff buffComponent = buffInstance.GetComponent<PotionBuff>();
            if(buffComponent != null){
                buffComponent.SetTower(targetTower);
            }
            yield return new WaitForSeconds(5f);
            StartCoroutine(ShootBuff());
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
