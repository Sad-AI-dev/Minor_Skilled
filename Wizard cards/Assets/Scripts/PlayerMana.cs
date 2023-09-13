using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RawImage manaBar;
    [SerializeField] private Text text;
    private float maxManaBarLength;
    private float manaBarChunk;
    private float currentManaBarLength;

    [Header("Mana")]
    public int totalMana;
    [SerializeField] private float regenInterval;
    [SerializeField] private int regenAmount;
    private int mana;

    private bool canRegen = true;

    private void Start()
    {
        maxManaBarLength = manaBar.transform.localScale.x;
        currentManaBarLength = maxManaBarLength;
        manaBarChunk = maxManaBarLength / totalMana;
        mana = totalMana;
    }

    private void Update()
    {
        if(canRegen && mana < totalMana)
        {
            StartCoroutine(RegenerateMana());
        }
    }

    public void UseMana(int manaUsed)
    {
        mana -= manaUsed;

        if (mana <= 0) mana = 0;

        text.text = $"{mana} / 100";

        manaBar.transform.localScale = new Vector3(manaBar.transform.localScale.x - (manaBarChunk * manaUsed), manaBar.transform.localScale.y, manaBar.transform.localScale.z);


    }

    IEnumerator RegenerateMana()
    {
        canRegen = false;
        mana += regenAmount;
        if (mana >= totalMana) mana = totalMana;
        text.text = $"{mana} / 100";
        manaBar.transform.localScale = new Vector3(manaBar.transform.localScale.x + (manaBarChunk * regenAmount), manaBar.transform.localScale.y, manaBar.transform.localScale.z);
        yield return new WaitForSeconds(regenInterval);
        canRegen = true;
    }

    public int getMana()
    {
        return mana;
    }
}
