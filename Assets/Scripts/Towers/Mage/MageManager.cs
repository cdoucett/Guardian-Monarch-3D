using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageManager : MonoBehaviour
{
    [SerializeField] private Transform crystal;
    [SerializeField] private GameObject beam;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject radiusRange;
    private float yRotation, yRotationSpeed;
    private float damage, range, chargeSpeed, maxTicks;
    private int targetMax;
    private bool infernalBeam = false, beamCreated = false, buffed;
    public bool path;
    private GameObject target = null;
    private WaitForSeconds searchTime;
    private List<GameObject> currentTargets;

    public int upgradeCount;
    // Start is called before the first frame update
    void Start()
    {
        //Range
        range = 15f;
        //Damage
        damage = 2f;

        //How many beams the mage shoots
        targetMax = 3;
        
        //Ramping speed and max ticks
        chargeSpeed = 1.5f;
        maxTicks = 3f;

        yRotation = 0;
        //How fast the crystal spins (based on targets)
        yRotationSpeed = 0.1f;

        searchTime = new WaitForSeconds(1f);
        currentTargets = new List<GameObject>();
        StartCoroutine(FindTarget());
    }

    // Update is called once per frame
    void Update()
    {
        //Crystal Rotation
        yRotation += yRotationSpeed;
        crystal.rotation = Quaternion.Euler(-90, yRotation, 0);
        if(currentTargets.Count == 0){
            yRotationSpeed = 0.05f;
        }
        else {
            yRotationSpeed = 0.2f * currentTargets.Count;
        }
    }
    private IEnumerator FindTarget(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(infernalBeam){
            float highestHealth = 0;
            if(target == null){
                beamCreated = false;
                foreach(GameObject enemy in enemies){
                    HealthManager health = enemy.GetComponent<HealthManager>();
                    if(health.GetHealth() > highestHealth) {
                        target = enemy;
                        highestHealth = health.GetHealth();
                    }
                }
            }
            if(!beamCreated && target != null){
                beamCreated = true;
                GameObject beamInstance = Instantiate(beam, firePoint.position, firePoint.rotation);
                beamScript beamComponent = beamInstance.GetComponent<beamScript>();
                if(beamComponent != null){
                    beamComponent.SetTarget(target.transform);
                    beamComponent.SetDamage(damage, chargeSpeed, maxTicks);
                    beamComponent.SetFirePoint(firePoint.position, range);
                }  
            }
        } else {
            foreach(GameObject enemy in enemies){
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if(enemy != null && distanceToEnemy <= range && currentTargets.Count < targetMax && !currentTargets.Contains(enemy)){
                    currentTargets.Add(enemy);
                    GameObject beamInstance = Instantiate(beam, firePoint.position, firePoint.rotation);
                    beamScript beamComponent = beamInstance.GetComponent<beamScript>();
                    if(beamComponent != null){
                        beamComponent.SetTarget(enemy.transform);
                        beamComponent.SetDamage(damage, chargeSpeed, maxTicks);
                        beamComponent.SetFirePoint(firePoint.position, range);
                    }  
                } else if ((enemy == null || distanceToEnemy > range) && currentTargets.Contains(enemy)){
                    currentTargets.Remove(enemy);
                }
            }
        }
        
        currentTargets.RemoveAll(s => s == null);
        currentTargets.TrimExcess();

        yield return searchTime;
        StartCoroutine(FindTarget());
    }

    public void Upgrade(){
        upgradeCount++;
        GUIManager.instance.SetUpgradeTower(gameObject);
        if(upgradeCount == 1){
            targetMax = 4;
        } else if (upgradeCount == 2){
            range += 3f;
            chargeSpeed -= 0.25f;
        } else if (upgradeCount == 3){
            maxTicks = 5;
        }
    }

    public void Upgrade(string path){
        upgradeCount++;
        GUIManager.instance.SetUpgradeTower(gameObject);
        if(path.Equals("Infernal Beam")){
            targetMax = 1;
            chargeSpeed -= 0.75f;
            maxTicks = 9;
            infernalBeam = true;
        } else {
            targetMax = int.MaxValue;
            maxTicks = 3;
        }
    }

    public void DoBuff(){
        if(!buffed){
            buffed = true;
            float oldDamage = damage;
            damage *= 1.25f;
            float oldChargeSpeed = chargeSpeed;
            chargeSpeed /= 1.25f;
            float oldRange = range;
            range *= 1.25f;
            StartCoroutine(EndBuff(oldDamage, oldChargeSpeed, oldRange));
        }
    }

    private IEnumerator EndBuff(float dmg, float chrgspd, float rng){
        yield return new WaitForSeconds(4.9f);
        damage = dmg;
        chargeSpeed = chrgspd;
        range = rng;
        buffed = false;
    }

    public void ShowRange(bool toggle){
        radiusRange.transform.localScale = new Vector3(1,.001f,1)*range/2.25f;
        radiusRange.SetActive(toggle);
    }
    public void Destroy(){
        Destroy(gameObject);
    }

    private void RemoveTarget(GameObject target){
        currentTargets.Remove(target);
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
