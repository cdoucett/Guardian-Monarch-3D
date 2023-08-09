using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLine : MonoBehaviour
{
    //[SerializeField] private LineRenderer line;
    [SerializeField] private GameObject enemyTransform;
    [SerializeField] private int healthDamage;

    private void Start()
    {
        //enemyTransform.transform.position = line.GetPosition(0);
        enemyTransform = this.gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        if(enemyTransform.transform.position.x <= -29.5f){
            GameManager.instance.player.UpdateHealth(-healthDamage);
            Destroy(enemyTransform);
            return;
        }
    }
}
