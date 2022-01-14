using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{


    public static SceneHelper instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SceneHelper();
            }
            return instance;
        }
        private set { }
    }

    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }

}