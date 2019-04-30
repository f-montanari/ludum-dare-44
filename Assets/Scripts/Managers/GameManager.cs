using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("Parameters")]
    public float StartingSkeletons = 0;
    public float spawnDistance = 1f;
    public float MaxPlayerHealth = 50f;
    public float SkeletonLifeCost = 10f;
    public int MaxLevelsAdventureMode = 5;    

    [HideInInspector]
    public int AvailableSkeletons;
    
    [Header("Prefabs/References")]
    public GameObject SkeletonPrefab;
    public GameObject player;
    public DungeonGenerator dungeonGenerator;
    public GameObject WinPanel;
    public GameObject LosePanel;

    private NavEntityBehaviour playerEntity;    

    // Start is called before the first frame update
    void Start()
    {                
        instance = this;                                
        instance.AvailableSkeletons = GameProgress.AvailableSkeletons;
    }

    public void PlayerDied()
    {
        LosePanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && AvailableSkeletons > 0)
        {
            SummonSkeleton();
        }
        if(Input.GetButtonDown("Heal") && AvailableSkeletons >0 )
        {
            ConsumeSoul();
        }        
    }

    public void SetupPlayer(GameObject player)
    {
        this.player = player;
        playerEntity = this.player.GetComponent<NavEntityBehaviour>();
        playerEntity.Health = GameProgress.playerHealth == 0 ? MaxPlayerHealth : GameProgress.playerHealth;
    }

    private void SummonSkeleton()
    {
        Vector3 spawnpoint = player.transform.position + new Vector3(spawnDistance * Mathf.Cos(UnityEngine.Random.Range(0, Mathf.PI / 2)), 0, spawnDistance * Mathf.Sin(UnityEngine.Random.Range(0, Mathf.PI / 2)));
        Instantiate(SkeletonPrefab, spawnpoint, Quaternion.identity);
        instance.AvailableSkeletons--;
        playerEntity.TakeDamage(SkeletonLifeCost, null);
    }    

    public void ConsumeSoul()
    {
        if(instance.AvailableSkeletons > 0)
        {
            playerEntity.Heal(SkeletonLifeCost);
            instance.AvailableSkeletons--;
        }            

    }

    public void NewLevel()
    {
        if(GameProgress.gameMode == GameProgress.GameMode.ADVENTURE && GameProgress.CurrentLevel == MaxLevelsAdventureMode)
        {
            WinPanel.SetActive(true);
            return;
        }
        AvailableSkeletons += GameObject.FindGameObjectsWithTag("Ally").Length;
        GameProgress.AvailableSkeletons = AvailableSkeletons;
        GameProgress.CurrentLevel += 1;
        GameProgress.playerHealth = playerEntity.Health;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    

    public void AddSkeleton()
    {
        instance.AvailableSkeletons++;
    }
}
