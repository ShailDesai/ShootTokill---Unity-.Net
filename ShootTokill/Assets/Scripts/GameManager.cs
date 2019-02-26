using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;

public enum GameState
{
    paused,
    Running
}

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public GUIManager guiManager;
    public TargetManager targetManager;
    public GameObject player;
    public Text scoreText;
    public int countdownMinutes = 3;
    public Text countdownText;
    public int winningScore = 2000;

    public GameState gameState;

    private int score;

    private void Awake()
    {


        //setting instance of game manager so everyone can acess it
        if (GameManager.instance == null)
        {
            GameManager.instance = this;
        }
        //keep screen on
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Start()
    {
        Init();
    }

    public void StartNewGame()
    {
        //Reseting the score

        ResetScore();

        //Enable target Manager
        targetManager.enabled = true;

        //starting countdown
        StartCoroutine(CountDown());

    }

    private void Init()
    {
       
        //Init targets
        targetManager.InitTargets();
        
    }

    public void EndGame()
    {
        //Disable target Manager
        targetManager.enabled = false;


        //stop all corotuines
        StopAllCoroutines();
    }

    public void ExitApplicationn()
    {

#if Unity_Editor
        EditorApplication.isPlaying = false
#else
        Application.Quit();
#endif

    }

    public void AddPoints(int points)
    {
        //add points to the toal score player achive
        score += points;
        //Check minimum score
        if(score>winningScore)
        {
            StartCoroutine(WinGame());
        }
        UpdatescoreText();
    }

    private void UpdatescoreText()
    {
        //UpdateScoreText UI feild
        scoreText.text = score.ToString();
    }

    private void ResetScore()
    {
        //reseting score to zero
        score = 0;
        UpdatescoreText();

    }


    private IEnumerator CountDown()
    {
        int i = countdownMinutes * 60;
        WaitForSeconds tick = new WaitForSeconds(1);

        int minutes;
        int seconds;

        //adjust countdown timer
        while (i >=0)//starting countdown loop
        {
            minutes = i / 60;
            seconds = i - (minutes * 60);

            countdownText.text = String.Format("{0:00}:{1:00}", minutes, seconds);

            yield return tick; //waitb for one second
            i--;
        }
        //Game Over
        { EndGame();
            guiManager.ShowGAmeOver();
        }




    }

    private IEnumerator WinGame()
    {
        guiManager.showVictoryScreen();

        yield return new WaitForSeconds(1);
        
       // yeild return new WaitForSeconds(1);

        EndGame();
    }



}
