using UnityEngine;
using System.Collections;


//reuse remove trigger
// except at trigger enter, delete cube
public class PutOnTrigger : RemoveTrgger {
    void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Cube"))
        {
            var animator = other.gameObject.GetComponent<Animator>();
            animator.SetBool("isVanish", true);
        }
    }
}
