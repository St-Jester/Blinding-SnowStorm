using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private float NetWorth = 1f;
    public static float generalDifficultyMod;

    public RandomButtons randomButtons;
    public static int EarnedMoney { get; set; }

    public static int SpentMoney { get; set; }
    

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
        CalculateDifficulty();//and do this every level start
    }

    void LevelEnd()
    {
    }

    public void LevelStart()
    {
        CalculateDifficulty();
    }

    void Update () {
		
	}
}
