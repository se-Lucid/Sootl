using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
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
    private int angerReset = 5;

    private GameObject lightLoc;
    private GameObject lastLightLoc;
    private GameObject enemy;
    [SerializeField] private GameObject player; //put player obj on here
    private NavMeshAgent agent;
    [SerializeField] private float hitRange = 1.0f; 
    [SerializeField] private float angerDst = 18; //change if needed
    [SerializeField] private float angerMax = 30;
    private float speed;
    private float runSpeed;
    private int nextNum;
    private bool angry;
    public bool canAngry;
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
        angerDst = 18;
        angerMax = 30;
        hitRange = 1.0f;
        nextNum = startNum;
        speed = agent.speed;
        runSpeed = speed * 2;
        targeting = Targeting.Patrol;
        angry = false;
        canAngry = true;
        //angerDst = 28;
    }

    
    // Update is called once per frame
    void Update()
    {
        
        if (target != null)
        {
            dstToTarget = Vector3.Distance(target.transform.position, transform.position);
            agent.SetDestination(target.transform.position);
        }

        if (targeting == Targeting.Patrol)
        {
            dstToPlayer = Vector3.Distance(player.transform.position, transform.position);
            Debug.Log(dstToPlayer.ToString());

            agent.speed = speed;
            if (lightLoc != null && lightLoc.activeInHierarchy && 
                Vector3.Distance(lightLoc.transform.position, transform.position) < angerDst)
            {
                LineOfSight(lightLoc);
            }
            if (dstToPlayer < angerDst) //player within angering range
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
            }
        }

        if (targeting == Targeting.Player)
        {
            agent.speed = speed;
            target = player;
            LOSTimer(player);

            if (dstToTarget <= hitRange)
            {
                //do damage or kill or whatever
                Debug.Log("BLEH, YOU DIE");
                ChangeScene.GameOver();
            }
            if (dstToTarget <= angerMax)
            {
                StartCoroutine(AngerReset());
            }
        }

        if (flashlight != null)
        {
            flashlight.GetComponent<INT_Flashlight>().lightTarget = lightLoc;
            if (lightLoc != null)
            {
                lastLightLoc = lightLoc;
            }
        }

        if (targeting == Targeting.Light)
        {
            //check for los
            agent.speed = runSpeed;
            target = lightLoc;
            if (lightLoc == null)
            {
                target = lastLightLoc;
            }
            if (target.transform.position.x == transform.position.x)
            {
                //wait a few seconds and swap to patrol
            }
        }
    }

   
    private void LineOfSight(GameObject obj) //check if player is behind wall
    {
        Debug.Log("Close to Player");

        var rayDirection = obj.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, angerDst))
        {
            Debug.DrawRay(transform.position, rayDirection * angerDst, Color.blue);
            if (player.CompareTag(hit.transform.tag) && canAngry
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
        if (player.CompareTag(obj.tag))
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
    private IEnumerator AngerReset()
    {
        if (angry)
        {
            angerReset = 5;
            canAngry = false;
            angry = false;
            yield return new WaitForSeconds(3);
            canAngry = true;
            targeting = Targeting.Patrol;
        }

    }

    private void LOSTimer(GameObject obj)
    {
        var rayDirection = obj.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, angerDst))
        {
            Debug.DrawRay(transform.position, rayDirection * angerDst, Color.blue);
            if (hit.transform.gameObject.tag == player.tag
                || hit.transform.GetComponent<Light>() == null)
            {
                angerReset = 5;
                return;
            }
            else while (angerReset > 0)
            {
                angerReset--;
                if (angerReset <= 0)
                {
                    StartCoroutine(AngerReset());
                }
            }
        }
    }
}