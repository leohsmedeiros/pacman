using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PacmanAnimator : MonoBehaviour {
    public enum PacmanAnimation { MOVE_RIGHT, MOVE_LEFT, MOVE_UP, MOVE_DOWN, DIE }

    private Animator _animator;
    private PacmanAnimation? _currentPacmanAnimation = null;

    void Start() {
        _animator = this.GetComponent<Animator>();
    }

    public void SetAnimation (PacmanAnimation pacmanAnimation) {
        if (pacmanAnimation.Equals(_currentPacmanAnimation))
            return;

        _animator.SetTrigger(pacmanAnimation.ToString());
        _currentPacmanAnimation = pacmanAnimation;
    }

}
