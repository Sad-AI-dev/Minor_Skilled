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
            DontDestroyOnLoad(gameObject); //mark as don't destroy on load
        }
    }
    public static GameManager instance;

    [Header("Money Settings")]
    public int money;

    [Header("External Components")]
    public Player player;

    //enemy spawning vars
    [HideInInspector] public ObjectSpawner enemySpawner;
    private bool isSpawning = true;

    //money UI vars
    public Action<int> onMoneyChanged;

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
