using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstruction : MonoBehaviour
{
    [SerializeField] private Material GridIndicator;

    private bool placeable;

    private void Awake(){
        GridIndicator.color = Color.red;
        placeable = false;
    }

    private void Start(){
        GridIndicator.color = Color.red;
        GameManager.instance.placeable = this;
        placeable = false;
    }

    private void OnTriggerStay (Collider other){
        if(other.gameObject.tag == "Obstruction"){
            GridIndicator.color = Color.red;
            placeable = false;
        }
    }

    private void OnTriggerExit (Collider other){
        if(other.gameObject.tag == "Obstruction"){
            GridIndicator.color = Color.green;
            placeable = true;
        }
    }

    public void setColor(bool color){
        if(color){
            GridIndicator.color = Color.green;
            placeable = true;
        } else {
            GridIndicator.color = Color.red;
            placeable = false;
        }
        
    }

    public bool isPlaceable(){
        return placeable;
    }
}
