using UnityEngine;

public class Level1QuestUIController : MonoBehaviour
{
    [SerializeField] private GameObject Panel1;
    [SerializeField] private GameObject Panel2;
    private void Awake()
    {
        Panel1.SetActive(true);
        Panel2.SetActive(true);
    }

    private void Start()
    {
        HandleQuestUI();
    }

    private void HandleQuestUI()
    {
        if(GlobalStoryManager.Instance.HasTalkedToChief == false)
        {
            Panel1.SetActive(true);
            Panel2.SetActive(false);
        }
        else if (GlobalStoryManager.Instance.HasTalkedToChief && GlobalStoryManager.Instance.playLSInvasionCutscene)
        {
            Panel1.SetActive(false);
            Panel2.SetActive(true);
        }
        else
        {
            Panel1.SetActive(false);
            Panel2.SetActive(false);
        }
    }
}
