using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTarget : MonoBehaviour
{
    public List<MeleeEnemy> enemies;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount-1; i++)
        {
            enemies.Add(transform.GetChild(i).GetComponent<MeleeEnemy>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (var item in enemies)
            {
                item.target = other.transform;
            }
        }
    }
}
