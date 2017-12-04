using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomButtons : MonoBehaviour {


#region WaveDifficulty
    private float CalculatedDifficluty;
    private const int baseAmount = 400;

    private int delta;
        #region DefaultWaveParameters
            float _timeBetween = 5f;
            int _numWaves = 2;
            float _rate = 30f;
            int _count = 6;
            float _time = 5f;
        #endregion
#endregion

    [System.Serializable]
    public struct Wave
    {
        public float rate;
        public int count;
        public float b_time;
    }

    enum WaveState
    {
        SPAWNING,//spawnenemy
        WAITING,//waitforkilling
        COUNTING//countdown
    }


    public delegate void LevelCompleted();
    public static event LevelCompleted LvlCompletedEvent;


    private WaveState state = WaveState.COUNTING;
    private int waveIndex = 0;
    private int AliveEnemies;
    private float waveCountdown;
    private GameObject parentCanvas;
    private CanvasGroup cg;

    public Wave[] waves;
    [Space]

    [Header("References")]
    public Button button;
    public Canvas playerUI;

    [Header ("Optional")]
    public float timeBetween = 5f;//must be random, based on money earned... or not
    public float SnowflakeAmount = 0.015f;

    void Start () {
        cg = playerUI.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        parentCanvas = GameObject.FindWithTag("GamePlay");
        SetDifficulty(GameManager.generalDifficultyMod);
        ButtonBehaviour.TimeOutEvent += MissedButton;
        ButtonBehaviour.KilledButton += OnEnemyKilled;
        ButtonBehaviour.EarnMoneyEvent += EarnReward;
    }

    void Update () {

        if (waveIndex >= waves.Length)
        {
            Debug.Log("Completed");
            //gotoshop
            ButtonBehaviour.KilledButton -= OnEnemyKilled;
            ButtonBehaviour.TimeOutEvent -= MissedButton;
            ButtonBehaviour.EarnMoneyEvent -= EarnReward;

            if(LvlCompletedEvent!=null)
            LvlCompletedEvent();
               enabled = false;//TEMP!!!
            return;
        }

        if (state == WaveState.WAITING)
        {
            if (AllEnemiesKilled())
            {
                state = WaveState.COUNTING;//start next wave
                //count next wave parameters

                GameManager.EarnedMoney = PlayerStats.SnowFlakes;//save to GameManager

                waveIndex++;
                if (waveIndex >= waves.Length)
                { return; }

                
                CalculatedDifficluty = CalculateWaveDifficulty(PlayerStats.SnowFlakes - baseAmount - delta);
                delta = PlayerStats.SnowFlakes - baseAmount;

                SetWaveDifficulty(CalculatedDifficluty);
                
                waveCountdown = timeBetween;
            
            }

        }
        else
            if(waveCountdown <= 0f)
            {
                //place for coroutine
                if (state != WaveState.SPAWNING)
                {
                //Debug.Log("Entrering Spawning" + waveIndex);
                StartCoroutine(SpawnButtonWaves());
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }
    }

    IEnumerator SpawnButtonWaves()
    {
        
        for (int i = 0; i < waves[waveIndex].count; i++)
        {
            state = WaveState.SPAWNING;

            SpawnButton(button);
            yield return new WaitForSeconds(60f / waves[waveIndex].rate);
        }
        state = WaveState.WAITING;
    }



    void  SpawnButton(Button button)
    {
        Button go = Instantiate(button,
                   new Vector3
                   (Random.Range(-parentCanvas.GetComponent<RectTransform>().rect.width/2 + 20,
                   parentCanvas.GetComponent<RectTransform>().rect.width / 2 - 20),

                   Random.Range(-parentCanvas.GetComponent<RectTransform>().rect.height / 2 +16,
                   parentCanvas.GetComponent<RectTransform>().rect.height/2 - 16),

                   0f),
                   parentCanvas.transform.rotation,
                   parentCanvas.transform);
        go.GetComponent<ButtonBehaviour>().Settime(waves[waveIndex].b_time);
        go.enabled = true;
    }

    public void SetWaveDifficulty(float waveDifficultyMod = 0f)
    {
        Debug.Log(waveDifficultyMod);

        waves[waveIndex].rate = _rate + waveDifficultyMod;
        waves[waveIndex].count = _count + Convert.ToInt32(Math.Ceiling(waveDifficultyMod));
        waves[waveIndex].b_time = _time - Mathf.Clamp(waveDifficultyMod, 0f, waveDifficultyMod);
        AliveEnemies = waves[waveIndex].count;
    }


    public void SetDifficulty(float GeneralDifficulty = 0f)
    {
        waves = new Wave[_numWaves + Convert.ToInt32(Math.Ceiling(GeneralDifficulty))];
        timeBetween = _timeBetween - Mathf.Clamp(GeneralDifficulty, 0f, GeneralDifficulty);
    }
    
    public void MissedButton()
    {
        PlayerStats.Lives -= ButtonBehaviour.s_buttonDamage;
        //Debug.Log("MissedButton");
    }

    public bool AllEnemiesKilled()
    {
       return AliveEnemies <= 0;
    }


    public void OnEnemyKilled()
    {
        AliveEnemies--;
        
    }

    public void EarnReward()
    {
        PlayerStats.SnowFlakes += ButtonBehaviour.s_reward;
        SpawnFlakes();
    }

    private float CalculateWaveDifficulty(int moneyEarned)
    {
        //Debug.Log("In Calculate" +  moneyEarned);
        return moneyEarned != 0 ? Mathf.Log10(moneyEarned) : 1f;
    }


    private void SpawnFlakes()
    {
        if(cg.alpha<1f)
            cg.alpha += SnowflakeAmount;
        Debug.Log(playerUI.GetComponent<CanvasGroup>().alpha);
    }
}
