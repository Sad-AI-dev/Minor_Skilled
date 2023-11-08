using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game {
    public class ShopReroller : MonoBehaviour
    {
        [SerializeField] private Shop shop;
        [SerializeField] private TMP_Text rerollsLabel;

        //vars
        private string baseText;

        private void Start()
        {
            baseText = rerollsLabel.text;
            UpdateLabel();
        }

        public void UpdateLabel()
        {
            rerollsLabel.text = baseText + shop.rerolls;
        }
    }
}
