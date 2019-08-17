using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour /*, IObserverProperty<int>*/ {

    public Text TextScore;
    public Text TextHighScore;

    // Start is called before the first frame update
    void Start() {
        TextHighScore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
        TextScore.text = "0";

        GameController controller = GameController.GetInstance();

        //((IReactiveProperty<int>)controller).Subscribe(this);
    }


    public void OnUpdateProperty(int value) {
        TextScore.text = value.ToString();    
    }


}
