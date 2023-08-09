using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSpawner : MonoBehaviour
{
    [SerializeField] private GameObject radiusRange;
    private float range;
    // Start is called before the first frame update
    void Start()
    {
        range = 10f;
        radiusRange.transform.localScale = new Vector3 (1,0.0001f,1)*range*2;
        gameObject.GetComponent<HealthManager>().SetSpeed(0.1f);
        StartCoroutine(SpawnTarget());
    }

    private IEnumerator SpawnTarget(){
        
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnTarget());
    }
}
