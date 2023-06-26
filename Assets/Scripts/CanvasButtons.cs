using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class CanvasButtons : MonoBehaviour
{
    public void NewGame()
    {
        string dataPath = Application.persistentDataPath;
        if (System.IO.File.Exists(dataPath + "/" + "pos" + ".save"))
        {
            File.Delete(dataPath + "/" + "pos" + ".save");
        }

        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
