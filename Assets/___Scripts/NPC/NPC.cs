using UnityEngine;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer _interactSprite;

    private Transform _playerTransform;
    private const float INTERACT_DISTANCE = 5f;

    void Start()
    {
        //yield return null;

        _playerTransform = FindPlayerGameObject().transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsWithinInteractDistance())
        {
            Interact();
        }

        if (_interactSprite.gameObject.activeSelf && !IsWithinInteractDistance())
        {
            _interactSprite.gameObject.SetActive(false);
        }
        else if (!_interactSprite.gameObject.activeSelf && IsWithinInteractDistance())
        {
            _interactSprite.gameObject.SetActive(true);
        }

    }

    private GameObject FindPlayerGameObject()
    {
        GameObject[] allTaggedPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in allTaggedPlayers)
        {
            if (obj.transform.parent == null || !obj.transform.parent.CompareTag("Player"))
            {
                return obj;
            }
        }

        Debug.LogError("Player not found");
        return null;
    }

    public abstract void Interact();

    private bool IsWithinInteractDistance()
    {
        return Vector2.Distance(_playerTransform.position, transform.position) < INTERACT_DISTANCE;
    }
}
