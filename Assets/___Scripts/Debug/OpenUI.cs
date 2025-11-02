using UnityEngine;

public class OpenUI : MonoBehaviour
{

    [SerializeField] private GameObject _canvas;
    [SerializeField] private KeyCode _key;

    void Start()
    {
        _canvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(_key))
            _canvas.SetActive(!_canvas.activeSelf);
    }
}
