using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContagionExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestoryWhenHit());
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Enemy"){
            HealthManager enemyHealth = other.gameObject.GetComponent<HealthManager>();
            enemyHealth.UpdateHealth(-20);
        }
    }

    private IEnumerator DestoryWhenHit(){
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
