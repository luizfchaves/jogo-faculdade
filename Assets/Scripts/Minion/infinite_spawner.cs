using UnityEngine;

public class InfiniteSpawner : MonoBehaviour {
    public float checkRadius = 5f;
    public GameObject minionPrefab;

    private float timeSinceLastCheck = Mathf.Infinity;
    public float checkInterval = 2f;

    private void CheckAndSpawnElf() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius);
        bool enemyFound = false;

        foreach (Collider collider in colliders){
            if (collider.CompareTag("Minion")) {
                enemyFound = true;
                break;
            }
        }

        if (!enemyFound) {
            Instantiate(minionPrefab, transform.position, transform.rotation);
        }
    }

    void Update() {
        timeSinceLastCheck += Time.deltaTime;

        if (timeSinceLastCheck >= checkInterval) {
            timeSinceLastCheck = 0f;
            CheckAndSpawnElf();
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}