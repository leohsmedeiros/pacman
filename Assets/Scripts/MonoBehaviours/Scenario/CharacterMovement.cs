using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    private Node _targetNode;
    public float speed = 5f;
    [HideInInspector]
    public bool pause = true;


    private void Start() {
        _targetNode = null;
    }

    void Update() {
        if (!GameController.Instance.gameModeManager.currentGameMode.Equals(GameMode.INTRO) &&
            !GameController.Instance.gameModeManager.currentGameMode.Equals(GameMode.DEAD) &&
            !pause && _targetNode != null) {

            this.transform.position = Vector2.MoveTowards(transform.position,
                                                          _targetNode.GetPosition2D(),
                                                          speed * Time.deltaTime);
        }
    }

    public void SetTargetNode(Node node) => _targetNode = node;

}
