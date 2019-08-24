using System.Collections;
using UnityEngine;
using UnityEngine.Video;

/*
 *  The responsibility of this script is to wait until video with
 *  intermission finish, and then return to game
 */

public class Intermission : MonoBehaviour {

    public SceneChanger sceneChanger;
    public VideoPlayer videoPlayer;

    IEnumerator ReturnToStageAfterVideo() {        
        yield return new WaitForSeconds((float) videoPlayer.clip.length);
        sceneChanger.LoadLevel();
    }

    void Start() => StartCoroutine(ReturnToStageAfterVideo());

}
