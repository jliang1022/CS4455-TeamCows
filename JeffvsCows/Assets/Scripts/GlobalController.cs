using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    public string nextLevel;
    public Canvas deathUI;
    public float playerRespawnTime;
    public GameObject[] cows;
    float playerRespawnTimeLeft;
    WarningLightController wlc;

    private void Start()
    {
        RevivePlayer();
        wlc = GameObject.Find("Warning Light").GetComponent<WarningLightController>();
        if (wlc == null)
            Debug.Log("No warning light found");
    }

    private void Update()
    {
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
        GameObject.Find("Player").GetComponent<PlayerController3D>().Respawn();
    }
}
