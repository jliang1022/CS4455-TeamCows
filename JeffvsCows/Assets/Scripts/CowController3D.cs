using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CowController3D : MonoBehaviour
{
    public CowType type;
    Vector3 defaultPos;
    Quaternion defaultRot;
    float rotateSpeed = 10f;

    // Turning Cow
    public float turnDeg, turnWaitTime, numOfTurns;
    float lowerTurnDeg, upperTurnDeg, turnTime, turnTimeLeft, turnDist;
    bool turningRight;
    float timeLeftBeforeTurn, currentOrientation;

    // Moving Cow
    public GameObject[] waypoints;
    Vector3[] path;
    int currPathNode;

    NavMeshAgent nmAgent;
    Animator anim;
    public CowState state;
    GameObject player;
    GameObject fov;
    GameObject[] allCows;
    GameObject sourceOfSound;
    float alertDist, timeBeforeLosePlayer, timeBeforeLosePlayerLeft, killDist, stoppingDist, eyesightDist;

    float soundInvestigateTime, soundInvestigateTimeLeft;

    float alertTime = 1f, alertTimeLeft;

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
        Idle, InvestigateSound, PlayerSeen, AttackPlayer, ReturnToDefault
    }

    void Start()
    {
        defaultPos = transform.position;
        defaultRot = transform.rotation;

        path = new Vector3[waypoints.Length + 1];
        path[0] = defaultPos;
        for (int i = 0; i < waypoints.Length; i++)
            path[i + 1] = waypoints[i].transform.position;

        state = CowState.Idle;
        nmAgent = GetComponent<NavMeshAgent>();
        fov = transform.GetChild(1).gameObject;
        anim = transform.GetChild(0).GetComponent<Animator>();
        allCows = GameObject.Find("GlobalControl").GetComponent<GlobalController>().cows;
        alertDist = 20f;

        player = GameObject.Find("Player");
        if (player == null)
            Debug.Log("Cow could not find player");

        timeBeforeLosePlayer = 5f;
        killDist = 3.5f;
        stoppingDist = 3f;
        nmAgent.stoppingDistance = stoppingDist;
        nmAgent.speed = 10f;
        eyesightDist = 10f;

        soundInvestigateTime = 3f;

        neighborTooCloseDist = 0.5f;

        turnTime = 0.5f;
        lowerTurnDeg = -turnDeg / 2f;
        upperTurnDeg = turnDeg / 2f;
        cowSounds = GetComponents<AudioSource>();
        cowAlertMoo = cowSounds[0];
    }

    void Update()
    {
        if (fov.GetComponent<FOVController>().CanSeePlayer() && !player.GetComponent<PlayerController3D>().Dashing() && state != CowState.PlayerSeen && state != CowState.AttackPlayer)
        {
            player = fov.GetComponent<FOVController>().player;
            state = CowState.PlayerSeen;
        }

        switch (state)
        {
            case CowState.ReturnToDefault:
                if (SetToDefaultPos())
                    state = CowState.Idle;
                break;
            case CowState.Idle:
                anim.SetBool("Walking", false);
                if (type == CowType.Stationary)
                {
                    //SetToDefaultPos();
                }
                else if (type == CowType.Turning)
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
                else if (type == CowType.Moving)
                {
                    anim.SetBool("Walking", true);
                    nmAgent.SetDestination(path[currPathNode]);
                    if (nmAgent.remainingDistance < nmAgent.stoppingDistance && !nmAgent.pathPending)
                    {
                        currPathNode++;
                        if (currPathNode >= path.Length)
                            currPathNode = 0;
                    }
                }
                break;
            case CowState.InvestigateSound:
                anim.SetBool("Walking", true);
                anim.SetBool("Alarm", true);
                if (sourceOfSound == null)
                {
                    Debug.Log("No sound source");
                    state = CowState.ReturnToDefault;
                    anim.SetBool("Alarm", false);
                }
                else
                {
                    if (soundInvestigateTimeLeft > 0)
                    {
                        soundInvestigateTimeLeft -= Time.deltaTime;
                        if (soundInvestigateTimeLeft <= 0)
                        {
                            anim.SetBool("Alarm", false);
                            state = CowState.ReturnToDefault;
                        }
                    }
                    else
                    {
                        nmAgent.SetDestination(sourceOfSound.transform.position);
                        if (nmAgent.remainingDistance < nmAgent.stoppingDistance && !nmAgent.pathPending)
                        {
                            soundInvestigateTimeLeft = soundInvestigateTime;
                        }
                    }

                }
                break;
            case CowState.PlayerSeen:
                anim.SetBool("Alarm", true);
                cowAlertMoo.volume = 0.9f;
                cowAlertMoo.Play();
                if (!alerted)
                {
                    alerted = true;
                    alertTimeLeft = alertTime;
                }
                if (alertTimeLeft > 0)
                    alertTimeLeft -= Time.deltaTime;
                else
                {
                    anim.SetBool("Alarm", false);
                    state = CowState.AttackPlayer;
                }
                break;
            case CowState.AttackPlayer:
                if (alerted)
                {
                    alerted = false;
                    foreach (GameObject cow in allCows)
                    {
                        if (cow != gameObject && cow.GetComponent<CowController3D>().state != CowState.AttackPlayer && cow.GetComponent<CowController3D>().state != CowState.PlayerSeen)
                            if (cow.GetComponent<CowController3D>().CanSeePlayer())
                                cow.GetComponent<CowController3D>().state = CowState.PlayerSeen;
                    }
                }
                anim.SetBool("Walking", true);
                Vector3 playerLoc = player.transform.position + player.GetComponent<PlayerController3D>().GetVelocity();
                Vector3 avoidVec = Vector3.zero;

                foreach (GameObject cow in allCows)
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
                {
                    state = CowState.ReturnToDefault;
                }

                Vector3 dest = playerLoc + avoidVec;
                nmAgent.SetDestination(dest);
                break;
        }
    }

    bool SetToDefaultPos()
    {
        if (Vector3.Distance(transform.position, defaultPos) > 1f)
        {
            anim.SetBool("Walking", true);
            nmAgent.SetDestination(defaultPos);
        }
        if (transform.rotation != defaultRot)
            if (nmAgent.remainingDistance < nmAgent.stoppingDistance && !nmAgent.pathPending)
            {
                anim.SetBool("Walking", false);
                transform.rotation = Quaternion.Lerp(transform.rotation, defaultRot, Time.deltaTime * rotateSpeed);
            }
        return Vector3.Distance(transform.position, defaultPos) < 1f && transform.rotation == defaultRot;
    }

    public void TeleportCow()
    {
        transform.position = defaultPos;
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
        return canSeePlayer && Vector3.Distance(transform.position, player.transform.position) <= eyesightDist;
    }

    void CheckPlayerCaught()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < killDist)
        {
            GameObject.Find("GlobalControl").GetComponent<GlobalController>().KillPlayer();
            foreach (GameObject cow in allCows)
            {
                cow.GetComponent<CowController3D>().state = CowState.ReturnToDefault;
            }
        }
    }

    public void HearSound(GameObject source)
    {
        sourceOfSound = source;
        state = CowState.InvestigateSound;
    }

    public bool CanHearSound(Vector3 soundLoc)
    {
        return Vector3.Distance(transform.position, soundLoc) < alertDist;
    }
}
