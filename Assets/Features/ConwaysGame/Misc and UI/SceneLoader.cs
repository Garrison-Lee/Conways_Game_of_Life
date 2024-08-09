using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Super basic reload method that knows there's only one scene in this interview project
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
