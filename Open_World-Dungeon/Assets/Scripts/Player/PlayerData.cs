using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public int MaxHealth;
    public int CurrentHealth;

    public TextMeshProUGUI healthText;

    public Vector3 TownSpawnLocation, DungeonSpawnLocation;

    private void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Town")
        {
            this.gameObject.transform.position = TownSpawnLocation;
        }
        if(scene.name == "Dungeon")
        {
            this.gameObject.transform.position = DungeonSpawnLocation;
        }
    }

    private void Update()
    {
        if(CurrentHealth <= 0)
        {
            Debug.Log("Game Over");
        }
        healthText.text = CurrentHealth.ToString() + " / " + MaxHealth.ToString();
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }
    public void Heal(int heal)
    {
        CurrentHealth += heal;
    }

    
}
