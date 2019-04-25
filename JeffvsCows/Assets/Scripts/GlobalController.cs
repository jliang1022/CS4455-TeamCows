using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    public string nextLevel;
    public Canvas deathUI;
    public Canvas pauseUI;
    public float playerRespawnTime;
    public GameObject[] cows;
    public GameObject[] balls;
    float playerRespawnTimeLeft;
    public WarningLightController wlc;
    bool isPaused = false;

    private void Start()
    {
        SetPlayerStart();
        //wlc = GameObject.Find("Warning Light").GetComponent<WarningLightController>();
        if (wlc == null)
            Debug.Log("No warning light found");
        pauseUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == false)
            {
                Time.timeScale = 0f;
                isPaused = true;
                pauseUI.gameObject.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                isPaused = false;
                pauseUI.gameObject.SetActive(false);
            }
        }
        if (playerRespawnTimeLeft > 0)
        {
            playerRespawnTimeLeft -= Time.deltaTime;
            if (playerRespawnTimeLeft <= 0)
            {
                RevivePlayer();
            }
        }

        bool warning = false;
        foreach (GameObject cow in cows)
        {
            if (cow.GetComponent<CowController3D>().state == CowController3D.CowState.PlayerSeen || cow.GetComponent<CowController3D>().state == CowController3D.CowState.AttackPlayer)
                warning = true;
        }

        if (!warning)
            wlc.Deactivate();
        else
            wlc.Activate();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void AlertCows(GameObject rock)
    {
        foreach (GameObject cow in cows)
        {
            if (cow.GetComponent<CowController3D>().CanHearSound(rock.transform.position))
                cow.GetComponent<CowController3D>().HearSound(rock);
        }
    }

    public void KillPlayer()
    {
        wlc.Deactivate();
        playerRespawnTimeLeft = playerRespawnTime;
        deathUI.gameObject.SetActive(true);
        GameObject.Find("Player").GetComponent<PlayerController3D>().Die();
    }

    public void RevivePlayer()
    {
        deathUI.gameObject.SetActive(false);
        foreach (GameObject cow in cows)
        {
            cow.GetComponent<CowController3D>().TeleportCow();
        }
        foreach (GameObject ball in balls)
        {
            ball.GetComponent<BallScript>().ReturnBall();
        }
        GameObject.Find("Player").GetComponent<PlayerController3D>().Respawn();
    }

    void SetPlayerStart()
    {
        deathUI.gameObject.SetActive(false);
        GameObject.Find("Player").GetComponent<PlayerController3D>().Respawn();
    }
}
