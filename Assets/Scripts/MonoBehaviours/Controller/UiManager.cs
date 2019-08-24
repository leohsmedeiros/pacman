using UnityEngine;
using UnityEngine.UI;

/*
 *  The responsibility of this script is to adapt the GUI about
 *  changes on score, lifes and other game informations
 */

public class UiManager : MonoBehaviour {

    public Button RetryButton;
    public Transform LifesParent;
    public Text TextScore;
    public Text TextHighScore;
    public Image fruitImage;


    public void FruitOnGUI(Sprite fruitSprite) => fruitImage.sprite = fruitSprite;

    public void HighScoreOnGUI(int score) => TextHighScore.text = score.ToString();

    public void ScoreOnGUI(int score) => TextScore.text = score.ToString();

    public void LifesOnGUI(int lifes) {
        for (int i = 0; i < LifesParent.childCount; i++) {
            LifesParent.GetChild(i).GetComponent<Image>().enabled = (i < lifes);
        }

        if(lifes == 0) {
            RetryButton.gameObject.SetActive(true);
        }
    }
}
