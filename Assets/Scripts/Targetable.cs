using UnityEngine;

[RequireComponent(typeof(Health))]
public class Targetable : MonoBehaviour {
    Health health;

    public void TakeDamage(float normalDamage, float poisonDamage) {
        health.TakeDamage(normalDamage);
        health.ApplyPoisonDamage(poisonDamage);
    }

    void Awake() {
        health = GetComponent<Health>();
    }
}