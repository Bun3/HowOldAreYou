using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    private static Title instance = null;

    private GameObject m_ForcusScene = null;

    public static Title Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;

        print("Title");
    }

    private void Start()
    {
        m_ForcusScene = Instantiate(Resources.Load<GameObject>("Prefabs/TitleScene/[Title]"), GameObject.Find("[UI]").transform);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

}
