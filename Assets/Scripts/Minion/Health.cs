using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
  [Header("Health")]
  public float maxHealth;
  public Slider slider;
  public float health;

  [Header("Poison")]
  private float poisonDamageTickRateSec = 1f;
  private float nextPoisonTickTime;
  [SerializeField] private float poisonDamageStack = 0f;
  private float poisonDamagePercentage = 0.4f;
  private Color poisonFlashColor = Color.green;
  private float flashDurationSec = 0.2f;
  private Renderer objectRenderer;
  private Color originalColor;
  private Coroutine flashCoroutine;

  [Header("UI Settings")]
  [SerializeField] private GameObject healthBarPrefab;
  [SerializeField] private Vector3 healthBarOffset = new Vector3(0, 1.5f, 0);



  public void TakeDamage(float damage) {
    health -= damage;

    slider.value = health;

    if (health <= 0){
      Destroy(gameObject);
      // add 10 coins to gameManager
      GameManager gameManager = FindFirstObjectByType<GameManager>();
      if (gameManager != null) {
        gameManager.handleMinionDeath();
      }
    }
  }

  public void ApplyPoisonDamage(float damage){
    poisonDamageStack+= damage;
  }

  IEnumerator FlashColorCoroutine() {
    objectRenderer.material.color = poisonFlashColor;

    yield return new WaitForSeconds(flashDurationSec);

   
    if (flashCoroutine != null) {
       objectRenderer.material.color = originalColor;
       flashCoroutine = null;
    }
  }


  void HandlePoisonDamage(){
    if(poisonDamageStack <= 0){
      return;
    }

    if (Time.time <= nextPoisonTickTime) {
      return;
    }

    float currentDamageAmount = Mathf.Max(poisonDamageStack * poisonDamagePercentage, 1);
    poisonDamageStack = Mathf.Max(poisonDamageStack - currentDamageAmount,0);
    TakeDamage(currentDamageAmount);
    flashCoroutine = StartCoroutine(FlashColorCoroutine());
    nextPoisonTickTime =  Time.time + poisonDamageTickRateSec;
  }

  void Start() {
    health = maxHealth;

    objectRenderer = GetComponentInChildren<Renderer>();
    originalColor = objectRenderer.material.color;

    nextPoisonTickTime = Time.time;
  }

  void Update(){
    HandlePoisonDamage(); 
  }
}