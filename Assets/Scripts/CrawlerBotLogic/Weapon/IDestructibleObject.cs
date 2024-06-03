using UnityEngine;

namespace CrawlerBotLogic.Weapon {
    public interface IDestructibleObject {
        public float health { get; set;  }

        public void TakeDamage(float amount);
        public void Die();
    }
}