using UnityEngine;
using UnityEngine.SceneManagement;

public class menuControl : MonoBehaviour
{
    public void StartGame(){
        SceneManager.LoadScene("Level1");
    }

    public void ExitGame(){
        Application.Quit();
    }
}
