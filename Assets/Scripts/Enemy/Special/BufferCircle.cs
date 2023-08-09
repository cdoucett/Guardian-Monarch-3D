using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferCircle : MonoBehaviour
{
    [SerializeField] private GameObject radiusRange;
    private float range;
    // Start is called before the first frame update
    void Start()
    {
        range = 10f;
        radiusRange.transform.localScale = new Vector3 (1,0.0001f,1)*range*2;
        radiusRange.SetActive(true);
        StartCoroutine(BuffTarget());
    }

    private IEnumerator BuffTarget(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemies){
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (range >= distanceToEnemy){
                enemy.GetComponent<HealthManager>().UpdateHealth(0.1f);
                enemy.GetComponent<HealthManager>().UpdateSpeed(1.1f, true);
            }
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(BuffTarget());
    }

    private void Update(){
        radiusRange.transform.position = gameObject.transform.position;
    }
}
