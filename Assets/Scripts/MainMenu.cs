using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnClickGame()
    {
        SceneManager.LoadScene("Prologue");
    }

    public void OnClickNext()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnClickHowGame()
    {
        SceneManager.LoadScene("HowGame");
    }
}
