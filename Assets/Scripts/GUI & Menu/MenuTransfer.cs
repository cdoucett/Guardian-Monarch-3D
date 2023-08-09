using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTransfer : MonoBehaviour
{
    public static MenuTransfer instance;
    int difficulty;
    // Start is called before the first frame update
    private void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);

        } else {
            Destroy(gameObject);
        }
    }

    public void setDifficulty(int diff){
        difficulty = diff;
    }

    public int GetDifficulty(){
        return difficulty;
    }
}
