using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour {

    public delegate void OutOfTime();
    public delegate void KillButton();
    public delegate void Earn();

    public static event KillButton KilledButton;
    public static event OutOfTime TimeOutEvent;
    public static event Earn EarnMoneyEvent;


    public float buttonTime;
    public int reward, buttonDamage = 1;
    public static int s_reward;
    public static int s_buttonDamage;


    private void Awake()
    {
        s_reward = reward;
        s_buttonDamage = buttonDamage;
    }
    public void Settime(float time)
    {
        buttonTime = time;
    }
	
	// Update is called once per frame
	void Update () {
		
        if(buttonTime>0f)
        {
            buttonTime -= Time.deltaTime;
        }
        else
        {
            if (TimeOutEvent != null)
            {
                TimeOutEvent();
            }
            DieButton();
        }
    }

    public void DieButton()
    {
        Destroy(this.gameObject);
        if (KilledButton != null)
        {
            KilledButton();
        }
    }
    
    public void EarnMoneyButton()
    {
        if(EarnMoneyEvent != null)
        {
            EarnMoneyEvent();
        }
    }
    
}
