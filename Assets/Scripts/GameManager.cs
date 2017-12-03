using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


    public static bool GameIsOver;

    public GameObject gameOverUI;
    public GameObject completeLevelUI;

    
    private float NetWorth = 1f;
    public static float generalDifficultyMod;
    
    public static int EarnedMoney;

    public static int SpentMoney;
    

    public void CalculateDifficulty()
    {
        if (EarnedMoney != 0 && SpentMoney != 0)
        {
            NetWorth = EarnedMoney / SpentMoney; //player must spend money
            generalDifficultyMod = Mathf.Log(NetWorth);
        }
        else
        {
            Debug.Log("Zero values" + EarnedMoney + " " + SpentMoney);
            generalDifficultyMod =  0f;
        }
    }

    private void Awake()
    {
        GameIsOver = false;

        CalculateDifficulty();//and do this every level start
    }


    private void Update()
    {
        
            if (GameIsOver)
                return;

            if (PlayerStats.Lives <= 0)
            {
                EndGame();
            }
        
    }
    void LevelEnd()
    {

    }

    public void LevelStart()
    {
        CalculateDifficulty();
    }

    void EndGame()
    {
        GameIsOver = true;
        gameOverUI.SetActive(true);
    }

}
