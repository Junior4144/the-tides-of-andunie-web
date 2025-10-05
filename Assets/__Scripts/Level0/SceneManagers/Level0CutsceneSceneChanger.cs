using UnityEngine;
using UnityEngine.SceneManagement;

public class Level0CutsceneSceneChanger : MonoBehaviour
{

    public float changeTime;
    public string sceneName;

    void Update()
    {
        changeTime -= Time.deltaTime;

        if (changeTime <= 0)
        {
            SceneManager.LoadScene(sceneName);
        } 
    }
}
