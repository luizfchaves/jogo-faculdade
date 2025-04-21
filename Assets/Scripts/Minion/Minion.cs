using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour {
    private NavMeshAgent agent;
    private GameObject target;
    public float hitDamage = 10f;
    public string castleTag = "Castle";
    static public float targetSwitchInterval = 2f;

    public float numeroTeste = 1f;

    private float timeSinceLastTargetSwitch = targetSwitchInterval + 1;

    GameObject GetClosestCastle() {
        GameObject[] castles  = GameObject.FindGameObjectsWithTag(castleTag);

        float closestDistance = Mathf.Infinity;
        GameObject closestCastle = null;
        Vector3 currentPosition = transform.position;

        foreach(GameObject castle in castles){
            float distance = Vector3.Distance(currentPosition, castle.transform.position);

            if(distance < closestDistance){
                closestDistance = distance;
                closestCastle = castle;
            }
        }

        Debug.Log("Closest tower: " + " "+closestCastle.transform.position.y + " "+ closestCastle.transform.position.x + " "+ closestCastle.transform.position.z);
        return closestCastle;
    }

    void FindAndSetTarget() {
        timeSinceLastTargetSwitch += Time.deltaTime;
        if(timeSinceLastTargetSwitch >= targetSwitchInterval){
            timeSinceLastTargetSwitch = 0f;
            target = GetClosestCastle();

            if(target == null ){
                if(agent.isOnNavMesh){
                    agent.ResetPath();
                }
                return;
            }
            // Debug.Log("target castle: " + " "+target.transform.position.y + " "+ target.transform.position.x + " "+ target.transform.position.z);

            // Vector3 direction = target.transform.position - transform.position;
            // Debug.Log("direction castle: " + " "+direction.y + " "+ direction.x + " "+ direction.z);
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
            Castle castle = target.GetComponent<Castle>();
            castle.Hit(hitDamage);
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
