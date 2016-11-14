//Johnathan Duke

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Assets._scripts{

    public class StartMenu  : MonoBehaviour {
        public Canvas menu;
        public Button startButton;
        public Button quitButton;
        public void Start() { }
        public void pressStart()
        {
            SceneManager.LoadScene("TeamSelectionScreen");
        }
        public void pressQuit()
        {
            Application.Quit();
        }
    }
}