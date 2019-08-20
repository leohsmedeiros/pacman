using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    public GameObject Ready;
    public Transform LifesParent;
    public Text TextScore;
    public Text TextHighScore;
    public float TimeToHideReadyObject;

    private float _timer = 0;

    void Start() {
        TextHighScore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
        TextScore.text = "0";
    }

    private void Update() {
        if (_timer >= TimeToHideReadyObject) {
            Ready.SetActive(false);
        } else {
            _timer += Time.deltaTime;
        }
    }

    public void UpdateScoreOnUi(int score) {
        TextScore.text = score.ToString();
    }

    public void UpdateLifesOnUi(int lifes) {
        for (int i = 0; i < LifesParent.childCount; i++) {
            LifesParent.GetChild(i).GetComponent<Image>().enabled = (i < lifes);
        }
    }

}
