using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyLabel : MonoBehaviour
{
    private TMP_Text label;
    private string baseText;

    private void Start()
    {
        label = GetComponent<TMP_Text>();
        baseText = label.text;
        GameManager.instance.onMoneyChanged += UpdateLabel;
        UpdateLabel(GameManager.instance.money);
    }

    private void UpdateLabel(int money)
    {
        label.text = baseText + money.ToString();
    }

    private void OnDestroy()
    {
        GameManager.instance.onMoneyChanged -= UpdateLabel;
    }
}
