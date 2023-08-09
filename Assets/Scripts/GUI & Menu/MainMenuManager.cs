using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject easy, medium, hard;

    public void StartGame(){
        SceneManager.LoadScene("Game");
        
    }

    public void ChangeDifficulty(){
        if(!easy.activeSelf){
            easy.SetActive(true);
            medium.SetActive(false);
            MenuTransfer.instance.setDifficulty(1);
        } else if(!medium.activeSelf){
            medium.SetActive(true);
            hard.SetActive(false);
            MenuTransfer.instance.setDifficulty(2);
        } else if(!hard.activeSelf){
            hard.SetActive(true);
            easy.SetActive(false);
            MenuTransfer.instance.setDifficulty(0);
        }
    }

    public void QuitGame(){
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
        
    }
}
