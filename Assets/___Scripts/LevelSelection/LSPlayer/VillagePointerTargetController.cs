using UnityEngine;
using UnityEngine.AI;

public class VillagePointerTargetController : MonoBehaviour
{
    private GameObject navigationTarget;

    private NavMeshAgent playerAgent;
    private Collider2D col;

    private bool wasHovering = false;

    private void Awake()
    {
        col = GetComponent<Collider2D>();

        if (transform.childCount > 0)
            navigationTarget = transform.GetChild(0).gameObject;
        else
            Debug.LogWarning($"{name} has no child object — navigationTarget was not set.");
    }

    private void Start()
    {
        playerAgent = PlayerManager.Instance.GetPlayerAgent();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && wasHovering)
        {
            playerAgent.SetDestination(navigationTarget.transform.position);
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool isHovering = col.OverlapPoint(mousePos);

        wasHovering = isHovering;
    }
}
