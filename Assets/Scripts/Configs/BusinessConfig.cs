using UnityEngine;

namespace Configs {
    /// <summary>
    /// ScriptableObject configuration for a single business entity.
    /// Defines all tunable parameters related to progression, income, and upgrades.
    /// Loaded at runtime from the Resources folder.
    /// </summary>
    [CreateAssetMenu(menuName = "Configs/Business")]
    public class BusinessConfig : ScriptableObject {
        [Header("Основная информация")] 
        [Tooltip("Уникальный идентификатор бизнеса, используется в логике и UI.")]
        public int id;
        
        [Tooltip("Отображаемое имя бизнеса в интерфейсе.")]
        public string businessName;

        [Header("Базовые параметры")] 
        [Tooltip("Начальная стоимость открытия или улучшения бизнеса.")]
        public float basePrice;
        
        [Tooltip("Базовый доход, который приносит бизнес на 1 уровне.")]
        public float baseIncome;
        
        [Tooltip("Задержка между циклами получения дохода (в секундах).")]
        public float delay;

        [Header("Улучшение 1")] 
        [Tooltip("Имя первого улучшения, отображаемое в UI.")]
        public string upgrade1Name;
        
        [Tooltip("Процентный множитель дохода при покупке первого улучшения.")]
        public float upgrade1Multiplier;
        
        [Tooltip("Стоимость первого улучшения.")]
        public float upgrade1Price;

        [Header("Улучшение 2")] 
        [Tooltip("Имя второго улучшения, отображаемое в UI.")]
        public string upgrade2Name;
        
        [Tooltip("Процентный множитель дохода при покупке второго улучшения.")]
        public float upgrade2Multiplier;
        
        [Tooltip("Стоимость второго улучшения.")]
        public float upgrade2Price;
    }
}