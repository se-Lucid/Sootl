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
    public GameObject[] storyTargets;
    public GameObject target;
    public GameObject flashlight;

    public float dstToTarget;
    public float dstToPlayer = 200;
    public int startNum = 0; //change if needed
    public int angerReset = 5;

    public GameObject lightLoc;
    private GameObject lastLightLoc;
    private GameObject enemy;
    [SerializeField] private GameObject player; //put player obj on here
    [SerializeField] private DynamicBillboarding faces;
    private NavMeshAgent agent;
    [SerializeField] private float hitRange = 10f; 
    [SerializeField] private float angerDst = 12; //change if needed
    [SerializeField] private float angerMax = 20;
    private float speed;
    private float runSpeed;
    private int nextNum;
    private bool angry;
    public bool canAngry;
    public enum Targeting
    {
        Patrol,
        Player,
        Light,
        Story
    }
    public Targeting targeting;
    
    void OnTriggerEnter(Collider other)
{
        if (targeting == Targeting.Light && other.gameObject.GetComponent<Destructable>())
        {
            other.gameObject.GetComponent<Destructable>().BlowUp();
        }
        //if (targeting == Targeting.Story && col.gameObject.CompareTag("StoryDestruct"))
        {
            //col.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        faces = FindAnyObjectByType<DynamicBillboarding>();
        enemy = gameObject;
        agent = gameObject.GetComponent<NavMeshAgent>();
        nextNum = startNum;
        speed = agent.speed;
        runSpeed = speed * 2f;
        targeting = Targeting.Patrol;
        angry = false;
        canAngry = true;
    }

    
    void Update()
    {
        
        if (target != null)
        {
            dstToTarget = Vector3.Distance(target.transform.position, transform.position);
            agent.SetDestination(target.transform.position);
        }
        if (flashlight != null)
        {
            lightLoc = flashlight.GetComponent<INT_Flashlight>().lightTarget;
            if (lightLoc != null)
            {
                lastLightLoc = lightLoc;
            }
        }

        if (targeting == Targeting.Patrol)
        {
            dstToPlayer = Vector3.Distance(player.transform.position, transform.position);
            //Debug.Log(dstToPlayer.ToString());

            agent.speed = speed;
            if (lightLoc != null 
                && lightLoc.activeInHierarchy 
                && Vector3.Distance(lightLoc.transform.position, transform.position) < angerDst
                && LineOfSight(lightLoc)
                )
            {
                StartCoroutine(GetAngry(lightLoc));

            }
            if (dstToPlayer < angerDst && LineOfSight(player)) //player within angering range
            {
                Debug.Log("something");
                StartCoroutine(GetAngry(player));
            }

            if (targets.Length != 0 && target != targets[nextNum]) //patrolling system
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

        else if (targeting == Targeting.Player)
        {
            agent.speed = runSpeed;
            dstToPlayer = Vector3.Distance(player.transform.position, transform.position);
            target = player;
            LOSTimer(player);
            if (dstToTarget >= angerMax)
            {
                StartCoroutine(AngerReset());
            }
        }

        else if (targeting == Targeting.Story)
        {
            agent.speed = runSpeed;
        }

        else if (targeting == Targeting.Light)
        {
            agent.speed = runSpeed; 
            if (!lightLoc.activeInHierarchy)
            {
                lastLightLoc = lightLoc;
            }
            else
            {
                StartCoroutine(AngerReset(4));
            }
            LOSTimer(lastLightLoc);
            target = lastLightLoc;
        }
        if (dstToPlayer <= hitRange)
        {
            //do damage or kill or whatever
            Debug.Log("BLEH, YOU DIE");
            ChangeScene.GameOver();
        }
    }

   
    private bool LineOfSight(GameObject obj) //check if player is behind wall
    {
        var rayDirection = obj.transform.position - transform.position;
        Debug.DrawRay(transform.position, rayDirection * angerDst, Color.yellow);
        if (
            Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, angerDst) 
            && (player.CompareTag(hit.transform.tag) || hit.transform.gameObject == lightLoc)
            //&& canAngry
            )
        {
            return true;
        }
        return false;
    }

    private IEnumerator GetAngry(GameObject obj)
    {
        agent.speed = runSpeed;
        Debug.Log("evil");
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
    private IEnumerator AngerReset(int timer = 3)
    {
        if (angry)
        {
            canAngry = false;
            angry = false;
            Debug.Log("anger no more");
            foreach (SpriteRenderer face in faces.sides)
            {
                face.gameObject.GetComponent<Animator>().SetBool("Lost", true);
            }
            angerReset = 5;
            speed = 0;
            yield return new WaitForSeconds(timer); 
            foreach (SpriteRenderer face in faces.sides)
            {
                face.gameObject.GetComponent<Animator>().SetBool("Lost", false);
            }
            speed = agent.speed;
            canAngry = true;
            targeting = Targeting.Patrol;
        }

    }

    private void LOSTimer(GameObject obj)
    {
        var rayDirection = obj.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, dstToTarget))
        {
            //Debug.DrawRay(transform.position, rayDirection * dstToTarget, Color.blue);
            if (LineOfSight(obj))
            {
                angerReset = 2;
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

    private void StorySetter()
    {
        targeting = Targeting.Story;
    }

}