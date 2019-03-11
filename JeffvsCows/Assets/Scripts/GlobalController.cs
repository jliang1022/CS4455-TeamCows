using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    public string nextLevel;
    public Canvas deathUI;
    public float playerRespawnTime;
    float playerRespawnTimeLeft;

    private void Start()
    {
        RevivePlayer();
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
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void KillPlayer()
    {
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
