using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int StartSceneBuildIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBtnClicked()
    {
        SceneManager.LoadScene(StartSceneBuildIndex, LoadSceneMode.Single);
    }

    public void LoadGameBtnClicked()
    {
        Debug.Log("Load Game Button Clicked");
    }

    public void QuitGameButtonClicked()
    {
        Debug.Log("Quit Button Clicked");
        Application.Quit(0);
    }
}
