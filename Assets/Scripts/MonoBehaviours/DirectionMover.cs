using UnityEngine;

public class DirectionMover : MonoBehaviour {
    public enum Direction { RIGHT, LEFT, UP, DOWN };

    public float speed;

    private Direction? currentDirection = null;

    public void ChangeDirection(Direction direction) {
        Node currentNode = GameController.GetInstance().currentPlayerNode;

        if (currentNode != null) {
            switch (direction) {
                case Direction.RIGHT:
                    if (currentNode.rightNode != null)
                        this.transform.position = currentNode.rightNode.transform.position;

                    break;

                case Direction.LEFT:
                    if (currentNode.leftNode != null)
                        this.transform.position = currentNode.leftNode.transform.position;

                    break;

                case Direction.UP:
                    if (currentNode.upNode != null)
                        this.transform.position = currentNode.upNode.transform.position;

                    break;

                case Direction.DOWN:
                    if (currentNode.downNode != null)
                        this.transform.position = currentNode.downNode.transform.position;

                    break;
            }
        }
    }

}
