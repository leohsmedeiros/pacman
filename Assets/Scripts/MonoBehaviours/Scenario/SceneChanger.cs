using UnityEngine;
using UnityEngine.SceneManagement;

/* The responsibility of this script is change the current scene */

public class SceneChanger : MonoBehaviour {

    [SerializeField]
    public string sceneName;

    public void LoadLevel() => SceneManager.LoadScene(sceneName);
    public void LoadLevel(string name) => SceneManager.LoadScene(name);

}
