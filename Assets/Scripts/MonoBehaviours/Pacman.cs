using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(DirectionMover))]
public class Pacman : MonoBehaviour {
    private Animator animator;
    private DirectionMover move;

    private void Start() {
        animator = this.GetComponent<Animator>();
        move = this.GetComponent<DirectionMover>();
        animator.SetTrigger("right");
    }

    //
    void Update() {
        float HorizontalAxis = Input.GetAxisRaw("Horizontal");
        float VerticalAxis = Input.GetAxisRaw("Vertical");

        if (HorizontalAxis > 0) {
            animator.SetTrigger("right");
            move.ChangeDirection(DirectionMover.Direction.RIGHT);
        } else if (HorizontalAxis < 0) {
            animator.SetTrigger("left");
            move.ChangeDirection(DirectionMover.Direction.LEFT);
        } else if (VerticalAxis > 0) {
            animator.SetTrigger("up");
            move.ChangeDirection(DirectionMover.Direction.UP);
        } else if (VerticalAxis < 0) {
            animator.SetTrigger("down");
            move.ChangeDirection(DirectionMover.Direction.DOWN);
        }
    }

    //public void ChangeDirection (PacmanDirection direction) {
    //    switch(direction) {
    //        case PacmanDirection.RIGHT:
    //            this.transform.eulerAngles = new Vector3(0, 0, 0);
    //            move.direction = Vector3.right;
    //            break;

    //        case PacmanDirection.LEFT:
    //            this.transform.eulerAngles = new Vector3(0, 0, 180);
    //            move.direction = Vector3.left;
    //            break;

    //        case PacmanDirection.UP:
    //            this.transform.eulerAngles = new Vector3(0, 0, 90);
    //            move.direction = Vector3.up;
    //            break;

    //        case PacmanDirection.DOWN:
    //            this.transform.eulerAngles = new Vector3(0, 0, 270);
    //            move.direction = Vector3.down;
    //            break;
    //    }
    //}
}
