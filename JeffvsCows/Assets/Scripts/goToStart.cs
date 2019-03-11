using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goToStart : MonoBehaviour
{
    // Start is called before the first frame update
    public void goStart()
    {
        SceneManager.LoadScene("Start_Menu");
    }
}
