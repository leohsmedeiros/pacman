using UnityEngine;

[RequireComponent(typeof(PacmanAnimatorController))]
[RequireComponent(typeof(PacmanMover))]
public class Pacman : MonoBehaviour, IObserverProperty<Direction> {
    private PacmanAnimatorController _pacmanAnimator;
    private PacmanMover _directionMover;

    private void Start() {
        _pacmanAnimator = this.GetComponent<PacmanAnimatorController>();
        _directionMover = this.GetComponent<PacmanMover>();

        ((IReactiveProperty<Direction>) _directionMover).Subscribe(this);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            _directionMover.ChangeDirection(Direction.RIGHT);

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            _directionMover.ChangeDirection(Direction.LEFT);

        else if (Input.GetKeyDown(KeyCode.UpArrow))
            _directionMover.ChangeDirection(Direction.UP);

        else if (Input.GetKeyDown(KeyCode.DownArrow))
            _directionMover.ChangeDirection(Direction.DOWN);
    }

    // pacman will react to direction changes updating the current animation
    public void OnUpdateProperty(Direction currentDirection) {
        switch(currentDirection) {
            case Direction.RIGHT:
                _pacmanAnimator.SetAnimation(PacmanAnimatorController.PacmanAnimation.MOVE_RIGHT);
                break;

            case Direction.LEFT:
                _pacmanAnimator.SetAnimation(PacmanAnimatorController.PacmanAnimation.MOVE_LEFT);
                break;

            case Direction.UP:
                _pacmanAnimator.SetAnimation(PacmanAnimatorController.PacmanAnimation.MOVE_UP);
                break;

            case Direction.DOWN:
                _pacmanAnimator.SetAnimation(PacmanAnimatorController.PacmanAnimation.MOVE_DOWN);
                break;
        }
    }

    public Direction GetDirection() {
        return _directionMover.GetCurrentDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.NodeTag)) {
            GameController.Instance.UpdateCurrentPlayerNode(collision.GetComponent<Node>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.NodeTag)) {
            GameController.Instance.UpdateCurrentPlayerNode(null);
        }
    }

}
