using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum ANIMALS
{
    CAT = 0, FROG, RABBIT, SQUID, NOTHIING
};

public class FirstGame : MonoBehaviour
{
    private static FirstGame instance = null;

    public static FirstGame Instance { get => instance; set => instance = value; }

    [SerializeField] private GameObject initializePanel = null;
    [SerializeField] private GameObject animalZone = null;
    [SerializeField] private Image resultTextImage = null;
    [SerializeField] private Text leftTimeText = null;

    private List<GameObject> animalList = new List<GameObject>();
    private List<int> randomAnimalCnt = new List<int>();
    private int totalAnimalCnt = 0;
    private int maxAnimalCnt = 0;
    private int[] animalCntArr;
    private ANIMALS maxAnimal;

    [HideInInspector]
    public float m_ScreenLeft = -1.75f;
    [HideInInspector]
    public float m_ScreenTop = 1.3f;
    [HideInInspector]
    public float m_ScreenRight = 1.75f;
    [HideInInspector]
    public float m_ScreenBottom = -1.9f;

    private WaitForSeconds waitOneSeconds = new WaitForSeconds(1f);

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;

        animalList.Add(Resources.Load<GameObject>("Prefabs/FirstGame/Cat"));
        animalList.Add(Resources.Load<GameObject>("Prefabs/FirstGame/Frog"));
        animalList.Add(Resources.Load<GameObject>("Prefabs/FirstGame/Rabbit"));
        animalList.Add(Resources.Load<GameObject>("Prefabs/FirstGame/Squid"));

        int animalCnt = animalList.Count;
        totalAnimalCnt = animalCnt * (int)Singleton.Game.NowDifficult * 2;
        randomAnimalCnt = GetRandomAnimalCnt(Singleton.Game.NowDifficult);
        maxAnimalCnt = randomAnimalCnt[0];
        animalCntArr = RandomSortAnimalCnt(randomAnimalCnt);

        printArr(animalCntArr);
        print(maxAnimal);
        print(animalCntArr[(int)maxAnimal]);
    }

    private IEnumerator Start()
    {
        initializePanel.SetActive(true);

        Text countDownText = initializePanel.transform.GetChild(0).GetComponent<Text>();
        resultTextImage.gameObject.SetActive(false);
        countDownText.gameObject.SetActive(true);


        
        StartCoroutine(ILeftTimeUpdate((int)Singleton.Game.NowDifficult * 10));

        for (int i = 0; i < animalList.Count; i++)
            for (int j = 0; j < animalCntArr[i]; j++)
                StartCoroutine(IInstantiateAnimal(i));

        for (int i = 3; i > 0; i--)
        {
            countDownText.text = i.ToString();
            yield return waitOneSeconds;
        }

        resultTextImage.gameObject.SetActive(true);
        countDownText.gameObject.SetActive(false);

        initializePanel.SetActive(false);

        yield break;
    }

    private IEnumerator ILeftTimeUpdate(int leftTime)
    {
        leftTimeText.text = leftTime.ToString();
        
        yield return new WaitWhile(() => { return initializePanel.activeSelf; });
        yield return waitOneSeconds;

        leftTimeText.text = (--leftTime).ToString();
        if (leftTime.Equals(0)) { StartCoroutine(ITimeOver()); yield break; }
        else yield return StartCoroutine(ILeftTimeUpdate(leftTime));
    }

    public void CompareMaxAnimal(ANIMALS a)
    {
        StartCoroutine(maxAnimal.Equals(a) ? IRightAnswer() : IWrongAnswer());
    }

    private IEnumerator ITextEffect(Sprite sprite)
    {
        initializePanel.SetActive(true);
        resultTextImage.sprite = sprite;
        
        RectTransform rt = resultTextImage.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(resultTextImage.sprite.texture.width, resultTextImage.sprite.texture.height);

        float minSize = 0.2f;
        float minPos = -1000f;

        float maxSize = 0.6f;
        float maxPos = 0f;

        float targetTime = 0.6f;
        float timer = 0f;
        while (timer <= targetTime)
        {
            rt.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, timer / targetTime);
            rt.anchoredPosition = Vector3.up * Mathf.Lerp(minPos, maxPos, timer / targetTime);

            timer += Time.smoothDeltaTime;
            yield return null;
        }

        rt.localScale = Vector3.one * maxSize;
        rt.anchoredPosition = Vector3.up * maxPos;
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator IRightAnswer()
    {
        yield return StartCoroutine(ITextEffect(Resources.Load<Sprite>("Sprites/FirstGame/Answer")));

        Singleton.Game.Age -= (int)Singleton.Game.NowDifficult * 20;
        Singleton.Scene.LoadScene("SecondGameScene");

        yield break;
    }

    private IEnumerator IWrongAnswer()
    {
        yield return StartCoroutine(ITextEffect(Resources.Load<Sprite>("Sprites/FirstGame/Wrong")));

        Singleton.Scene.LoadScene("MainMenu");
        yield break;
    }

    private IEnumerator ITimeOver()
    {
        yield return StartCoroutine(ITextEffect(Resources.Load<Sprite>("Sprites/FirstGame/TimeOver")));

        Singleton.Scene.LoadScene("MainMenu");
        yield break;
    }

    private IEnumerator IInstantiateAnimal(int animalIndex)
    {
        SpriteRenderer sr;

        sr = Instantiate(animalList[animalIndex], animalZone.transform).GetComponent<SpriteRenderer>();

        sr.enabled = false;
        yield return new WaitForSeconds(3f);
        sr.enabled = true;

        yield break;
    }

    private int[] RandomSortAnimalCnt(List<int> list)
    {
        int[] tmp = new int[list.Count];
        for (int i = 0; i < tmp.Length; i++)
        {
            int index = Random.Range(0, list.Count);
            tmp[i] = list[index];
            list.RemoveAt(index);
        }

        int tmpMax = 0;
        for (int i = 0; i < tmp.Length; i++)
        {
            if (tmpMax <= tmp[i])
            {
                maxAnimal = (ANIMALS)i;
                tmpMax = tmp[i];
            }
        }
        return tmp;
    }

    private List<int> GetRandomAnimalCnt(DIFFICULT difficult)
    {
        int[][] animalPattern = new int[][] { new int[] { 3, 2, 2, 1 }, new int[] { 4, 2, 1, 1 } };
        List<int> tmpList = new List<int>();

        int random = Random.Range(0, 2); //0 ~ 1
        for (int i = 0; i < animalPattern[random].Length; i++)
            tmpList.Add(animalPattern[random][i]);

        for (int i = 1; i < (int)difficult; i++)
            for (int j = 0; j < tmpList.Count; j++)
                tmpList[j] += 2;

        return tmpList;
    }

    private void printArr(int[] arr)
    {
        string tmp = "Arr Contect: ";
        for (int i = 0; i < arr.Length; i++)
        {
            tmp += arr[i].ToString();
            tmp += i != arr.Length - 1 ? ", " : "";
        }
        print(tmp);
    }

}
