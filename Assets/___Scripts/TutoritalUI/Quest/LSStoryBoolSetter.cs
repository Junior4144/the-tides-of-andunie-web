using System.Collections;
using UnityEngine;

public class LSStoryBoolSetter : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SetBoolAfterDelay());
    }

    private IEnumerator SetBoolAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        GlobalStoryManager.Instance.enterLevelSelectorFirstTime = true;
    }
}