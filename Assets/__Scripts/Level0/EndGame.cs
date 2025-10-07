using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    private float _timeToWaitBeforeExit;

    [SerializeField]
    private SceneController _sceneController;

    [SerializeField]
    private GameObject _playerHero;

    PlayerHeroMovement _playerHeroMovement;

    private void Awake()
    {
        _playerHeroMovement = _playerHero.GetComponent<PlayerHeroMovement>();
    }
    public void EndCurrentSession() =>
        Invoke(nameof(EndSession), _timeToWaitBeforeExit);

    private void EndSession() =>
        _sceneController.LoadScene("Main Menu");

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //        EndCurrentSession();
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EndCurrentSession();
            EndGameChangeStats();
        }

    }
    void EndGameChangeStats()
    {
        _playerHeroMovement.enabled = false;
        _playerHero.gameObject.layer = LayerMask.NameToLayer("IgnoreCannon"); // new layer
    }
}
