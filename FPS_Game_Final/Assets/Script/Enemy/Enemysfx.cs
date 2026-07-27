using UnityEngine;

// Put this on the SAME GameObject as the Animator (the enemy's model).
// Unity's Animation Events can pass an AudioClip directly as a parameter —
// so ONE method here covers every sound. Add an Animation Event at whichever
// frame the sound should play (footstep contact, swing whoosh, roar, death
// thud, etc), set the Function to "PlaySound", and drag the specific clip
// into the event's own AudioClip parameter field. No need to predefine
// categories or arrays — pick any clip per-event, right there in the Animator.
[RequireComponent(typeof(AudioSource))]
public class EnemySFX : MonoBehaviour
{
    private AudioSource audioSource;

    [Range(0f, 0.5f)] public float pitchVariation = 0.1f; // slight random pitch so repeats don't feel robotic

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Call from an Animation Event — drag any AudioClip into the event's own parameter field
    public void PlaySound(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(clip);
    }

    // Not an Animation Event — call this directly from code (e.g. EnemyBase.TakeDamage())
    // since getting hit isn't tied to a specific animation frame.
    public void PlaySound(AudioClip[] variations)
    {
        if (variations == null || variations.Length == 0) return;
        PlaySound(variations[Random.Range(0, variations.Length)]);
    }
}