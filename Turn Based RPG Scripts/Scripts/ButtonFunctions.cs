//Dennis Bruce
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets
{
    public class ButtonFunctions : MonoBehaviour
    {

        public void loadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        public void exitGame()
        {
            Application.Quit();
        }
        public void saveAndExit()
        {
            DataClass.saveData(DataClass.player1);
            DataClass.saveData(DataClass.player2);
            Application.Quit();
        }
    }    
}

