using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerInputs : MonoBehaviour {
    private Animator animator;

    private void Start() {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float HorizontalAxis = Input.GetAxisRaw("Horizontal");
        float VerticalAxis = Input.GetAxisRaw("Vertical");

        if (System.Math.Abs(HorizontalAxis) > 0) {
            animator.SetTrigger((HorizontalAxis > 0 ? "right" : "left"));
        }

        if (System.Math.Abs(VerticalAxis) > 0) {
            animator.SetTrigger((VerticalAxis > 0 ? "up" : "down"));
        }

        if (Input.GetAxisRaw("Fire1") > 0) {
            animator.SetTrigger("die");
        }


        this.transform.position += Vector3.right;
    }
}
