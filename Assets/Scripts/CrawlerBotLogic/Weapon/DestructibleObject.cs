using UnityEngine;

namespace CrawlerBotLogic.Weapon {
    public class DestructibleObject : MonoBehaviour {
        [field: SerializeField]
        public float health { get; set; }

        public GameObject destructionFXPrefab;
        
        public void Die() {
            Instantiate(destructionFXPrefab,transform.position,transform.rotation);
            Destroy(gameObject);
        }

        public void TakeDamage(float amount) {
            health -= amount;
            if (health <= 0) {
                health = 0;
                Die();
            }
        }
    }
}