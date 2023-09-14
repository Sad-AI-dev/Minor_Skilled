using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
            transform.SetParent(null);
        }
    }
    public static GameManager instance;

    [Header("Money Settings")]
    public int money;

    [Header("External Components")]
    public Player player;
    public CameraInputManager camInputManager;

    //enemy spawning vars
    [HideInInspector] public ObjectSpawner enemySpawner;
    private bool isSpawning = true;

    //money UI vars
    public Action<int> onMoneyChanged;

    private void Start()
    {
        //mark as dont destroy on load
        DontDestroyRegister.instance.RegisterObject(gameObject);
    }

    //============== Enemy Spawning =================
    public void SpawnEnemy(GameObject prefab)
    {
        if (isSpawning && enemySpawner) {
            enemySpawner.SpawnObject(prefab);
        }
    }

    //=========== Manager Money ================
    public void GainMoney(int toGain)
    {
        money += toGain;
        UpdateMoneyUI();
    }

    public void PayMoney(int price)
    {
        money -= price;
        UpdateMoneyUI();
    }

    public bool CanAfford(int price)
    {
        return money >= price;
    }

    private void UpdateMoneyUI()
    {
        onMoneyChanged?.Invoke(money);
    }
}
