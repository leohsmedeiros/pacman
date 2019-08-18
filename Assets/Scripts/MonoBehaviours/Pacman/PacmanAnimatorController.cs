using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PacmanAnimatorController : MonoBehaviour {
    public enum PacmanAnimation { MOVE_RIGHT, MOVE_LEFT, MOVE_UP, MOVE_DOWN, DIE }

    private Animator _animator;
    private PacmanAnimation? _currentPacmanAnimation = null;

    void Start() {
        _animator = this.GetComponent<Animator>();
    }

    public void SetAnimation (PacmanAnimation pacmanAnimation) {
        if (pacmanAnimation.Equals(_currentPacmanAnimation))
            return;

        switch (pacmanAnimation) {
            case PacmanAnimation.MOVE_RIGHT:
                _animator.SetTrigger("right");
                break;
            case PacmanAnimation.MOVE_LEFT:
                _animator.SetTrigger("left");
                break;
            case PacmanAnimation.MOVE_UP:
                _animator.SetTrigger("up");
                break;
            case PacmanAnimation.MOVE_DOWN:
                _animator.SetTrigger("down");
                break;
            case PacmanAnimation.DIE:
                _animator.SetTrigger("die");
                break;
        }

        _currentPacmanAnimation = pacmanAnimation;
    }

}
