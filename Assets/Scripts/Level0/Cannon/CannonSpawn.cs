using UnityEngine;

public class CannonSpawn : MonoBehaviour
{
    public GameObject cannon;
    public AudioClip fireSound;
    private float timer;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1)
        {
            timer = 0;
            spawnCannonBall();
        }
    }

    void spawnCannonBall()
    {
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
        Instantiate(cannon, transform.position, Quaternion.identity);
    }
}
