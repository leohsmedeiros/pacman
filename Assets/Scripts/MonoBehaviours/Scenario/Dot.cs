using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Dot : MonoBehaviour {
    private List<Action<Dot>> actionsOnGetCaught;
    public bool IsEnergizer = false;

    void Start() {
        actionsOnGetCaught = new List<Action<Dot>>();

        GameController
            .Instance
            .GetComponent<GameController>()
            .RegisterDot(this);
    }

    IEnumerator DisableAfterSound() {
        this.GetComponent<AudioSource>().Play();
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(this.GetComponent<AudioSource>().clip.length);

        this.gameObject.SetActive(false);
    }

    public void SubscribeOnCaught (Action<Dot> action) {
        actionsOnGetCaught.Add(action);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.Instance.settings.PlayerTag)) {
            actionsOnGetCaught.ForEach(action => action.Invoke(this));
            StartCoroutine(DisableAfterSound());
        }
    }
}
