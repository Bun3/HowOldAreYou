using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    public float x;
    public float y;

    public Point Zero { get => new Point(0f, 0f); }
    public Point One { get => new Point(1f, 1f); }

    public Point(float _x = 0f, float _y = 0f)
    {
        x = _x;
        y = _y;
    }

    public static Point operator +(Point lhs, Point rhs) => new Point(lhs.x + rhs.x, lhs.y + rhs.y);
    public static Point operator -(Point lhs, Point rhs) => new Point(lhs.x - rhs.x, lhs.y - rhs.y);
    public static Point operator *(Point lhs, Point rhs) => new Point(lhs.x * rhs.x, lhs.y * rhs.y);
    public static Point operator *(Point lhs, int rhs) => new Point(lhs.x * rhs, lhs.y * rhs);
    public static Point operator *(Point lhs, float rhs) => new Point(lhs.x * rhs, lhs.y * rhs);
    public static bool operator ==(Point lhs, Point rhs) => lhs.x.Equals(rhs.x) && lhs.y.Equals(rhs.y);
    public static bool operator !=(Point lhs, Point rhs) => !lhs.x.Equals(rhs.x) || !lhs.y.Equals(rhs.y);

    public static implicit operator Vector3(Point rhs) => new Vector3(rhs.x, rhs.y, 0);
    public static implicit operator Vector2(Point rhs) => new Vector2(rhs.x, rhs.y);
    public static implicit operator Point(Vector3 rhs) => new Point(rhs.x, rhs.y);

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    public override string ToString()
    {
        return "Point {X:" + x.ToString() + ", Y:" + y.ToString() + "}";
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class SecondGame : MonoBehaviour
{

    #region BoardVariavle

    private const float distance = 2.6f;

    private static Point zero = new Point(-14.2f, 14.35f);

    private Point[,] points = new Point[12, 12];
    private int[,] memorys = new int[12, 12];

    #endregion

    private Camera cam = null;
    private RabbitCtrl rabbitCtrl = null;
    private Vector2Int currentRabbitPos = Vector2Int.zero;
    private Dictionary<Vector2Int, GameObject> objs = new Dictionary<Vector2Int, GameObject>();

    public static SecondGame Instance = null;

    public Vector2Int CurrentRabbitPos { get => currentRabbitPos; set => currentRabbitPos = value; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;

        #region BoardResources
        GameObject objPrefab = Resources.Load<GameObject>("Prefabs/SecondGame/[Object]");

        Sprite[] sprites = new Sprite[7];
        for (int i = 0; i < sprites.Length; i++)
            sprites[i] = Resources.Load<Sprite>("Sprites/SecondGame/" + i);
        #endregion

        cam = Camera.main;

        memorys = MapEditor.LoadMapData(DIFFICULT.DIFFICULT_EASY);
        for (int row = 0; row < memorys.GetLength(0); row++)
        {
            for (int col = 0; col < memorys.GetLength(1); col++)
            {
                points[row, col] = new Point(zero.x + (col * distance), zero.y - (row * distance));
                if (!memorys[row, col].Equals(-1))
                {
                    if (memorys[row, col].Equals(0))
                    {
                        if (rabbitCtrl != null)
                        {
                            print("Already Rabbit");
                        }
                        else
                        {
                            rabbitCtrl = Instantiate(Resources.Load<GameObject>("Prefabs/SecondGame/[Rabbit]")).GetComponent<RabbitCtrl>();
                            rabbitCtrl.transform.position = points[row, col];
                            currentRabbitPos = new Vector2Int(col, row);
                            cam.transform.position = new Vector3(points[row, col].x, points[row, col].y, -10f);
                        }
                    }
                    else
                    {
                        SpriteRenderer obj = Instantiate(objPrefab, points[row, col], Quaternion.identity).GetComponent<SpriteRenderer>();
                        obj.sprite = sprites[memorys[row, col]];
                        obj.transform.localScale = GetScaleForInRect(obj.sprite, 2.4f);
                        obj.transform.localScale = GetTruncatedScale(obj.transform.localScale);
                        obj.name = memorys[row, col].ToString();
                        objs.Add(new Vector2Int(col, row), obj.gameObject);
                    }
                }
            }
        }

    }

    private void LateUpdate()
    {
        cam.transform.position = new Vector3(rabbitCtrl.transform.position.x, rabbitCtrl.transform.position.y, -10f);
        cam.transform.position = new Vector3(Mathf.Clamp(cam.transform.position.x, -8.8f, 8.8f), Mathf.Clamp(cam.transform.position.y, -9.1f, 9.1f), -10f);
    }

    private Vector3 GetScaleForInRect(Sprite sprite, float rectSize)
    {
        rectSize *= 100;
        int w = sprite.texture.width, h = sprite.texture.height;
        return Vector3.one + Vector3.one * ((rectSize - (w >= h ? w : h)) / 100);
    }
    private Vector3 GetTruncatedScale(Vector3 scale) => new Vector3(Mathf.Floor(scale.x * 10) * 0.1f, Mathf.Floor(scale.y * 10) * 0.1f, Mathf.Floor(scale.z * 10) * 0.1f);

    public void MoveRabbit(Vector2Int v)
    {
        if (!GetCanMoveThatBoard(v + currentRabbitPos)) return;
        memorys[currentRabbitPos.y, currentRabbitPos.x] = -1;
        currentRabbitPos = v += currentRabbitPos;
        if (!memorys[v.y, v.x].Equals(-1))
        {
            print(memorys[v.y, v.x]);
            Destroy(objs[v]);
        }
        memorys[currentRabbitPos.y, currentRabbitPos.x] = 0;
        rabbitCtrl.transform.position = points[v.y, v.x];
    }

    public bool GetCanMoveThatBoard(Vector2Int v)
    {
        if (v.x < 0 || v.x > 11 || v.y < 0 || v.y > 11) return false;
        return memorys[v.y, v.x].Equals(-1) || memorys[v.y, v.x].Equals(1) || memorys[v.y, v.x].Equals(2) || memorys[v.y, v.x].Equals(3);
    }
}
