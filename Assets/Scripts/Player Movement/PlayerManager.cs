using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] private Animator animator;

    private float rotationSpeed;
    private float currentHealth, maxHealth;

    #region Startup functions
    void Start()
    {
        maxHealth = 100;
        currentHealth = 50;
        rotationSpeed = 10f;

        UpdateHealth(0);

    }
    #endregion
    

    #region Update Functions
    void Update()
    {
        MovePlayer();

    }
    #endregion


    #region Collision Functions
    private void OnCollisionEnter( Collision collision){
    }

    private void OnTriggerEnter (Collider other){
        if (other.tag == "Enemy"){
            GUIManager.instance.IncreaseWave();
        }
    }
    #endregion

    #region Movement end Input Functions
    private void MovePlayer(){
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(h, 0, v) + playerTransform.position;
        if (h != 0 || v != 0){
            
            //Debug.DrawLine (playerTransform.position, faceDirection,Color.red);
            //Rotate player to face the input direction
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection - playerTransform.position);

            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        float speedValue = Mathf.Max (Mathf.Abs(h), Mathf.Abs(v));

        animator.SetFloat("Speed", speedValue);


    }
    #endregion
    /// <summary>
    /// Adds the supplied value to the player's current health. Pass a negative value to damage the player
    /// </summary>
    private void UpdateHealth(int value){
        currentHealth += value;

        if(currentHealth < 0){
            print("Dead :c");
        }
        if (currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
        GUIManager.instance.updateHealthBar (currentHealth / maxHealth);
    }

    public void GetMouseLocation(){
        Vector3 mousePos = Input.mousePosition;
        bool temp = true;
        if (temp){
            print(mousePos);
        }
        
        //Ray ray = 
    }
}
