using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ButtonEffect : MonoBehaviour
{
    //«ö¶s
    public CanvasGroup startPanel;
    public void StartGame()
    {
        Destroy(startPanel.gameObject);
    }
    public void QuitGame()
    {
        Application.Quit();

    }

}
