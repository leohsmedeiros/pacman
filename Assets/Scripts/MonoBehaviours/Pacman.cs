using UnityEngine;

[RequireComponent(typeof(PacmanAnimatorController))]
[RequireComponent(typeof(DirectionMover))]
public class Pacman : MonoBehaviour {
    private PacmanAnimatorController _pacmanAnimator;
    private DirectionMover _directionMover;

    private void Start() {
        _pacmanAnimator = this.GetComponent<PacmanAnimatorController>();
        _directionMover = this.GetComponent<DirectionMover>();
    }

    void Update() {
        //float HorizontalAxis = Input.GetAxisRaw("Horizontal");
        //float VerticalAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            _pacmanAnimator.SetAnimation(PacmanAnimatorController.PacmanAnimation.MOVE_RIGHT);
            _directionMover.ChangeDirection(DirectionMover.Direction.RIGHT);
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            _pacmanAnimator.SetAnimation(PacmanAnimatorController.PacmanAnimation.MOVE_LEFT);
            _directionMover.ChangeDirection(DirectionMover.Direction.LEFT);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            _pacmanAnimator.SetAnimation(PacmanAnimatorController.PacmanAnimation.MOVE_UP);
            _directionMover.ChangeDirection(DirectionMover.Direction.UP);
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            _pacmanAnimator.SetAnimation(PacmanAnimatorController.PacmanAnimation.MOVE_DOWN);
            _directionMover.ChangeDirection(DirectionMover.Direction.DOWN);
        }
    }
}
