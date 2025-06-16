using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Data {
    /// <summary>
    /// Container for shared UI references used across ECS systems to update the interface.
    /// Holds lists of UI elements related to balance, businesses, and upgrades.
    /// </summary>
    public class SharedUIData {

        [Header("Balance View Info")] 
        public TMP_Text BalanceText;  // UI text displaying player's balance

        [Header("Business View Info")] 
        public List<TMP_Text> BusinessNameText;        // List of business name texts
        public List<TMP_Text> BusinessLevelText;       // List of business level texts
        public List<TMP_Text> BusinessIncomeText;      // List of business income texts
        public List<TMP_Text> BusinessLevelUpText;     // List of level-up price texts
        public List<Image> BusinessProgressBar;        // List of business progress bar images

        [Header("Upgrade 1 View Info")] 
        public List<TMP_Text> Upgrade1NameText;        // List of first upgrade name texts
        public List<TMP_Text> Upgrade1IncomeRaiseText; // List of first upgrade income increase texts
        public List<TMP_Text> Upgrade1PriceText;       // List of first upgrade price texts

        [Header("Upgrade 2 View Info")] 
        public List<TMP_Text> Upgrade2NameText;        // List of second upgrade name texts
        public List<TMP_Text> Upgrade2IncomeRaiseText; // List of second upgrade income increase texts
        public List<TMP_Text> Upgrade2PriceText;       // List of second upgrade price texts
    }
}