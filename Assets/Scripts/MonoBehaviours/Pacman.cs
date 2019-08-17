using UnityEngine;

[RequireComponent(typeof(PacmanAnimatorController))]
[RequireComponent(typeof(DirectionMover))]
public class Pacman : MonoBehaviour, IObserverProperty<DirectionMover.Direction> {
    private PacmanAnimatorController _pacmanAnimator;
    private DirectionMover _directionMover;

    private void Start() {
        _pacmanAnimator = this.GetComponent<PacmanAnimatorController>();
        _directionMover = this.GetComponent<DirectionMover>();

        ((IReactiveProperty<DirectionMover.Direction>) _directionMover).Subscribe(this);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            _directionMover.ChangeDirection(DirectionMover.Direction.RIGHT);

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            _directionMover.ChangeDirection(DirectionMover.Direction.LEFT);

        else if (Input.GetKeyDown(KeyCode.UpArrow))
            _directionMover.ChangeDirection(DirectionMover.Direction.UP);

        else if (Input.GetKeyDown(KeyCode.DownArrow))
            _directionMover.ChangeDirection(DirectionMover.Direction.DOWN);
    }

    // pacman will react to direction changes updating the current animation
    public void OnUpdateProperty(DirectionMover.Direction currentDirection) {
        switch(currentDirection) {
            case DirectionMover.Direction.RIGHT:
                _pacmanAnimator.SetAnimation(PacmanAnimatorController.PacmanAnimation.MOVE_RIGHT);
                break;

            case DirectionMover.Direction.LEFT:
                _pacmanAnimator.SetAnimation(PacmanAnimatorController.PacmanAnimation.MOVE_LEFT);
                break;

            case DirectionMover.Direction.UP:
                _pacmanAnimator.SetAnimation(PacmanAnimatorController.PacmanAnimation.MOVE_UP);
                break;

            case DirectionMover.Direction.DOWN:
                _pacmanAnimator.SetAnimation(PacmanAnimatorController.PacmanAnimation.MOVE_DOWN);
                break;
        }
    }

}
