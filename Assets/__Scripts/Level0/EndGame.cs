using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    private float _timeToWaitBeforeExit;

    public void EndCurrentSession() =>
        Invoke(nameof(EndSession), _timeToWaitBeforeExit);

    private void EndSession() =>
        SceneManager.LoadScene("Main Menu");

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            EndCurrentSession();
    }
}
