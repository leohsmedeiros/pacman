using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  The responsibility of this script is to triggers an event
 *  when the dot get caught.
 */

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Dot : MonoBehaviour {
    private List<Action<Dot>> _actionsOnGetCaught;
    public bool IsEnergizer = false;


    IEnumerator DisableAfterPlaySound() {
        this.GetComponent<AudioSource>().Play();
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(this.GetComponent<AudioSource>().clip.length);

        this.gameObject.SetActive(false);
    }

    void Start() {
        _actionsOnGetCaught = new List<Action<Dot>>();

        GameController
            .Instance
            .GetComponent<GameController>()
            .RegisterDot(this);
    }

    public void SubscribeOnCaught (Action<Dot> action) => _actionsOnGetCaught.Add(action);

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.Instance.settings.PlayerTag)) {
            _actionsOnGetCaught.ForEach(action => action.Invoke(this));
            StartCoroutine(DisableAfterPlaySound());
        }
    }
}
