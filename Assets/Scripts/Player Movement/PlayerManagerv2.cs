using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerv2 : MonoBehaviour
{
    public Transform playerDirection;
    public Transform playerTransform;

    [SerializeField] Transform playerCamera;

    Ray ray;

    private Rigidbody body;
    private float playerSpeed;

    private float stamina, maxHealth, currentHealth;
    private bool tired, spam, inMenu;
    private float directUp, directDown, sensitivity;


    void Start()
    {
        playerSpeed = 4f;
        
        //Sprint
        stamina = 100;
        tired = false;
        spam = true;

        //Rigit body
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
        body.drag = 5f;

        //Health
        currentHealth = 100;
        maxHealth = 100;
        UpdateHealth(0);

        GameManager.instance.player = this;
    }

    // Update is called once per frame
    private void Update()
    {
        if(currentHealth <= 0){
            GUIManager.instance.SwitchDisplayLose();
        }
        //Get direction
        Direction();

        //Movement
        PlayerMovement();

        Vector3 cameraPos = new Vector3(playerCamera.transform.rotation.y,playerCamera.transform.rotation.x,playerCamera.transform.rotation.z);
        //Tower Looking
        //0 = left
        if(Input.GetMouseButtonDown(0)){
            ray = new Ray(playerCamera.position, playerCamera.forward);
            if(Physics.Raycast(ray, out RaycastHit hit, 4)){
                if(hit.collider.gameObject.name.Equals("Obstruction")){
                    GameObject towerInfo = hit.collider.gameObject;
                    if(GUIManager.instance.GetToggle()){
                        GUIManager.instance.SetUpgradeTower(towerInfo);
                        GUIManager.instance.SwitchDisplay();
                    }
                }
            }
        }
        Debug.DrawRay(playerCamera.position, playerCamera.forward, Color.red);
    }

    #region Player Movement

    private void Direction(){
        directUp = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        directDown = Input.GetAxisRaw("Vertical") * Time.deltaTime;
    }

    private void PlayerMovement(){
        if(GUIManager.instance.GetToggle()){
            Vector3 moveDirect = directDown * playerDirection.forward + directUp * playerDirection.right;
            body.AddForce(moveDirect.normalized * playerSpeed, ForceMode.Force);
            //Sprint
            if(stamina <= 0 && !tired) {
                tired = true;
            } else if(Input.GetKey(KeyCode.LeftShift) && stamina > 0 && !tired){
                if(spam){
                    stamina -= 5f;
                    spam = false;
                }
                body.AddForce(moveDirect.normalized * playerSpeed * 1.5f, ForceMode.Force);
                stamina -= 0.05f;
            } else if (stamina < 100) {
                stamina += 0.05f;
                spam = true;
            }
            if(tired && stamina >= 100){
                tired = false;
            }

            GUIManager.instance.updateStaminaBar(stamina/100, tired);
        }
    }
    #endregion
    
    /// <summary>
    /// Adds the supplied value to the player's current health. Pass a negative value to damage the player
    /// </summary>
    public void UpdateHealth(int value){
        currentHealth += value;
        if(currentHealth < 0){
            GUIManager.instance.SwitchDisplayLose();
        }
        if (currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
        GUIManager.instance.updateHealthBar (currentHealth / maxHealth);
    }



    /*v1
    private void playerInput(){
        float currentX = playerTransform.position.x;
        float currentZ = playerTransform.position.z;

        if(Input.GetKey(KeyCode.W)){
            playerTransform.position = new Vector3 (currentX, groundHeight, currentZ + (playerSpeed * Time.deltaTime));
        } else if(Input.GetKey(KeyCode.S)){
            playerTransform.position = new Vector3 (currentX, groundHeight, currentZ - (playerSpeed * Time.deltaTime));
        }
        if(Input.GetKey(KeyCode.A)){
            playerTransform.position = new Vector3 (currentX - (playerSpeed * Time.deltaTime), groundHeight, currentZ);
        } else if(Input.GetKey(KeyCode.D)){
            playerTransform.position = new Vector3 (currentX + (playerSpeed * Time.deltaTime), groundHeight, currentZ);
        }

    }*/
}
