using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beamScript : MonoBehaviour
{
    private Transform target;
    [SerializeField] private LineRenderer beam;
    private Vector3 firePoint;
    private float range, timeToRamp, damage, rampingTicks, rampMax;
    public void SetTarget(Transform enemy){
        target = enemy;
    }

    public void SetDamage(float dmg, float chargeSpeed, float maxTicks){
        damage = dmg;
        timeToRamp = chargeSpeed;
        rampMax = maxTicks;
    }

    public void SetFirePoint(Vector3 point, float towerRange){
        firePoint = point;
        range = towerRange;
        beam.SetPosition(0, firePoint);
    }

    void Start()
    {
        StartCoroutine(DamageRamping());
    }


    // Update is called once per frame
    void Update()
    {
        if(target == null){
            Destroy(gameObject);
            return;
        }
        float distanceToEnemy = Vector3.Distance(firePoint, target.transform.position);
        //Direction
        if(distanceToEnemy > range){
            Destroy(gameObject);
            return;
        }
        beam.SetPosition(1, target.position);
    }

    private IEnumerator DamageRamping(){
        yield return new WaitForSeconds(timeToRamp);
        if(target != null){
            HealthManager enemyHealth = target.gameObject.GetComponent<HealthManager>();
            enemyHealth.UpdateHealth(-damage);
        }
        if(rampingTicks == rampMax){
            print("Damage maxed! " + damage);
        } else {
            damage *= 2;
            rampingTicks++;
        }
        StartCoroutine(DamageRamping());
    }
}