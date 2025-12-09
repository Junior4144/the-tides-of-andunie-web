using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostInvasionCutsceneUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _uIObject;

    private void Awake()
    {
        _uIObject.SetActive(false);
    }

    private void OnEnable()
    {
        LSCameraManager.InvasionCutsceneEnded += HandleInvasionUI;
    }

    private void OnDisable()
    {
        LSCameraManager.InvasionCutsceneEnded -= HandleInvasionUI;
    }

    private void HandleInvasionUI()
    {
        _uIObject.SetActive(true);
        UIEvents.DefaultPopUPActive.Invoke();
    }

    public void OnButtonClicked()
    {
        if (_uIObject != null)
        {
            UIEvents.DefaultPopUpDisabled.Invoke();
            _uIObject.SetActive(false);
        }
    }
}

