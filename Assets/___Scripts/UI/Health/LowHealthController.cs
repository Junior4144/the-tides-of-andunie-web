using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class LowHealthController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TMP_Text pauseDialogue;
    [SerializeField] private float lowHealthThreshold = 0.4f;
    [SerializeField] private float healAmount = 25f;
    [SerializeField] private KeyCode healKey = KeyCode.E;
    [SerializeField] private UnityEvent OnLowHealthTriggered;

    private PlayerHealthController health;
    private bool uiActive;

    private IEnumerator Start()
    {
        if (speechBubble) speechBubble.SetActive(false);
        if (pauseDialogue) pauseDialogue.gameObject.SetActive(false);

        // Wait until player spawns
        yield return new WaitUntil(() => PlayerManager.Instance != null);
        yield return new WaitUntil(() => PlayerManager.Instance.GetComponentInChildren<PlayerHealthController>() != null);

        health = PlayerManager.Instance.GetComponentInChildren<PlayerHealthController>();
        Debug.Log($"LowHealthController: Player found, health = {health.GetCurrentHealth()}");
    }

    private void Update()
    {
        if (health == null) return;

        float hpPct = health.GetPercentHealth();
        if (hpPct <= lowHealthThreshold)
        {
            if (!uiActive) ShowUI();
            if (Input.GetKeyDown(healKey)) Heal();
        }
        else if (uiActive) HideUI();

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Force low health for test");
            health.SetCurrentHealth(health.GetMaxHealth() * 0.3f);
        }

    }

    private void ShowUI()
    {
        uiActive = true;
        speechBubble?.SetActive(true);
        pauseDialogue?.gameObject.SetActive(true);
        pauseDialogue.text = "You're badly hurt!\nPress <b>E</b> to heal.";
        OnLowHealthTriggered?.Invoke();
        Debug.Log("LowHealthController: Showing low health UI");
    }

    private void HideUI()
    {
        uiActive = false;
        speechBubble?.SetActive(false);
        if (pauseDialogue) pauseDialogue.text = "";
        pauseDialogue?.gameObject.SetActive(false);
    }

    private void Heal()
    {
        health.AddHealth(healAmount);
        Debug.Log($"LowHealthController: Healed {healAmount}");
    }

}
