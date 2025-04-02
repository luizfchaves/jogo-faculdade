using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour {
    private NavMeshAgent agent;
    private GameObject target;
    public float hitDamage = 10f;
    public string towerTag = "Tower";
    static public float targetSwitchInterval = 2f;

    public float numeroTeste = 1f;

    private float timeSinceLastTargetSwitch = targetSwitchInterval + 1;

    GameObject GetClosestTower() {
        GameObject[] towers  = GameObject.FindGameObjectsWithTag(towerTag);

        float closestDistance = Mathf.Infinity;
        GameObject closestTower = null;
        Vector3 currentPosition = transform.position;

        foreach(GameObject tower in towers){
            float distance = Vector3.Distance(currentPosition, tower.transform.position);

            if(distance < closestDistance){
                closestDistance = distance;
                closestTower = tower;
            }
        }

        Debug.Log("Closest tower: " + " "+closestTower.transform.position.y + " "+ closestTower.transform.position.x + " "+ closestTower.transform.position.z);
        return closestTower;
    }

    void FindAndSetTarget() {
        timeSinceLastTargetSwitch += Time.deltaTime;
        if(timeSinceLastTargetSwitch >= targetSwitchInterval){
            timeSinceLastTargetSwitch = 0f;
            target = GetClosestTower();

            if(target == null ){
                if(agent.isOnNavMesh){
                    agent.ResetPath();
                }
                return;
            }
            Debug.Log("target tower: " + " "+target.transform.position.y + " "+ target.transform.position.x + " "+ target.transform.position.z);

            Vector3 direction = target.transform.position - transform.position;
            Debug.Log("direction tower: " + " "+direction.y + " "+ direction.x + " "+ direction.z);
            agent.ResetPath();
            agent.SetDestination(target.transform.position);
        }
    }

    void CheckIfChegou() {
        if(target == null){
            return;
        }

        Vector3 currentPosition = transform.position;
        float distance = Vector3.Distance(currentPosition, target.transform.position);
        if(distance < numeroTeste){
            Tower tower = target.GetComponent<Tower>();
            tower.Hit(hitDamage);
            Destroy(gameObject);
        }
    }


    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        FindAndSetTarget();
    }

    void FixedUpdate() {
        CheckIfChegou();
    }
}
