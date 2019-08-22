using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour {
    [SerializeField]
    public SceneAsset scene;

    public void LoadLevel() => SceneManager.LoadScene(scene.name);
}
