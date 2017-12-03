using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour {

    public delegate void OutOfTime();
    public delegate void KillButton();
    public static event KillButton KilledButton;
    public static event OutOfTime TimeOutEvent;

    public float buttonTime;
    public int reward;
    public static int s_reward;


    private void Awake()
    {
        s_reward = reward;
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
    
    
}
