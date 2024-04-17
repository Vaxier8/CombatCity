using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Buttons : MonoBehaviour
{
    public Button Start_Button, Option_Button;

    void Start() {
        Start_Button.onClick.AddListener(NextScene);
    
    }
    public void NextScene()
    {
        SceneManager.LoadScene("TestScene");
    }
}