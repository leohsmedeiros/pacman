using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Portal : MonoBehaviour {

    public Vector3 futurePoint;
    public Pacman.PacmanDirection direction;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals("Player")) {
            collision.transform.position = futurePoint;
            collision.gameObject.GetComponent<Pacman>().ChangeDirection(direction);
        }
    }
}
