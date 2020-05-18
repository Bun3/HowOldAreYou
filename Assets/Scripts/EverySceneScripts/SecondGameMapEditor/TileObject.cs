using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    private RectTransform rt = null;
    private Transform ts = null;
    private Image image = null;

    public RectTransform Rt { get => rt; set => rt = value; }
    public Image Image { get => image; set => image = value; }
    public Transform Ts { get => ts; set => ts = value; }

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        ts = transform;
    }
}
