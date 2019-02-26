using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject ingamme;
    public GameObject gameOver;
    public GameObject playerCrosshair;
    public GameObject victory;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        ShowMainMenu();
        
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        ingamme.SetActive(false);
        victory.SetActive(false);
        gameManager.gameState = GameState.paused;
    }

    public void ShowInGameGUI()
    {
        mainMenu.SetActive(false);
        ingamme.SetActive(true);
        victory.SetActive(false);
        playerCrosshair.SetActive(true);

        gameManager.gameState = GameState.Running;

    }
    public void ShowGAmeOver()
    {
        gameOver.SetActive(true);
        playerCrosshair.SetActive(false);
        gameManager.gameState = GameState.Running;
    }

    public void showVictoryScreen()
    {
       
        ingamme.SetActive(false);
        victory.SetActive(true);
        gameManager.gameState = GameState.paused;
    }
}
