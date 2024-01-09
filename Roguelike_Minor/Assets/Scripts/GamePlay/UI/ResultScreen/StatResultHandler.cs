using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StatSettings = Game.ResultScreen.StatSettings;

namespace Game {
    public class StatResultHandler : MonoBehaviour
    {
        //========= util funcs ========
        private void SetupLabels(StatSettings settings, ResultScreen screen, string resultText, float baseScore)
        {
            SetupTitle(settings);
            SetResultText(settings, resultText);
            HandleScore(settings, screen, baseScore);
        }
        private void SetupLabels(StatSettings settings, ResultScreen screen, object result, float baseScore)
        {
            SetupLabels(settings, screen, result.ToString(), baseScore);
        }

        //title
        private void SetupTitle(StatSettings settings)
        {
            settings.statLabel.leftLabel.text = settings.name + ":";
        }

        //result
        private void SetResultText(StatSettings settings, string s)
        {
            settings.statLabel.rightLabel.text = s;
        }

        //score
        private void HandleScore(StatSettings settings, ResultScreen screen, float baseScore)
        {
            int score = Mathf.FloorToInt(baseScore * settings.scoreMult);
            settings.scoreLabel.text = score.ToString();
            //register score
            screen.totalScore += score;
        }

        //================= Stat Handling =================
        //==== Handle progression stats ====
        public void HandleTotalTimeStat(StatSettings settings, ResultScreen screen)
        {
            TimeSpan ts = TimeSpan.FromSeconds(screen.statTracker.runTime);
            SetupLabels(settings, screen, string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds), screen.statTracker.runTime);
        }

        public void HandleStagesCleared(StatSettings settings, ResultScreen screen)
        {
            int stagesCleared = screen.statTracker.totalStagesCleared;
            SetupLabels(settings, screen, stagesCleared, stagesCleared);
        }

        //==== Handle combat stats ====
        public void HandleTotalKills(StatSettings settings, ResultScreen screen)
        {
            int totalKills = screen.statTracker.enemiesKilled;
            SetupLabels(settings, screen, totalKills, totalKills);
        }

        public void HandleDamageDealt(StatSettings settings, ResultScreen screen)
        {
            int totalDamage = Mathf.FloorToInt(screen.statTracker.damageDealt);
            SetupLabels(settings, screen, totalDamage, totalDamage);
        }

        public void HandleMostDamage(StatSettings settings, ResultScreen screen)
        {
            int mostDamage = Mathf.FloorToInt(screen.statTracker.mostDamageDealt);
            SetupLabels(settings, screen, mostDamage, mostDamage);
        }

        public void HandleDamageTaken(StatSettings settings, ResultScreen screen)
        {
            int damageTaken = Mathf.FloorToInt(screen.statTracker.damageTaken);
            SetupLabels(settings, screen, damageTaken, damageTaken);
        }

        public void HandleTotalHealed(StatSettings settings, ResultScreen screen)
        {
            int totalHealed = Mathf.FloorToInt(screen.statTracker.totalHealed);
            SetupLabels(settings, screen, totalHealed, totalHealed);
        }

        //==== Handle Inventory Stats ====
        public void HandleItemsCollected(StatSettings settings, ResultScreen screen)
        {
            int itemsCollected = screen.statTracker.totalItemsCollected;
            SetupLabels(settings, screen, itemsCollected, itemsCollected);
        }

        public void HandleSlotsCollected(StatSettings settings, ResultScreen screen)
        {
            int slotsCollected = screen.statTracker.totalSlotsCollected;
            SetupLabels(settings, screen, slotsCollected, slotsCollected);
        }

        //==== Handle economy stats ====
        public void HandleTotalPurchases(StatSettings settings, ResultScreen screen)
        {
            int purchases = Mathf.FloorToInt(screen.statTracker.totalPurchases);
            SetupLabels(settings, screen, purchases, purchases);
        }

        public void HandleTotalMoney(StatSettings settings, ResultScreen screen)
        {
            int totalMoney = Mathf.FloorToInt(screen.statTracker.totalMoneyCollected);
            SetupLabels(settings, screen, totalMoney, totalMoney);
        }

        public void HandleMoneySpent(StatSettings settings, ResultScreen screen)
        {
            int moneySpent = Mathf.FloorToInt(screen.statTracker.totalMoneySpent);
            SetupLabels(settings, screen, moneySpent, moneySpent);
        }
    }
}
