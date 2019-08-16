using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Move))]
public class Pacman : MonoBehaviour {
    public enum PacmanDirection { RIGHT, LEFT, UP, DOWN }

    private Animator animator;
    private Move move;

    private void Start() {
        animator = this.GetComponent<Animator>();
        move = this.GetComponent<Move>();
        animator.SetTrigger("right");
    }

    void Update() {
        float HorizontalAxis = Input.GetAxisRaw("Horizontal");
        float VerticalAxis = Input.GetAxisRaw("Vertical");

        if (HorizontalAxis > 0) {
            ChangeDirection(PacmanDirection.RIGHT);
        } else if (HorizontalAxis < 0) {
            ChangeDirection(PacmanDirection.LEFT);
        } else if (VerticalAxis > 0) {
            ChangeDirection(PacmanDirection.UP);
        } else if (VerticalAxis < 0) {
            ChangeDirection(PacmanDirection.DOWN);
        }
    }

    public void ChangeDirection (PacmanDirection direction) {
        switch(direction) {
            case PacmanDirection.RIGHT:
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                move.direction = Vector3.right;
                break;

            case PacmanDirection.LEFT:
                this.transform.eulerAngles = new Vector3(0, 0, 180);
                move.direction = Vector3.left;
                break;

            case PacmanDirection.UP:
                this.transform.eulerAngles = new Vector3(0, 0, 90);
                move.direction = Vector3.up;
                break;

            case PacmanDirection.DOWN:
                this.transform.eulerAngles = new Vector3(0, 0, 270);
                move.direction = Vector3.down;
                break;
        }
    }
}
