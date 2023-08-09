using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBuff : MonoBehaviour
{

    private GameObject target;
    private float speed;
    public void SetTower(GameObject tower){
        target = tower;
    }
    // Start is called before the first frame update
    void Start()
    {
        speed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null){
            Destroy(gameObject);
            return;
        }
        //Direction
        Vector3 dir = target.transform.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject == target){
            string name = target.transform.parent.name;
            if(name.Equals("Archer(Clone)")){
                target.GetComponent<ArcherManager>().DoBuff();
            } else if (name.Equals("Cannon(Clone)")){
                target.GetComponent<CannonManager>().DoBuff();
            } else if (name.Equals("Mage(Clone)")){
                target.GetComponent<MageManager>().DoBuff();
            } else if (name.Equals("Poison(Clone)")){
                target.GetComponent<PoisonManager>().DoBuff();
            }
            Destroy(gameObject);
        }
    }
}
