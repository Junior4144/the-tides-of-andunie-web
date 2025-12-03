using System.Collections;
using TMPro;
using UnityEngine;

public class EnterVillageTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;


    private void Start()
    {
        StartCoroutine(HandleSetup());
    }

    private IEnumerator HandleSetup()
    {
        yield return null;

        _titleText.text = LSManager.Instance.GetVillageName(VillageIDManager.Instance.villageId);


    }
}
