using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] targets;
    public GameObject target;
    public float dstToTarget;
    public float dstToPlayer;
    public int startNum = 3; //change this if u want

    private GameObject enemy;
    [SerializeField] private GameObject player; //put player obj on here
    private NavMeshAgent agent;
    [SerializeField] private float hitRange;
    [SerializeField] private float angerDst;
    [SerializeField] private float angerMax;
    private float speed;
    private int nextNum;
    private bool angry;
    private bool canAngry;
    public enum Targeting
    {

        Patrol,
        Player,
        Light
    }
    public Targeting targeting;

    void Start()
    {
        enemy = gameObject;
        agent = gameObject.GetComponent<NavMeshAgent>();
        hitRange = 8.0f;
        nextNum = startNum;
        speed = agent.speed;
        targeting = Targeting.Patrol;
        angry = false;
        canAngry = true;
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = speed;
        if (target != null)
        {
            DistanceToTarget(target);
        }

        if (targeting == Targeting.Patrol)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < angerDst) //player within angering range
            {
                PlayerLOS();
            }

            if (targets.Length != 0 && target == null) //patrolling system
            {
                target = targets[startNum];
                agent.SetDestination(target.transform.position);
            }
            if (dstToTarget <= hitRange)
            {
                nextNum++;
                if (nextNum <= targets.Length - 1)
                {
                    
                    
                }
                else
                {
                    nextNum = 0;
                }
                target = targets[nextNum];
                agent.SetDestination(target.transform.position);
            }
        }

        if (targeting == Targeting.Player)
        {
            target = player;
            agent.SetDestination(target.transform.position);

            if (dstToTarget <= hitRange)
            {
                //do damage or kill or whatever
            }
            if (dstToTarget <= angerMax)
            {
                StartCoroutine(AngerTimer());
            }
        }
    }

    private void DistanceToTarget(GameObject target) //for patrolling
    {
        if (target == null)
            return;
        else if (target == player)
        {
            dstToPlayer = new Vector3(enemy.transform.position.x - target.transform.position.x,
            enemy.transform.position.y - target.transform.position.y,
            enemy.transform.position.z - target.transform.position.z).magnitude;
        }
        else
        {
            dstToTarget = new Vector3(  enemy.transform.position.x - target.transform.position.x,
                                        enemy.transform.position.y - target.transform.position.y,
                                        enemy.transform.position.z - target.transform.position.z).magnitude;
        }
    }
    private void PlayerLOS() //check if player is behind wall
    {
        RaycastHit hit;
        var rayDirection = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {

            if (hit.transform == player && canAngry)
            {
                StartCoroutine(GetAngry());
            }
            else
            {
                return;
            }

        }
    }

    private IEnumerator GetAngry()
    {
        yield return new WaitForSeconds(0); //in case you want an animation in here or something
        targeting = Targeting.Player;
    }
    private IEnumerator AngerTimer()
    {
        if (angry)
        {
            canAngry = false;
            angry = false;
            yield return new WaitForSeconds(3);
            canAngry = true;
        }

    }
}