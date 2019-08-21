using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    public GameObject Ready;
    public Button RetryButton;
    public Transform LifesParent;
    public Text TextScore;
    public Text TextHighScore;
    public float TimeToHideReadyObject;

    private float _timer = 0;

    void Start() {
        TextScore.text = "0";
    }

    private void Update() {
        if (_timer >= TimeToHideReadyObject) {
            Ready.SetActive(false);
        } else {
            _timer += Time.deltaTime;
        }
    }


    public void UpdateHighScoreOnUi(int score) {
        TextHighScore.text = score.ToString();
    }

    public void UpdateScoreOnUi(int score) {
        TextScore.text = score.ToString();
    }

    public void UpdateLifesOnUi(int lifes) {
        for (int i = 0; i < LifesParent.childCount; i++) {
            LifesParent.GetChild(i).GetComponent<Image>().enabled = (i < lifes);
        }

        if(lifes == 0) {
            RetryButton.gameObject.SetActive(true);
        }
    }



}
