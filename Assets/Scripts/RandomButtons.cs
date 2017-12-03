using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomButtons : MonoBehaviour {

    #region WaveDifficulty

    public int ThisWaveEarned { get; set; }


    private float CalculateWaveDifficulty()
    {
        return ThisWaveEarned != 0 ? Mathf.Log10(ThisWaveEarned) : 1f;
    }
#region DefaultValues
    float _timeBetween = 5f;
    int _numWaves = 5;
    float _rate = 10f;
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

    WaveState state = WaveState.COUNTING;
    private int waveIndex = 0;
    private int AliveEnemies;

    private float waveCountdown;

    public int number;//temp for coroutines

    [Header("References")]
    public Button button;

    public float timeBetween = 5f;//must be random, based on money earned... or not
    
    public Wave[] waves;


    void Start () {
        SetDifficulty(GameManager.generalDifficultyMod);
        ButtonBehaviour.TimeOutEvent += MissedButton;
        ButtonBehaviour.KilledButton += OnEnemyKilled;
    }

	void Update () {

        if (waveIndex >= waves.Length - 1)
        {
            Debug.Log("Completed");
            //gotoshop
            ButtonBehaviour.KilledButton -= OnEnemyKilled;
            ButtonBehaviour.TimeOutEvent -= MissedButton;
            enabled = false;//TEMP!!!
            return;
        }

        if (state == WaveState.WAITING)
        {
            if (AllEnemiesKilled())
            {
                state = WaveState.COUNTING;//start next wave
                //count next wave parameters

                GameManager.EarnedMoney = ThisWaveEarned;//save to GameManager

                waveIndex++;
                SetWaveDifficulty(CalculateWaveDifficulty());
                Debug.Log("Setting difficulty" + waveIndex);
                waveCountdown = timeBetween;
                ThisWaveEarned = 0;
            }

        }
        else
            if(waveCountdown <= 0f)
            {
                //place for coroutine
                if (state != WaveState.SPAWNING)
                {
                    Debug.Log("Entrering Spawning" + waveIndex);
                
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
        state = WaveState.SPAWNING;
        for (int i = 0; i < waves[waveIndex].count; i++)
        {
             SpawnButton(button);
            yield return new WaitForSeconds(1f/waves[waveIndex].rate);
        }
        state = WaveState.WAITING;
    }



    void  SpawnButton(Button button)
    {
        Button go = Instantiate(button,
                   new Vector3(Random.Range(0, this.gameObject.GetComponent<RectTransform>().rect.width),
                   Random.Range(0, this.gameObject.GetComponent<RectTransform>().rect.height),
                   0f),
                   this.transform.rotation, this.gameObject.transform);
        go.GetComponent<ButtonBehaviour>().Settime(waves[waveIndex].b_time);
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
        Debug.Log("Earned" + ButtonBehaviour.s_reward);
        ThisWaveEarned += ButtonBehaviour.s_reward;
    }
}
