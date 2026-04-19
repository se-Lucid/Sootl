using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] targets;
    public GameObject target;
    public GameObject flashlight;

    public float dstToTarget;
    public float dstToPlayer;
    public int startNum = 0; //change if needed

    private GameObject lightLoc;
    private GameObject lastLightLoc;
    private GameObject enemy;
    [SerializeField] private GameObject player; //put player obj on here
    private NavMeshAgent agent;
    [SerializeField] private float hitRange;
    [SerializeField] private float angerDst; //change if needed
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
        hitRange = 2.0f;
        nextNum = startNum;
        speed = agent.speed;
        targeting = Targeting.Patrol;
        angry = false;
        canAngry = true;
        //angerDst = 28;
    }

    // Update is called once per frame
    void Update()
    {
        
        agent.speed = speed;
        if (target != null)
        {
            DistanceToTarget(target);
            agent.SetDestination(target.transform.position);
        }

        if (targeting == Targeting.Patrol)
        {
            if (lightLoc != null && lightLoc.activeInHierarchy && 
                Vector3.Distance(lightLoc.transform.position, transform.position) < angerDst)
            {
                LineOfSight(lightLoc);
            }
            if (Vector3.Distance(player.transform.position, transform.position) < angerDst) //player within angering range
            {
                LineOfSight(player);
            }

            if (targets.Length != 0 && target == null) //patrolling system
            {
                target = targets[nextNum];
            }
            if (dstToTarget <= hitRange)
            {
                nextNum++;
                if (nextNum > targets.Length - 1)
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
            //speed = agent.speed * 1.5f;

            if (dstToTarget <= hitRange)
            {
                //do damage or kill or whatever
                Debug.Log("BLEH, YOU DIE");
            }
            if (dstToTarget <= angerMax)
            {
                StartCoroutine(AngerTimer());
            }
        }

        flashlight.GetComponent<INT_Flashlight>().lightTarget = lightLoc;
        lastLightLoc = lightLoc;
        if (targeting == Targeting.Light)
        {
            target = lightLoc;
            if (target.transform.position.x == transform.position.x)
            {
                //do something
            }
        }
    }

    private void DistanceToTarget(GameObject target) //for patrolling
    {
        if (target == null)
            return;
        else if (target == player)
        {
            dstToPlayer = new Vector3(  enemy.transform.position.x - target.transform.position.x,
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
    private void LineOfSight(GameObject obj) //check if player is behind wall
    {
        Debug.Log("Close to Player");

        RaycastHit hit;
        var rayDirection = obj.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit, angerDst))
        {
            Debug.DrawRay(transform.position, rayDirection * angerDst, Color.blue);
            if (hit.transform.gameObject.tag == player.tag && canAngry
                || hit.transform.GetComponent<Light>() != null && canAngry)
            {
                StartCoroutine(GetAngry(obj));
            }
            else 
            {
                return;
            }

        }
    }

    private IEnumerator GetAngry(GameObject obj)
    {
        if (obj.transform.gameObject.tag == player.tag)
        {
            targeting = Targeting.Player;
        }
        else
        {
            targeting = Targeting.Light;
        }
        angry = true;
        yield return new WaitForSeconds(0); //in case you want an animation in here or something
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