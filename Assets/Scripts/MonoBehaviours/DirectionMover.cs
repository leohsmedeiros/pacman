using UnityEngine;

public class DirectionMover : MonoBehaviour {
    public enum Direction { RIGHT, LEFT, UP, DOWN };

    public float speed;

    private Vector3 _directionVector { get; set; } = Vector3.zero;

    public void ChangeDirection(Direction direction) {
        switch (direction) {
            case Direction.RIGHT:
                _directionVector = Vector3.right;
                break;

            case Direction.LEFT:
                _directionVector = Vector3.left;
                break;

            case Direction.UP:
                _directionVector = Vector3.up;
                break;

            case Direction.DOWN:
                _directionVector = Vector3.down;
                break;
        }
    }

    private void Update() {
        this.transform.position += (_directionVector * speed * Time.deltaTime);
    }
}
