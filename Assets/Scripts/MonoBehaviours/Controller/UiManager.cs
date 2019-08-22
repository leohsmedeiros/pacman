using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    public Button RetryButton;
    public Transform LifesParent;
    public Text TextScore;
    public Text TextHighScore;
    public Image fruitImage;


    void Start() {
        TextScore.text = "0";
    }

    public void UpdateFruitOnGUI(Sprite fruitSprite) => fruitImage.sprite = fruitSprite;

    public void UpdateHighScoreOnGUI(int score) => TextHighScore.text = score.ToString();

    public void UpdateScoreOnGUI(int score) => TextScore.text = score.ToString();

    public void UpdateLifesOnGUI(int lifes) {
        for (int i = 0; i < LifesParent.childCount; i++) {
            LifesParent.GetChild(i).GetComponent<Image>().enabled = (i < lifes);
        }

        if(lifes == 0) {
            RetryButton.gameObject.SetActive(true);
        }
    }



}
