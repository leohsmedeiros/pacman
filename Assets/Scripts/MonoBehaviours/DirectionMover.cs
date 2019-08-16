using UnityEngine;

public class DirectionMover : MonoBehaviour {
    public enum Direction { RIGHT, LEFT, UP, DOWN };

    public float speed;

    private Vector3 directionVector { get; set; } = Vector3.zero;

    public void ChangeDirection(Direction direction) {
        switch (direction) {
            case Direction.RIGHT:
                directionVector = Vector3.right;
                break;

            case Direction.LEFT:
                directionVector = Vector3.left;
                break;

            case Direction.UP:
                directionVector = Vector3.up;
                break;

            case Direction.DOWN:
                directionVector = Vector3.down;
                break;
        }
    }

    private void Update() {
        this.transform.position += (directionVector * speed * Time.deltaTime);
    }
}
