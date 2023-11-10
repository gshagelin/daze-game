using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Baracuda.Monitoring;
[MFontSize(12)]

public class enemyController : MonitoredBehaviour
{
    //x main phases 
        //Idle: Haven't spotted player.
        //Chase: Direct sight on player, runs straight towards them.
        //Hunt: After chase phase if sight on player is lost or after idle phase if it hears the player. 
        //Investigates position where it last saw/heard the player. Begins walking around the area afterwards. 
        //After a selected period of time in the hunt period, it will call reinforcements.
        //Reinforce: After existing enemy in hunt phase has called reinforcement. Spawns new enemies that marches to location
        //where reinforcements where called from. Will enter hunt or chase phase if player is seen or heard along the way. 

        public GameObject enemyVision;
        public GameObject pointOfSight;
        public GameObject player;
        public float visionDistance;
        public float visionAngle;

        private Vector3 rayDirection;
        public float runSpeed;
        public float walkSpeed;
        public float maxRotationSpeed;

        private bool canSeePlayer;
        [Monitor]
        private string activeMode;
        [Monitor]
        private float extendedChaseTimer = 3f;
        private bool canExtendChase = false;

        private UnityEngine.AI.NavMeshAgent agent;
        private float defaultAgentStop;
        [Monitor]
        private bool hasCheckedLastPos = false;

        void Start () {
            activeMode = "idle";
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            defaultAgentStop = agent.stoppingDistance;

        }
        void Update () {
            RaycastHit hit;
            rayDirection = pointOfSight.transform.position - enemyVision.transform.position;
            if (Physics.Raycast(enemyVision.transform.position, rayDirection, out hit, visionDistance) && hit.collider.gameObject.name == "playerManager") {
                float dotProduct = Vector3.Dot((player.transform.position - enemyVision.transform.position).normalized, enemyVision.transform.forward.normalized);
                if (dotProduct >= 0.5f) {
                    Debug.DrawRay(enemyVision.transform.position, rayDirection * 10, Color.green);
                    extendedChaseTimer = 3f;
                    chaseState();
                } else {
                    Debug.DrawRay(enemyVision.transform.position, rayDirection * 10, Color.red);
                }
            } else {
                if (canExtendChase == true) {
                    extendedChaseTimer -= Time.deltaTime;
                    if (extendedChaseTimer >= 0f) {
                        chaseState();
                    } else {
                        Debug.Log("what");
                        canExtendChase = false;
                        if (hasCheckedLastPos == false) {
                            huntState ();
                        }
                    }
                }
            }
        }

        void chaseState () {
            agent.stoppingDistance = defaultAgentStop;
            canExtendChase = true;
            activeMode = "chase";
            agent.speed = runSpeed;
            
            Vector3 distanceToPlayer = player.transform.position - transform.position;
            agent.destination = player.transform.position;

            gameObject.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (player.transform.position - transform.position), maxRotationSpeed * Time.deltaTime, 0f));
            hasCheckedLastPos = false;
        }

        void attackingPlayer () {
            Debug.Log("attacking");
        }
        Vector3 lastKnownPos;
        void huntState () {
            delay(3);
            agent.stoppingDistance = 0;
            agent.speed = walkSpeed;
            activeMode = "hunt";
    	    lastKnownPos = player.transform.position;
            huntPos();
            hasCheckedLastPos = true;
        }
        IEnumerator huntPos () {                                 
            while ((player.transform.position - transform.position).magnitude > 3f) {
                agent.destination = lastKnownPos;
                yield return null;
            }
        }
        void delay (float delay) {
            float timer =+ Time.deltaTime;
            if (timer >= delay) {
                return;
            }
        }
}
