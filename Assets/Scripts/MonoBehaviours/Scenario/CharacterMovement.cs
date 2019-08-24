using UnityEngine;

/*
 *  The responsibility of this script is to interpolate the position
 *  of any character towards a target node
 */

public class CharacterMovement : MonoBehaviour {

    public float speed = 5f;
    private bool _pause = false;
    private Node _targetNode = null;


    void Update() {
        if (!_pause && _targetNode != null) {
            this.transform.position = InterpolateToTarget(_targetNode);
        }
    }

    private Vector2 InterpolateToTarget(Node node) =>
        Vector2.MoveTowards(transform.position, node.GetPosition2D(), speed * Time.deltaTime);

    public void SetTargetNode(Node node) => _targetNode = node;

    public Node GetTargetNode() => _targetNode;

    public void Pause() => _pause = true;

    public void Resume() => _pause = false;
}
