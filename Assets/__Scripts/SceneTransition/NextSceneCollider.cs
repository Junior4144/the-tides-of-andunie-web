using UnityEngine;
using UnityEngine.SceneManagement;


public class NextSceneCollider : MonoBehaviour
{
    [SerializeField]
    private SceneController _sceneController;

    public string nextScene = "";


    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("PersistentGameplay");
        SceneManager.LoadScene(nextScene, LoadSceneMode.Additive);
    }
}
