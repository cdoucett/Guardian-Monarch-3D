using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;
    //Main
    [SerializeField] private TextMeshProUGUI wave_Txt;
    [SerializeField] private TextMeshProUGUI money_Txt;
    [SerializeField] private TextMeshProUGUI enemyCount_Txt;
    [SerializeField] private TextMeshProUGUI tower_Txt;
    [SerializeField] private TextMeshProUGUI cost_Txt;
    [SerializeField] private Image healthBar_Img;
    [SerializeField] private Image staminaBar_Img;

    //Upgrade
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeCount_Txt;
    [SerializeField] private TextMeshProUGUI upgradeTower_Txt;
    [SerializeField] private TextMeshProUGUI upgradeDes_Txt;
    [SerializeField] private TextMeshProUGUI upgradeCost_Txt;
    [SerializeField] private TextMeshProUGUI upgradePath_Txt;
    [SerializeField] private GameObject pathSelector;
    private GameObject upgradeObject;
    private int[] archerUpgradeCost = new int[4], cannonUpgradeCost = new int[4], mageUpgradeCost = new int[4], poisonUpgradeCost = new int[4];
    private string[] archerUpgradeDes = new string[4], cannonUpgradeDes = new string[4], mageUpgradeDes = new string[4], poisonUpgradeDes = new string[4];
    private string[] archerUpgradePath = new string[2], cannonUpgradePath = new string[2], mageUpgradePath = new string[2], poisonUpgradePath = new string[2];
    private string upgradeTower;

    [SerializeField] private GameObject mainHUD;
    [SerializeField] private GameObject upgradeHUD;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    private int cost = 0;
    private bool toggle, selected, paused, rangeToggle;
    private float sellBack;
    

    private int wave, money, upgradeNumber;
    private void Awake (){
        //Singleton
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }

    }

    private void Update(){
        enemyCount_Txt.text = GameManager.instance.enemyCount.ToString();
        //Return to menu
        if(paused){
            if(Input.GetKeyDown(KeyCode.E)){
                paused = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("Menu");
            }
        }
        if(Input.GetKey(KeyCode.LeftControl)){
            if(Input.GetKeyDown(KeyCode.M)){
                ChangeMoney(200);
            }
        }
        if(toggle){
            //Set selected tower display
            if((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && !selected){
                tower_Txt.text = "Archer";
                cost_Txt.text = "150";
                cost = 150;
            } else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) {
                tower_Txt.text = "Cannon";
                cost_Txt.text = "300";
                cost = 300;
            } else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) {
                tower_Txt.text = "Mage";
                cost_Txt.text = "500";
                cost = 500;
            } else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) {
                tower_Txt.text = "Poison";
                cost_Txt.text = "700";
                cost = 700;
            }
            //Change color if player can buy
            if(CheckMoney(cost)){
                cost_Txt.color = Color.green;
            } else {
                cost_Txt.color = Color.red;
            }

            if(Input.GetKeyDown(KeyCode.F1)){
                Pause();
            }
        } else {
            #region Upgrade HUD
            if(Input.GetKeyDown(KeyCode.E)){
                if(upgradeTower.Equals("Archer")){
                    if(!rangeToggle){
                        upgradeObject.GetComponent<ArcherManager>().ShowRange(false);
                    }
                } else if (upgradeTower.Equals("Cannon")){
                    if(!rangeToggle){
                        upgradeObject.GetComponent<CannonManager>().ShowRange(false);
                    }
                    
                } else if (upgradeTower.Equals("Mage")){
                    if(!rangeToggle){
                        upgradeObject.GetComponent<MageManager>().ShowRange(false);
                    }
                    
                } else if (upgradeTower.Equals("Poison")){
                    if(!rangeToggle){
                        upgradeObject.GetComponent<PoisonManager>().ShowRange(false);
                    }
                    
                }
                
                SwitchDisplay();
            }
            
            //Button color to help with upgrading
            if(money < int.Parse(upgradeCost_Txt.text)){
                ColorBlock colorBlock = upgradeButton.colors;
		        colorBlock.normalColor = Color.red;
		        upgradeButton.colors = colorBlock;
            } else {
                ColorBlock colorBlock = upgradeButton.colors;
		        colorBlock.normalColor = Color.green;
		        upgradeButton.colors = colorBlock;
            }

            //Path selector
            if(upgradeNumber == 3){
                pathSelector.SetActive(true);
                if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)){
                    if(upgradeTower.Equals("Archer")){
                        upgradePath_Txt.text = archerUpgradePath[0];
                    } else if (upgradeTower.Equals("Cannon")){
                        upgradePath_Txt.text = cannonUpgradePath[0];
                    } else if (upgradeTower.Equals("Mage")){
                        upgradePath_Txt.text = mageUpgradePath[0];
                    } else if (upgradeTower.Equals("Poison")){
                        upgradePath_Txt.text = poisonUpgradePath[0];
                    }
                } else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) {
                    if(upgradeTower.Equals("Archer")){
                        upgradePath_Txt.text = archerUpgradePath[1];
                    } else if (upgradeTower.Equals("Cannon")){
                        upgradePath_Txt.text = cannonUpgradePath[1];
                    } else if (upgradeTower.Equals("Mage")){
                        upgradePath_Txt.text = mageUpgradePath[1];
                    } else if (upgradeTower.Equals("Poison")){
                        upgradePath_Txt.text = poisonUpgradePath[1];
                    }
                }
            } else {
                pathSelector.SetActive(false);
            }

            //Upgrading tower
            if(upgradeNumber == 4){
                upgradeDes_Txt.text = "This tower has reach the maximum amount of upgrades!";
                upgradeCost_Txt.text = "0";
                upgradeCount_Txt.text = "MAX";
            } else if(Input.GetKeyUp(KeyCode.U)){
                if(upgradeNumber == 3){
                    if(!upgradePath_Txt.text.Equals("No path selected")){
                        if(upgradeTower.Equals("Archer") && CheckMoney(archerUpgradeCost[upgradeNumber])){
                            ChangeMoney(-archerUpgradeCost[upgradeNumber]);
                            upgradeObject.GetComponent<ArcherManager>().Upgrade(upgradePath_Txt.text);
                        } else if (upgradeTower.Equals("Cannon") && CheckMoney(cannonUpgradeCost[upgradeNumber])){
                            ChangeMoney(-cannonUpgradeCost[upgradeNumber]);
                            upgradeObject.GetComponent<CannonManager>().Upgrade(upgradePath_Txt.text);
                        } else if (upgradeTower.Equals("Mage") && CheckMoney(mageUpgradeCost[upgradeNumber])){
                            ChangeMoney(-mageUpgradeCost[upgradeNumber]);
                            upgradeObject.GetComponent<MageManager>().Upgrade(upgradePath_Txt.text);
                        } else if (upgradeTower.Equals("Poison") && CheckMoney(poisonUpgradeCost[upgradeNumber])){
                            ChangeMoney(-poisonUpgradeCost[upgradeNumber]);
                            upgradeObject.GetComponent<PoisonManager>().Upgrade(upgradePath_Txt.text);
                        }
                    }
                } else {
                    if(upgradeTower.Equals("Archer") && CheckMoney(archerUpgradeCost[upgradeNumber])){
                        ChangeMoney(-archerUpgradeCost[upgradeNumber]);
                        upgradeObject.GetComponent<ArcherManager>().Upgrade();
                        
                    } else if (upgradeTower.Equals("Cannon") && CheckMoney(cannonUpgradeCost[upgradeNumber])){
                        ChangeMoney(-cannonUpgradeCost[upgradeNumber]);
                        upgradeObject.GetComponent<CannonManager>().Upgrade();
                        
                    } else if (upgradeTower.Equals("Mage") && CheckMoney(mageUpgradeCost[upgradeNumber])){
                        ChangeMoney(-mageUpgradeCost[upgradeNumber]);
                        upgradeObject.GetComponent<MageManager>().Upgrade();
                        
                    } else if (upgradeTower.Equals("Poison") && CheckMoney(poisonUpgradeCost[upgradeNumber])){
                        ChangeMoney(-poisonUpgradeCost[upgradeNumber]);
                        upgradeObject.GetComponent<PoisonManager>().Upgrade();
                        
                    }
                }
            }
            if(Input.GetKeyDown(KeyCode.R)){
                rangeToggle = !rangeToggle;
                if(upgradeTower.Equals("Archer") || upgradeTower.Equals("Archer(Clone)")){
                    upgradeObject.GetComponent<ArcherManager>().ShowRange(rangeToggle);
                } else if (upgradeTower.Equals("Cannon") || upgradeTower.Equals("Cannon(Clone)")){
                    upgradeObject.GetComponent<CannonManager>().ShowRange(rangeToggle);
                } else if (upgradeTower.Equals("Mage") || upgradeTower.Equals("Mage(Clone)")){
                    upgradeObject.GetComponent<MageManager>().ShowRange(rangeToggle);
                } else if (upgradeTower.Equals("Poison") || upgradeTower.Equals("Poison(Clone)")){
                    upgradeObject.GetComponent<PoisonManager>().ShowRange(rangeToggle);
                }
            }
            if(Input.GetKeyDown(KeyCode.Backspace)){
                if(upgradeTower.Equals("Archer")){
                    ChangeMoney((int)Mathf.Round(150 * sellBack));
                    for(int i = 0; i < upgradeNumber; i++){
                        ChangeMoney((int)Mathf.Round(archerUpgradeCost[i]* sellBack));
                    }
                    Destroy(upgradeObject.transform.parent.gameObject);
                } else if (upgradeTower.Equals("Cannon")){
                    ChangeMoney((int)Mathf.Round(300* sellBack));
                    for(int i = 0; i < upgradeNumber; i++){
                        ChangeMoney((int)Mathf.Round(cannonUpgradeCost[i] * sellBack));
                    }
                    Destroy(upgradeObject.transform.parent.gameObject);
                    
                } else if (upgradeTower.Equals("Mage")){
                    ChangeMoney((int)Mathf.Round(500* sellBack));
                    for(int i = 0; i < upgradeNumber; i++){
                        ChangeMoney((int)Mathf.Round(mageUpgradeCost[i]* sellBack));
                    }
                    Destroy(upgradeObject.transform.parent.gameObject);
                    
                } else if (upgradeTower.Equals("Poison")){
                    ChangeMoney((int)Mathf.Round(700* sellBack));
                    for(int i = 0; i < upgradeNumber; i++){
                        ChangeMoney((int)Mathf.Round(poisonUpgradeCost[i]* sellBack));
                    }
                    Destroy(upgradeObject.transform.parent.gameObject);
                    
                }
                SwitchDisplay();
            }
        }
        #endregion
    }

    private void Start(){
        wave = 0;
        SetWaveDisplay();
        money = 500;
        sellBack = 0;
        int difficulty = MenuTransfer.instance.GetDifficulty();
        if(difficulty == 0){
            money = 900;
            sellBack = 0.9f;
        } else if (difficulty == 1){
            money = 500;
            sellBack = 0.75f;
        } else if (difficulty == 2){
            money = 300;
            sellBack = 0.60f;
        }
        SetMoneyDisplay();
        toggle = true;
        rangeToggle = false;
        paused = false;
        Time.timeScale = 1;

        //Upgrade stuff
        upgradeTower = "None";
        upgradeNumber = 100;
        #region Cost
        archerUpgradeCost[0] = 200;
        archerUpgradeCost[1] = 400;
        archerUpgradeCost[2] = 600;
        archerUpgradeCost[3] = 1000;

        cannonUpgradeCost[0] = 200;
        cannonUpgradeCost[1] = 500;
        cannonUpgradeCost[2] = 800;
        cannonUpgradeCost[3] = 1300;

        mageUpgradeCost[0] = 400;
        mageUpgradeCost[1] = 700;
        mageUpgradeCost[2] = 900;
        mageUpgradeCost[3] = 2000;

        poisonUpgradeCost[0] = 300;
        poisonUpgradeCost[1] = 800;
        poisonUpgradeCost[2] = 1200;
        poisonUpgradeCost[3] = 1500;
        #endregion

        #region Descriptions
        archerUpgradeDes[0] = "Increases the range, damage, and attack speed of this tower";
        archerUpgradeDes[1] = "Arrows deal even more damage";
        archerUpgradeDes[2] = "Enemies now take bleed damage after being hit with an arrow \nLast for 3s and does not stack";
        archerUpgradeDes[3] = "Sniper Arrows: \nHeavily increase damage and range \n\nBomb Arrows: \nArrows now explode on impact, dealing AOE damage";

        cannonUpgradeDes[0] = "Increase cannon's AOE explosion radius";
        cannonUpgradeDes[1] = "Increase the range of this tower";
        cannonUpgradeDes[2] = "Cannon shoots slightly faster and AOE explosion radius is even bigger";
        cannonUpgradeDes[3] = "Huge Bombs: \nBombs explosion radius is nuclear. \n\nContagion Bombs: \nWhen a bomb explodes on an enemy, they are 'Marked' and when they die they explode, damaging surronding enemies";

        mageUpgradeDes[0] = "Increase beam limit to 4";
        mageUpgradeDes[1] = "Beams charge faster to max damage. Also increase the range slightly";
        mageUpgradeDes[2] = "Increase tick rate of mage beam to 5";
        mageUpgradeDes[3] = "Infernal Beam: \nThis tower now only targets one enemy, however deals the power of 8 beams \n\nOmni Beam: \nMage tower now targets ALL enemies in range, but only has 3 ticks of charge up";

        poisonUpgradeDes[0] = "Increase range & attack speed greatly";
        poisonUpgradeDes[1] = "All potions have stronger status effects \n\nDamge: increase damage \nSlow: increase slow \nFreeze: increase freeze time \nPosion: increase duration";
        poisonUpgradeDes[2] = "Status effects last longer on the ground after exploding";
        poisonUpgradeDes[3] = "Universal Posion: \nPotions thrown now apply every effect \n\nBuff potions: \nTower now throws potions which buff range, damage, and attack speed to towers in it's range \nPotions last for 5s and thrown every 5s";
        #endregion
    
        #region Path names
        archerUpgradePath[0] = "Sniper Arrows";
        archerUpgradePath[1] = "Bomb Arrows";

        cannonUpgradePath[0] = "Huge Bombs";
        cannonUpgradePath[1] = "Contagion Bombs";

        mageUpgradePath[0] = "Infernal Beam";
        mageUpgradePath[1] = "Omni Beam";

        poisonUpgradePath[0] = "Universal Posion";
        poisonUpgradePath[1] = "Buff potions";
        #endregion
        
    }

    #region Switching Displays
    public void SwitchDisplay(){
        //Toggle = true -> No upgradeHUD
        //Toggle = false -> upgradeHUD
        toggle = !toggle;
        upgradeHUD.SetActive(!toggle);
    }
    public void SwitchDisplayLose(){
        upgradeHUD.SetActive(false);
        mainHUD.SetActive(false);
        paused = false;
        Pause();
        loseScreen.SetActive(true);
    }
    public void SwitchDisplayWin(){
        upgradeHUD.SetActive(false);
        mainHUD.SetActive(false);
        paused = false;
        Pause();
        winScreen.SetActive(true);
    }

    private void Pause(){
        if(paused){
            paused = false;
            Time.timeScale = 1;
        } else {
            paused = true;
            Time.timeScale = 0;
        }
    }

    public bool CheckDisplay(){
        return winScreen.activeSelf || loseScreen.activeSelf;
    }
    #endregion

    #region Main Display
    
    private void SetWaveDisplay(){
        wave_Txt.text = wave.ToString();
    }
    private void SetMoneyDisplay(){
        money_Txt.text = money.ToString();
    }

    private void SetCostDisplay(int cost){
        cost_Txt.text = cost.ToString();
    }

    public void IncreaseWave(){
        wave += 1;
        SetWaveDisplay();
    }

    #region money
    /// <summary>
    /// Adds the supplied value to the player's money. Pass a negative value to remove money
    /// </summary>
    public void ChangeMoney(int value){
        money += value;
        SetMoneyDisplay();
    }
    /// <summary>
    /// Compares value to player's money, returns true if money is greater or equal to value
    /// </summary>
    public bool CheckMoney(int value){
        return money >= value;
    }
    #endregion
    
    public string GetTowerSelected(){
        return tower_Txt.text;
    }

    public bool GetToggle(){
        return toggle;
    }

    #region Health Bar
    public void updateHealthBar (float healthPercentage){
        healthBar_Img.fillAmount = healthPercentage;
    }
    #endregion

    #region Stamina Bar
    public void updateStaminaBar (float stamina, bool tired){
        staminaBar_Img.fillAmount = stamina;
        if (tired){
            staminaBar_Img.color = Color.red;
        } else {
            staminaBar_Img.color = Color.green;
        }
    }
    #endregion
    #endregion

    #region Upgrade Display

    public void SetUpgradeTower(GameObject towerInfo){
        upgradeTower = towerInfo.transform.parent.name;
        upgradeObject = towerInfo;
        if(upgradeTower.Equals("Archer") || upgradeTower.Equals("Archer(Clone)")){
            upgradeTower = "Archer";
            upgradeNumber = towerInfo.GetComponent<ArcherManager>().upgradeCount;
            towerInfo.GetComponent<ArcherManager>().ShowRange(true);
        } else if (upgradeTower.Equals("Cannon") || upgradeTower.Equals("Cannon(Clone)")){
            upgradeTower = "Cannon";
            upgradeNumber = towerInfo.GetComponent<CannonManager>().upgradeCount;
            towerInfo.GetComponent<CannonManager>().ShowRange(true);
        } else if (upgradeTower.Equals("Mage") || upgradeTower.Equals("Mage(Clone)")){
            upgradeTower = "Mage";
            upgradeNumber = towerInfo.GetComponent<MageManager>().upgradeCount;
            towerInfo.GetComponent<MageManager>().ShowRange(true);
        } else if (upgradeTower.Equals("Poison") || upgradeTower.Equals("Poison(Clone)")){
            upgradeTower = "Poison";
            upgradeNumber = towerInfo.GetComponent<PoisonManager>().upgradeCount;
            towerInfo.GetComponent<PoisonManager>().ShowRange(true);
        }
        UpdateUpgradeInfo();
    }

    private void UpdateUpgradeInfo(){
        upgradeCount_Txt.text = (upgradeNumber+1).ToString();
        upgradeTower_Txt.text = upgradeTower;
        if(upgradeNumber != 4){
            if(upgradeTower.Equals("Archer")){
                upgradeDes_Txt.text = archerUpgradeDes[upgradeNumber];
                upgradeCost_Txt.text = archerUpgradeCost[upgradeNumber].ToString();
            } else if (upgradeTower.Equals("Cannon")){
                upgradeDes_Txt.text = cannonUpgradeDes[upgradeNumber];
                upgradeCost_Txt.text = cannonUpgradeCost[upgradeNumber].ToString();
            } else if (upgradeTower.Equals("Mage")){
                upgradeDes_Txt.text = mageUpgradeDes[upgradeNumber];
                upgradeCost_Txt.text = mageUpgradeCost[upgradeNumber].ToString();
            } else if (upgradeTower.Equals("Poison")){
                upgradeDes_Txt.text = poisonUpgradeDes[upgradeNumber];
                upgradeCost_Txt.text = poisonUpgradeCost[upgradeNumber].ToString();
            }
        }
        if(upgradeNumber == 3){
            upgradePath_Txt.text = "No path selected";
        }
    }
    #endregion
}
