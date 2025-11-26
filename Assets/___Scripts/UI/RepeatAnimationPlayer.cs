using UnityEngine;

public class RepeatAnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _timeBetweenAnimations = 30f;

    private float timer;

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= _timeBetweenAnimations)
        {
            _animator.Play(0);
            timer = 0f;
        }
    }
}
