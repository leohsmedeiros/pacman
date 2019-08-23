using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour {

    [SerializeField]
    public string sceneName;

    public void LoadLevel() => SceneManager.LoadScene(sceneName);

}
