using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    public static int SnowFlakes;
    public int startSF = 400;

    public static int Lives;
    public int startLives = 20;

    void Start()
    {
        SnowFlakes = startSF;
        Lives = startLives;
    }

}