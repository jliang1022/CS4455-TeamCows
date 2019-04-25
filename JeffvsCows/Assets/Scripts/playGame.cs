using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playGame : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1_2");
    }
}
