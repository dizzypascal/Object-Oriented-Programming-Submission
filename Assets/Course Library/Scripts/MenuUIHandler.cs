using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    private PlayerController playerControllerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartButton()
    {
        if (GameObject.Find("Game Over Screen") != null)
        {
            GameObject.Find("Game Over Screen").SetActive(false);
            playerControllerScript.gameOver = false;
        }
        
        //pressing start button will load main scene
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        //quit button will quit application or (in editor) playmode
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif

    }

}
