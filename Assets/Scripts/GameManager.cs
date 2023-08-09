using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject enemyKnight;
    [SerializeField] private GameObject enemyArcher;
    [SerializeField] private GameObject enemyBuffer;
    [SerializeField] private GameObject enemyBrute;
    [SerializeField] private GameObject enemyKing;
    [SerializeField] private LineRenderer line;
    [SerializeField] private HealthManager hp;

    [HideInInspector] public PlayerManagerv2 player;
    [HideInInspector] public Obstruction placeable;
    private float enemyTime, healthScaler = 1, enemyKnightPercent = 1, enemyArcherPercent = 0, enemyBrutePercent = 0, enemyBufferPercent = 0;
    private int enemyAmount, wave, difficulty;
    //Total enemies alive
    public int enemyCount;

    private void Awake(){
        difficulty = MenuTransfer.instance.GetDifficulty();
        if (instance == null){

            instance = this;

            //DontDestroyOnLoad(gameObject);

        } else {
            Destroy(gameObject);
        }
    }
    private void Start(){
        //Total Enemies to spawn
        enemyAmount = 4;
        //Total Enemies alive
        enemyCount = 0;
        enemyTime = 3f;
        wave = 0;
    }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.P) && enemyCount == 0 && !GUIManager.instance.CheckDisplay()){
            GUIManager.instance.IncreaseWave();
            enemyCount = enemyAmount;
            wave++;
            StartCoroutine(SpawnWaves());
        }
        if(Input.GetKey(KeyCode.LeftControl)){
            if(Input.GetKeyDown(KeyCode.Equals)){
                GUIManager.instance.IncreaseWave();
                healthScaler += .1f;
            Scale();
            if(wave > 20){
                enemyBrutePercent += 0.01f;
                enemyBufferPercent += 0.01f;
                enemyKnightPercent -= 0.01f;
                enemyArcherPercent -= 0.01f;
            } else if (wave > 15) {
                enemyBufferPercent += 0.03f;
                enemyBrutePercent -= 0.01f;
                enemyKnightPercent -= 0.01f;
                enemyArcherPercent -= 0.01f;
            } else if (wave > 10) {
                enemyBrutePercent += 0.05f;
                enemyKnightPercent -= 0.025f;
                enemyArcherPercent -= 0.025f;
            } else if (wave > 5) {
                enemyKnightPercent -= 0.1f;
                enemyArcherPercent += 0.1f;
            }
            wave++;
            }
        }
        if(wave == 25 && enemyCount == 0){
            GUIManager.instance.SwitchDisplayWin();
        }
    }
    
    private IEnumerator SpawnWaves(){
        /*          ~~~How Spawning Works~~~
        Enemy Amount increase after each wave by 1.5x + 1
        Spawn enemies based on their percent (pool of spawn) they have
            Waves 1-4: No change as it's just Knight
            Waves 5-9: Archer +10% pool (-10%). End at -> 50% archers 50% knights
            Waves 10-14: Brutes +5% pool (-2.5% each). End at -> 30% brutes 35% archers 35% knights
            Waves 15-19: Buffers +3% pool (1% each). End at -> 15% buffers 25% brutes 30% archers 30% knights
            Waves 20+: Buffers & Brutes +1% pool (1% each). Max at 45% buffer 55% brutes */
        
        if(wave > 19){
            enemyBrutePercent += 0.01f;
            enemyBufferPercent += 0.01f;
            enemyKnightPercent -= 0.01f;
            enemyArcherPercent -= 0.01f;
        } else if (wave > 14) {
            enemyBufferPercent += 0.03f;
            enemyBrutePercent -= 0.01f;
            enemyKnightPercent -= 0.01f;
            enemyArcherPercent -= 0.01f;
        } else if (wave > 9) {
            enemyBrutePercent += 0.05f;
            enemyKnightPercent -= 0.025f;
            enemyArcherPercent -= 0.025f;
        } else if (wave > 4) {
            enemyKnightPercent -= 0.1f;
            enemyArcherPercent += 0.1f;
        }

        int knightAmount = (int)Mathf.Round(enemyAmount * enemyKnightPercent);
        int archerAmount = (int)Mathf.Round(enemyAmount * enemyArcherPercent);
        int bruteAmount = (int)Mathf.Round(enemyAmount * enemyBrutePercent);
        int bufferAmount = (int)Mathf.Round(enemyAmount * enemyBufferPercent);
        int enemiesSpawning = enemyAmount;
        if(wave == 25){
            enemyCount++;
            enemyAmount++;
            GameObject king = Instantiate(enemyKing, line.GetPosition(0), enemyKnight.transform.rotation);
        }
        while(enemiesSpawning > 0){
            bool spawned = false;
            while(!spawned){
                int randomNum = Random.Range(0,4);
                if(randomNum == 1){
                    if(knightAmount > 0){
                        GameObject knight = Instantiate(enemyKnight, line.GetPosition(0), enemyKnight.transform.rotation);
                        knight.GetComponent<HealthManager>().ScaleHealth(healthScaler);
                        knightAmount--;
                        spawned = true;
                    }
                } else if (randomNum == 2){
                    if(archerAmount > 0){
                        GameObject archer = Instantiate(enemyArcher, line.GetPosition(0), enemyKnight.transform.rotation);
                        archer.GetComponent<HealthManager>().ScaleHealth(healthScaler);
                        archerAmount--;
                        spawned = true;
                    }
                } else if (randomNum == 3){
                    if(bruteAmount > 0){
                        GameObject brute = Instantiate(enemyBrute, line.GetPosition(0), enemyKnight.transform.rotation);
                        brute.GetComponent<HealthManager>().ScaleHealth(healthScaler);
                        bruteAmount--;
                        spawned = true;
                    }
                } else {
                    if(bufferAmount > 0){
                        GameObject buffer = Instantiate(enemyBuffer, line.GetPosition(0), enemyKnight.transform.rotation);
                        buffer.GetComponent<HealthManager>().ScaleHealth(healthScaler);
                        bufferAmount--;
                        spawned = true;
                    }
                }

            }
            yield return new WaitForSeconds(enemyTime);
            enemiesSpawning--;
        }
        
        Scale();
        
    }

    private void Scale(){
        float healthScale = .1f;
        int enemyAmountCap = 350;
        float timeScale = 1.2f;
        float timeCap = 0.4f;

        if(difficulty == 0){
            healthScale = 0.075f;
            timeScale = 1.1f;
            timeCap = 0.5f;
            enemyAmountCap = 400;
        } else if (difficulty == 1){
            healthScale = 0.09f;
            timeScale = 1.15f;
            timeCap = 0.4f;
            enemyAmountCap = 500;
        } else if (difficulty == 2){
            healthScale = 0.12f;
            timeScale = 1.2f;
            timeCap = 0.3f;
            enemyAmountCap = 600;
        }
        healthScaler += healthScale;
        if(enemyTime >= timeCap){
            enemyTime /= timeScale;
        }
        if(enemyAmount >= enemyAmountCap){
		    enemyAmount = (int)((float)enemyAmount*1.1f);
		} else {
            enemyAmount += (int)(enemyAmount/2 + 1);
        }
    }
}
