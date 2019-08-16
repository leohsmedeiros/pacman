using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public float speed;

    public Vector3 direction { get; set; } = Vector3.zero;

    // Update is called once per frame
    void Update() {
        this.transform.position += (direction * speed * Time.deltaTime);
    }
}
