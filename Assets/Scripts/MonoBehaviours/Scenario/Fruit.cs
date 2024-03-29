﻿using System;
using System.Collections.Generic;
using UnityEngine;

/*
 *  The responsibility of this script is to triggers an event
 *  when the fruit get caught.
 */

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class Fruit : MonoBehaviour {
    public int points;
    public GameObject scoreOnBoardTextPrefab;

    private List<Action> _actionsForGetCaught;
    private AudioSource _audioSource;

    private void Start() {
        _actionsForGetCaught = new List<Action>();
        _audioSource = this.GetComponent<AudioSource>();
        GameController.Instance.RegisterFruit(this);
    }

    public void SubscribeOnGetCaught(Action action) => _actionsForGetCaught.Add(action);

    public Sprite GetSprite() => this.GetComponent<SpriteRenderer>().sprite;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag.Equals(GameController.Instance.settings.PlayerTag)) {
            _actionsForGetCaught.ForEach(action => action.Invoke());

            GameObject scoreObject = Instantiate(scoreOnBoardTextPrefab, this.transform.position, Quaternion.identity);
            scoreObject.GetComponent<TextMesh>().text = points.ToString();
            Destroy(scoreObject, 2);

            this.GetComponent<Collider2D>().enabled = false;
            this.GetComponent<SpriteRenderer>().enabled = false;

            _audioSource.Play();
            Destroy(this, _audioSource.clip.length);
        }
    }
}
