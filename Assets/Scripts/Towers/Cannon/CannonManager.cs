using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    [SerializeField] private Transform rotator, cannon;
    [SerializeField] private GameObject cannonBall;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject radiusRange;
    private float yRotation, yRotationSpeed;
    private float damage, attackSpeed, range;
    private WaitForSeconds searchTime;
    private Vector3 explosionRadius;
    private Transform target;
    private bool contagion = false, buffed;

    public bool path;

    public int upgradeCount;
    void Start()
    {
        damage = 25f;
        attackSpeed = 2f;
        range = 8f;
        explosionRadius = new Vector3(4,4,4);
        searchTime = new WaitForSeconds(1f);
        target = null;

        StartCoroutine(FindTarget());
    }

    // Update is called once per frame
    void Update()
    {
        FaceEnemy();
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

    private void Shoot(){
        if(target != null){
            GameObject ballInstance = Instantiate(cannonBall, firePoint.position, firePoint.rotation);
            cannonBallScript ballComponent = ballInstance.GetComponent<cannonBallScript>();
            if(ballComponent != null){
                ballComponent.setTarget(target);
                ballComponent.setDamage(damage, explosionRadius, contagion);
            }
        }
    }

    public void Upgrade(){
        upgradeCount++;
        GUIManager.instance.SetUpgradeTower(gameObject);
        if(upgradeCount == 1){
            explosionRadius = new Vector3(6,6,6);
        } else if (upgradeCount == 2){
            range += 4f;
        } else if (upgradeCount == 3){
            attackSpeed -= 0.3f;
            explosionRadius = new Vector3(8,8,8);
        } 
    }

    public void Upgrade(string path){
        upgradeCount++;
        GUIManager.instance.SetUpgradeTower(gameObject);
        if(path.Equals("Huge Bombs")){
            explosionRadius = new Vector3(12,12,12);
        } else {
            contagion = true;
        }
    }

    //Cannon rotator
    private void FaceEnemy(){
        if(target == null){
            return;
        }
        
        Vector3 dirVector = target.position - rotator.position;
        if(dirVector.magnitude <= range){
            Quaternion targetRotation = Quaternion.LookRotation(dirVector);
            rotator.rotation = targetRotation;
            Vector3 yAngle = rotator.rotation.eulerAngles;
            rotator.rotation = Quaternion.Euler(0, yAngle.y, 0);
            yAngle.x = Mathf.Clamp(yAngle.x,-5,15);
            cannon.rotation = Quaternion.Euler(yAngle.x,yAngle.y,0);
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
        radiusRange.transform.localScale = new Vector3(1,.001f,1)*range*6.7f;
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
