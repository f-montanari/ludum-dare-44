using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgress : MonoBehaviour
{

    public enum GameMode
    {
        ADVENTURE,
        ENDLESS
    }

    public static GameProgress instance;        
    public static int CurrentLevel = 0;
    public static int AvailableSkeletons = 0;
    public static float playerHealth = 0;
    public static GameMode gameMode;

    internal static void ResetGameProgress()
    {
        AvailableSkeletons = 0;
        CurrentLevel = 0;
        playerHealth = 0;             
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }
}
