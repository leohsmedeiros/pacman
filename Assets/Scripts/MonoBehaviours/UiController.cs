using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour /*, IObserverProperty<int>*/ {

    public GameObject Ready;
    public Text TextScore;
    public Text TextHighScore;


    IEnumerator HideReadyWithSeconds(int seconds) {
        yield return new WaitForSeconds(seconds);
        Ready.SetActive(false);
    }

    void Start() {
        TextHighScore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
        TextScore.text = "0";

        StartCoroutine(HideReadyWithSeconds(5));

        GameController controller = GameController.Instance;

        //((IReactiveProperty<int>)controller).Subscribe(this);
    }

    public void OnUpdateProperty(int value) {
        TextScore.text = value.ToString();    
    }

}
