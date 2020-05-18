using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public void GameStart(DIFFICULT d)
    {
        Singleton.Game.NowDifficult = d;
        StartCoroutine(ILoadScene("FirstGameScene"));
    }

    public void LoadScene(string sceneName) => StartCoroutine(ILoadScene(sceneName));

    private IEnumerator ILoadScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        yield return new WaitUntil(() => { return asyncOperation.progress >= 0.9f; });
        print(sceneName + " Is Ready");

        asyncOperation.allowSceneActivation = true;

        yield break;
    }

}
