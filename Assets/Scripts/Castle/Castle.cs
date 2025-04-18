using UnityEngine;

public class Castle : MonoBehaviour {
    public float currentHealth = 100f;

  
    void TakeDamage(float damage) {
        currentHealth -= damage;

        if (currentHealth <= 0){
            Destroy(gameObject);
        }
    }
    public void Hit(float damage){
        TakeDamage(damage);
    }

    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }
}
