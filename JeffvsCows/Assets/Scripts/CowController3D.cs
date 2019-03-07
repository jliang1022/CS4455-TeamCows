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
    public GameObject player;
    GameObject fov;
    AllCows allCows;
    float alertDist;

    // Flocking
    float neighborTooCloseDist;

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
        allCows = GameObject.Find("AllCows").GetComponent<AllCows>();
        alertDist = 20f;

        neighborDist = 10f;
        neighborTooCloseDist = 0.5f;

        turnTime = 0.5f;
        lowerTurnDeg = -turnDeg / 2f;
        upperTurnDeg = turnDeg / 2f;
    }

    void Update()
    {
        if (fov.GetComponent<FOVController>().CanSeePlayer())
            state = CowState.PlayerSeen;

        switch(state)
        {
            case CowState.Idle:
                anim.SetBool("Walking", false);
                if (type == CowType.Turning)
                {
                    if (turnTimeLeft > 0)  // Turning
                    {
                        Debug.Log("Turning");
                        float turnIncrement = turnDist / turnTime;
                        transform.Rotate(new Vector3(0, turnIncrement * Time.deltaTime, 0));
                        turnTimeLeft -= Time.deltaTime;
                        if (turnTimeLeft <= 0)
                            timeLeftBeforeTurn = turnWaitTime;
                    }
                    else if (timeLeftBeforeTurn <= 0)  // Initiate turning
                    {
                        Debug.Log("Starting turn");
                        turnDist = CalculateStationaryTurnAmount();
                        turnTimeLeft = turnTime; 
                    }
                    else  // waiting to turn
                    {
                        Debug.Log("Waiting to turn");
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
                    cow.GetComponent<CowController3D>().state = CowState.AttackPlayer;
                }
                break;
            case CowState.AttackPlayer:
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
}
