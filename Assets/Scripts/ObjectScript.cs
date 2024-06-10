using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateAnimation()
    {
        animator.SetTrigger("interact");
    }
}