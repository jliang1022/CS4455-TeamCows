using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CowController3D : MonoBehaviour
{
    public CowType type;

    // Turning Cow
    public float turnDeg, turnWaitTime, numOfTurns;
    float lowerTurnDeg, upperTurnDeg, turnTime, turnTimeLeft, turnDist;
    bool turningRight;
    float timeLeftBeforeTurn, currentOrientation;

    NavMeshAgent nmAgent;
    Animator anim;
    public CowState state;
    GameObject player;
    GameObject fov;
    AllCows allCows;
    float alertDist, timeBeforeLosePlayer, timeBeforeLosePlayerLeft, killDist;

    // Flocking
    float neighborTooCloseDist;
    AudioSource cowAlertMoo;
    AudioSource[] cowSounds;
    bool alerted;

    public enum CowType
    {
        Stationary, Turning, Moving
    }

    public enum CowState
    {
        Idle, InvestigateSound, PlayerSeen, AttackPlayer
    }

    void Start()
    {
        state = CowState.Idle;
        nmAgent = GetComponent<NavMeshAgent>();
        fov = transform.GetChild(1).gameObject;
        anim = transform.GetChild(0).GetComponent<Animator>();
        allCows = GameObject.Find("GlobalCowHolder").GetComponent<AllCows>();
        alertDist = 20f;
        player = GameObject.Find("Player");
        timeBeforeLosePlayer = 5f;
        killDist = 3.5f;

        neighborTooCloseDist = 0.5f;

        turnTime = 0.5f;
        lowerTurnDeg = -turnDeg / 2f;
        upperTurnDeg = turnDeg / 2f;
        cowSounds = GetComponents<AudioSource>();
        cowAlertMoo = cowSounds[0];
    }

    void Update()
    {
        if (fov.GetComponent<FOVController>().CanSeePlayer())
        {
            state = CowState.PlayerSeen;
        }

        CanSeePlayer();

        switch(state)
        {
            case CowState.Idle:
                anim.SetBool("Walking", false);
                if (type == CowType.Turning)
                {
                    if (turnTimeLeft > 0)  // Turning
                    {
                        //Debug.Log("Turning");
                        float turnIncrement = turnDist / turnTime;
                        transform.Rotate(new Vector3(0, turnIncrement * Time.deltaTime, 0));
                        turnTimeLeft -= Time.deltaTime;
                        if (turnTimeLeft <= 0)
                            timeLeftBeforeTurn = turnWaitTime;
                    }
                    else if (timeLeftBeforeTurn <= 0)  // Initiate turning
                    {
                        //Debug.Log("Starting turn");
                        turnDist = CalculateStationaryTurnAmount();
                        turnTimeLeft = turnTime; 
                    }
                    else  // waiting to turn
                    {
                        //Debug.Log("Waiting to turn");
                        timeLeftBeforeTurn -= Time.deltaTime;
                    }
                }
                break;
            case CowState.InvestigateSound:
                break;
            case CowState.PlayerSeen:
                state = CowState.AttackPlayer;
                foreach (GameObject cow in allCows.cows)
                {
                    if (cow.GetComponent<CowController3D>().CanSeePlayer())
                        cow.GetComponent<CowController3D>().state = CowState.AttackPlayer;
                }
                alerted = true;
                break;
            case CowState.AttackPlayer:
                if (alerted)
                {
                    cowAlertMoo.volume = 0.9f;
                    cowAlertMoo.Play();
                    alerted = false;
                }
                anim.SetBool("Walking", true);
                Vector3 playerLoc = player.transform.position + player.GetComponent<PlayerController3D>().GetVelocity();
                Vector3 avoidVec = Vector3.zero;

                foreach (GameObject cow in allCows.cows)
                {
                    if (cow != gameObject)
                    {
                        float dist = Vector3.Distance(transform.position, cow.transform.position);
                        if (dist < neighborTooCloseDist)
                            avoidVec += cow.transform.position - transform.position;
                    }
                }

                avoidVec = avoidVec.normalized * (1 / neighborTooCloseDist);

                if (CanSeePlayer())
                {
                    timeBeforeLosePlayerLeft = timeBeforeLosePlayer;
                    CheckPlayerCaught();
                }
                else
                {
                    timeBeforeLosePlayerLeft -= Time.deltaTime;
                }

                if (timeBeforeLosePlayerLeft <= 0)
                    state = CowState.Idle;

                Vector3 dest = playerLoc + avoidVec;
                nmAgent.SetDestination(dest);
                break;
        }
    }

    float CalculateStationaryTurnAmount()
    {
        float turnAmount = turningRight ? turnDeg / numOfTurns : -turnDeg / numOfTurns;

        if (currentOrientation + turnAmount < lowerTurnDeg)
        {
            turnAmount = lowerTurnDeg - currentOrientation;
            currentOrientation = lowerTurnDeg;
            turningRight = true;
        }
        else if (currentOrientation + turnAmount > upperTurnDeg)
        {
            turnAmount = upperTurnDeg - currentOrientation;
            currentOrientation = upperTurnDeg;
            turningRight = false;
        }
        else
        {
            currentOrientation += turnAmount;
        }

        //Debug.Log(currentOrientation);
        return turnAmount;
    }

    public bool CanSeePlayer()
    {
        RaycastHit hit;
        bool canSeePlayer = false;
        Vector3 dirOfPlayer = player.transform.position - transform.position;
        dirOfPlayer = dirOfPlayer.normalized;
        dirOfPlayer = Quaternion.Inverse(transform.rotation) * dirOfPlayer;
        Physics.Raycast(transform.position, transform.TransformDirection(dirOfPlayer), out hit, Mathf.Infinity);
        Debug.DrawRay(transform.position, transform.TransformDirection(dirOfPlayer) * hit.distance, Color.red);
        if (hit.collider != null)
        {
            canSeePlayer = hit.collider.CompareTag("Player");
        }
        return canSeePlayer;
    }

    void CheckPlayerCaught()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < killDist)
        {
            GameObject.Find("GlobalControl").GetComponent<GlobalController>().KillPlayer();
            foreach (GameObject cow in allCows.cows)
            {
                cow.GetComponent<CowController3D>().state = CowState.Idle;
            }
        }
    }
}
