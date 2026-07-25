using UnityEngine;

// Put this on the SAME GameObject as the Animator (the child model).
// When "Apply Root Motion" is checked, Unity normally moves THIS object
// based on the animation. Since the actual game logic (NavMeshAgent, collider,
// enemy script) lives on the PARENT, that causes the visual mesh to drift away
// from the parent over time. This script intercepts that movement via
// OnAnimatorMove() and redirects it to the parent instead, keeping the child
// locked at local (0,0,0) so it never separates from the parent.
[RequireComponent(typeof(Animator))]
public class RootMotionRelay : MonoBehaviour
{
    private Animator anim;
    private Transform parentTransform;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        parentTransform = transform.parent;

        if (parentTransform == null)
            Debug.LogWarning("RootMotionRelay: this object has no parent — root motion has nowhere to redirect to.");
    }

    // Called by Unity automatically each frame root motion would normally be applied,
    // ONLY if Apply Root Motion is checked on the Animator.
    private void OnAnimatorMove()
    {
        if (parentTransform == null) return;

        // Apply the animation's movement/rotation delta to the PARENT instead of this object
        parentTransform.position += anim.deltaPosition;
        parentTransform.rotation *= anim.deltaRotation;

        // Keep this child locked at local zero so it never visually drifts from the parent
        transform.localPosition = Vector3.zero;
        // (local rotation is intentionally left alone in case the model needs a fixed local orientation offset)
    }
}