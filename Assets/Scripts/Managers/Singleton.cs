using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneManagers))]
[RequireComponent(typeof(GameManager))]
public class Singleton : MonoBehaviour
{
    private static Singleton instance = null;

    private static SceneManagers scene = null;
    private static GameManager game = null;

    public static Singleton Instance { get => instance; set => instance = value; }

    public static SceneManagers Scene { get => scene; set => scene = value; }
    public static GameManager Game { get => game; set => game = value; }

    private void Awake()
    {
        #region Make Singleton

        if (instance != null)   Destroy(instance);
        instance = this;

        #endregion

        #region Manager's Parsing

        scene = GetComponent<SceneManagers>();
        game = GetComponent<GameManager>();

        #endregion

        DontDestroyOnLoad(this.gameObject);
    }
}
