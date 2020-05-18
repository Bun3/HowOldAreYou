using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

[System.Serializable]
public class MapData
{
    public DIFFICULT difficult = DIFFICULT.DIFFICULT_NOTHING;
    [SerializeField] public List<int[,]> mapDatas = new List<int[,]>();
}

public class MapEditor : MonoBehaviour
{
    private static MapEditor instance = null;

    private Camera mainCam;

    [SerializeField] private DIFFICULT difficult = DIFFICULT.DIFFICULT_NOTHING;

    //[SerializeField] [Range(1, 10)] private int ScrollSpeed;
    //[SerializeField] [Range(1, 10)] private int MoveSpeed;

    [SerializeField] private GameObject uis = null;
    [SerializeField] private GameObject ui = null;
    [SerializeField] private Image mouseObj = null;

    [SerializeField] private Sprite deleteImage = null;

    [SerializeField] private MapData mapData;

    private bool isUIPop = false;
    private int currentIndex = 0;

    private const float distance = 2.6f;

    private Vector2 zero = new Vector2(-14.2f, 14.35f);

    private Vector2[,] positions = new Vector2[12, 12];
    private Rect[,] rects = new Rect[12, 12];
    private int[,] memorys = new int[12, 12];

    private GameObject tileObjectResource = null;

    private List<TileObject> tileObjects = new List<TileObject>();

    public static MapEditor Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;

        Screen.SetResolution(1100, 1200, false);

        mainCam = GetComponent<Camera>();
        uis.SetActive(false);
        mouseObj.sprite = null;
        for (int i = 0; i < positions.GetLength(0); i++)
        {
            for (int j = 0; j < positions.GetLength(1); j++)
            {
                positions[i, j] = new Vector2(zero.x + (j * distance), zero.y - (i * distance));

                Vector2 p = positions[i, j];
                rects[i, j] = new Rect(p.x - 1.2f, p.y + 1.2f, p.x + 1.2f, p.y - 1.2f);
                DrawRect(rects[i, j]);

                memorys[i, j] = -1;
            }
        }

        tileObjectResource = Resources.Load<GameObject>("Prefabs/MapEditor/[Object]");
    }

    public void ObjectReset()
    {
        for (int i = 0; i < tileObjects.Count; i++)
            Destroy(tileObjects[i].gameObject);
        tileObjects.Clear();
        for (int i = 0; i < memorys.GetLength(0); i++)
            for (int j = 0; j < memorys.GetLength(1); j++)
                memorys[i, j] = -1;
    }

    private void DrawRect(Rect r)
    {
        Debug.DrawLine(new Vector3(r.x, r.y), new Vector3(r.width, r.y), Color.red, 1000f);
        Debug.DrawLine(new Vector3(r.width, r.y), new Vector3(r.width, r.height), Color.red, 1000f);
        Debug.DrawLine(new Vector3(r.width, r.height), new Vector3(r.x, r.height), Color.red, 1000f);
        Debug.DrawLine(new Vector3(r.x, r.height), new Vector3(r.x, r.y), Color.red, 1000f);
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos = mainCam.ScreenToWorldPoint(mousePos);
        mouseObj.transform.position = mousePos;

        if (mouseObj.sprite != null)
        {
            for (int i = 0; i < positions.GetLength(0); i++)
            {
                for (int j = 0; j < positions.GetLength(1); j++)
                {
                    if (PtInRect(rects[i, j], mousePos))
                    {
                        MouseInBox(i, j);
                    }
                }
            }
        }

        mouseObj.transform.SetAsLastSibling();
    }

    private bool PtInRect(Rect r, Vector2 p)
    {
        return p.x <= r.width && p.x >= r.x && p.y <= r.y && p.y >= r.height;
    }

    private void MouseInBox(int i, int j)
    {
        mouseObj.transform.position = positions[i, j];

        if (Input.GetMouseButton(0))
        {
            for (int listIndex = 0; listIndex < tileObjects.Count; listIndex++)
            {
                if (mouseObj.transform.position.Equals(tileObjects[listIndex].transform.position))
                {
                    Destroy(tileObjects[listIndex].gameObject);
                    tileObjects.RemoveAt(listIndex);
                }
            }

            if (!mouseObj.sprite.Equals(deleteImage))
            {
                TileObject tileObj = Instantiate(tileObjectResource, positions[i, j], Quaternion.identity, ui.transform).GetComponent<TileObject>();
                tileObj.Image.sprite = mouseObj.sprite;
                tileObj.name = mouseObj.name;
                tileObjects.Add(tileObj);
            }
            memorys[i, j] = System.Convert.ToInt32(mouseObj.name);
        }
    }

    private string GetPath(int index = 0)
    {
        string path = Application.dataPath + "/Resources/MapData/" + difficult.ToString().Substring(10) + "/_" + index.ToString() + ".txt";
        print(path);
        return File.Exists(path) ? GetPath(++index) : path;
    }

    public void SaveMapData()
    {
        if (difficult.Equals(DIFFICULT.DIFFICULT_NOTHING))
        {
            print("Please Set Map's Difficult");
            return;
        }

        string data = JsonConvert.SerializeObject(memorys, Formatting.Indented);
        print(data);
        File.WriteAllText(GetPath(), data);
    }

    public static int[,] LoadMapData(DIFFICULT d)
    {
        string path = Application.dataPath + "/Resources/MapData/" + d.ToString().Substring(10);
        return JsonConvert.DeserializeObject<int[,]>(File.ReadAllText(path + "/_" + Random.Range(0, new DirectoryInfo(path).GetFiles("*.txt").Length).ToString() + ".txt"));
    }

    public void LoadMap(TextAsset mapData = null)
    {
        if (mapData == null) return;

        ObjectReset();
        memorys = JsonConvert.DeserializeObject<int[,]>(mapData.text);

        for (int row = 0; row < memorys.GetLength(0); row++)
        {
            for (int col = 0; col < memorys.GetLength(1); col++)
            {
                if (!memorys[row, col].Equals(-1))
                {
                    TileObject tileObj = Instantiate(tileObjectResource, positions[row, col], Quaternion.identity, ui.transform).GetComponent<TileObject>();
                    tileObj.Image.sprite = Resources.Load<Sprite>("Sprites/SecondGame/" + memorys[row, col]);
                    tileObj.name = memorys[row, col].ToString();
                    tileObjects.Add(tileObj);
                }
            }
        }
    }

    public void ChangeUICondition()
    {
        isUIPop = !isUIPop;
        uis.SetActive(isUIPop);
    }

    public void ClickDeleteButton()
    {
        mouseObj.sprite = deleteImage;
        mouseObj.color = Color.white;
        mouseObj.name = (-1).ToString();

        ButtonConditionInit();
    }

    private void ButtonConditionInit()
    {
        for (int i = 0; i < uis.transform.childCount; i++)
            uis.transform.GetChild(i).localScale = Vector3.one;
    }

    public void ButtonClick(int index)
    {
        currentIndex = index;
        for (int i = 0; i < uis.transform.childCount; i++)
        {
            if (i.Equals(index))
            {
                uis.transform.GetChild(index).localScale = Vector3.one * 0.8f;

                mouseObj.sprite = uis.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite;
                mouseObj.color = Color.white;
                mouseObj.name = i.ToString();

            }
            else uis.transform.GetChild(i).localScale = Vector3.one;
        }
    }

}
