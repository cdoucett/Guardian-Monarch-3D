using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private GameObject cellIndicator, archer, cannon, mage, poison;
    private int range, archerCost, cannonCost, mageCost, poisonCost;

    private void Start(){
        range = 6;
        archerCost = 150;
        cannonCost = 300;
        mageCost = 500;
        poisonCost = 700;
    }

    private void Update(){
        //Showing mouse location
        Vector3 mousePosition = playerCamera.forward * range + playerCamera.position;
        if(mousePosition.y <= 0){
            
            cellIndicator.SetActive(true);

            //Convert the world to grid/cell based on where the mouse is
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);

            //Convert grid position to the transform of the indicator
            if(GUIManager.instance.GetTowerSelected() == "Archer" || GUIManager.instance.GetTowerSelected() == "Cannon"){
                cellIndicator.transform.localScale = new Vector3(2.95f, 0.25f, 2.95f);
                cellIndicator.transform.position = grid.CellToWorld(gridPosition);
            } else {
                cellIndicator.transform.localScale = new Vector3(3.95f, 0.25f, 3.95f);
                cellIndicator.transform.position = grid.CellToWorld(gridPosition);
                Vector3 newPosition = cellIndicator.transform.position + new Vector3(0.5f, 0, 0.5f);
                cellIndicator.transform.position = newPosition;
            }
            //Building
            if (Input.GetKeyDown(KeyCode.B)){
                if(GUIManager.instance.GetTowerSelected() == "Archer"){
                    if (GUIManager.instance.CheckMoney(archerCost) && GameManager.instance.placeable.isPlaceable()){
                        GUIManager.instance.ChangeMoney(-archerCost);
                        Instantiate(archer, cellIndicator.transform.position, Quaternion.identity);
                    }
                } else if (GUIManager.instance.GetTowerSelected() == "Cannon"){
                    if (GUIManager.instance.CheckMoney(cannonCost) && GameManager.instance.placeable.isPlaceable()){
                        GUIManager.instance.ChangeMoney(-cannonCost);
                        Instantiate(cannon, cellIndicator.transform.position, Quaternion.identity);
                    }
                } else if (GUIManager.instance.GetTowerSelected() == "Mage"){
                    if (GUIManager.instance.CheckMoney(mageCost) && GameManager.instance.placeable.isPlaceable()){
                        GUIManager.instance.ChangeMoney(-mageCost);
                        Instantiate(mage, cellIndicator.transform.position, Quaternion.identity);
                    }
                } else if (GUIManager.instance.GetTowerSelected() == "Poison"){
                    if (GUIManager.instance.CheckMoney(poisonCost) && GameManager.instance.placeable.isPlaceable()){
                        GUIManager.instance.ChangeMoney(-poisonCost);
                        Instantiate(poison, cellIndicator.transform.position, Quaternion.identity);
                    }
                }
            }
        } else {
            GameManager.instance.placeable.setColor(true);
            cellIndicator.SetActive(false);
        }
        
    }

}
