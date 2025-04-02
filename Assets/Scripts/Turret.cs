using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
  [Header("Targeting Parameters")]
  public float detectionRadius = 5f;
  private GameObject closestTarget = null;


  [Header("Weapon Parameters")]
  public float normalDamage = 100f;
  public float poisonDamage = 0f;
  public GameObject shootFrom;
  private LineRenderer lineRenderer;
  public float shotInterval = 0.1f;
  private float nextShotTime;


  [Header("Visuals")]
  public bool canRotate = false;
  public float rotationSpeed = 5f;
  public Color shotColor = Color.red;


  [Header("Recoil Parameters")]
  public float recoilDistance = 0.2f;
  public float recoilDuration = 0.1f;
  private bool isRecoiling = false;
  private Vector3 originalPosition;
  private Coroutine recoilCoroutine = null;

  void InitializeLineRenderer() {
    lineRenderer = GetComponent<LineRenderer>();
    if (lineRenderer == null) {
      lineRenderer = gameObject.AddComponent<LineRenderer>();

      lineRenderer.enabled = false;
      lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
      lineRenderer.startWidth = 0.1f;
      lineRenderer.endWidth = 0.1f;
      lineRenderer.positionCount = 2;
    }

    if (shootFrom == null) {
      Debug.LogError("Target Object not assigned!");
    }
  }

  void FindClosestTargetable() {
    Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

    closestTarget = null;
    float closestDistance = Mathf.Infinity;

    foreach (Collider collider in colliders) {
      Targetable targetComponent = collider.gameObject.GetComponent<Targetable>();

      if (targetComponent  != null) {
        float distance = Vector3.Distance(transform.position, collider.transform.position);

        if (distance < closestDistance) {
          closestDistance = distance;
          closestTarget = collider.gameObject;
        }

      }
    }
  }

  void RotateTowardsTarget() {
    if(!canRotate){
      return;
    }

    if (closestTarget) {
      Vector3 targetDirection = closestTarget.transform.position - shootFrom.transform.position;
      targetDirection.y = 0;

      if (targetDirection != Vector3.zero) {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,rotationSpeed * Time.deltaTime);
      }
    }
  }

  void DealDamageToTarget() {
    if (closestTarget != null){
      Targetable target = closestTarget.GetComponent<Targetable>();
      
      nextShotTime = Time.time + shotInterval;

      if (target != null) {
        UpdateLineRenderer(closestTarget);
        target.TakeDamage(normalDamage, poisonDamage);

        if (recoilCoroutine != null) {
          StopCoroutine(recoilCoroutine);
          transform.position = originalPosition;
        }
        recoilCoroutine = StartCoroutine(ApplyRecoil());

      }
    }
  }

  IEnumerator ApplyRecoil() {
    isRecoiling = true;
    Vector3 recoilTargetPosition = originalPosition - transform.forward * recoilDistance;
    float halfDuration = recoilDuration / 2.0f;
    float elapsedTime = 0f;

    while (elapsedTime < halfDuration) {
      transform.position = Vector3.Lerp(originalPosition, recoilTargetPosition,elapsedTime / halfDuration);
      elapsedTime += Time.deltaTime;
      yield return null;
    }
    transform.position = recoilTargetPosition;
   
    elapsedTime = 0f;
    while (elapsedTime < halfDuration) {
      transform.position = Vector3.Lerp(recoilTargetPosition, originalPosition,elapsedTime / halfDuration);
      elapsedTime += Time.deltaTime;
      yield return null;
    }
   
    transform.position = originalPosition;
    isRecoiling = false;
    recoilCoroutine = null;
  }

  IEnumerator EmulateShotVisuals() {
    lineRenderer.enabled = true;

    lineRenderer.startColor = shotColor;
    lineRenderer.endColor = shotColor;

    yield return new WaitForSeconds(0.05f);


    lineRenderer.enabled = false;
  }

  void UpdateLineRenderer(GameObject closestTarget) {
    StartCoroutine(EmulateShotVisuals());
    lineRenderer.SetPosition(0, shootFrom.transform.position);
    lineRenderer.SetPosition(1, closestTarget.transform.position);
  }

  void Start() {
    InitializeLineRenderer();
    originalPosition = transform.position;
  }

  void Update() {
    if (!isRecoiling) {
      originalPosition = transform.position;
    }
    FindClosestTargetable();
    RotateTowardsTarget();

    if (Time.time >= nextShotTime) {
        DealDamageToTarget();
    }
  }

  private void OnDrawGizmosSelected() {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(transform.position, detectionRadius);
  }
}
