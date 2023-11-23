using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] Sprite _circleSprite;
    RectTransform _graphContainer;

    private void Start()
    {
        _graphContainer = transform.GetComponent<RectTransform>();
        GameObject.Find("EventSystem").GetComponent<GeneralSettings>().UpdateAll();
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, float xSize)
    {
        GameObject gameObject = new("circle", typeof(Image));
        gameObject.transform.SetParent(_graphContainer, false);
        gameObject.GetComponent<Image>().sprite = _circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y / 2 + _graphContainer.sizeDelta.y / 2);
        if (anchoredPosition.y > 0)
            rectTransform.sizeDelta = new Vector2(xSize / 2, anchoredPosition.y);
        else
        {
            rectTransform.sizeDelta = new Vector2(xSize / 2, -anchoredPosition.y);
            rectTransform.rotation = Quaternion.Euler(0, 0, 180);
        }
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }
    public void ShowGraph(List<int> valueList, int count)
    {
        float graphHeight = _graphContainer.sizeDelta.y;
        float yMaximum = 20f * count / 10;
        float xSize = _graphContainer.sizeDelta.x / count;

        for (int i = 0; i < valueList.Count; ++i)
        {
            float xPosition = xSize / 2 + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            CreateCircle(new Vector2(xPosition, yPosition), xSize);
        }
    }
    public void EditGraph(int lindex, int rindex)
    {
        var lchild = gameObject.transform.GetChild(lindex);
        var rchild = gameObject.transform.GetChild(rindex);
        RectTransform lrectTransform = lchild.gameObject.GetComponent<RectTransform>();
        RectTransform rrectTransform = rchild.gameObject.GetComponent<RectTransform>(); 
        (lrectTransform.anchoredPosition, rrectTransform.anchoredPosition) 
            = (new Vector2(rrectTransform.anchoredPosition.x, lrectTransform.anchoredPosition.y), 
            new Vector2(lrectTransform.anchoredPosition.x, rrectTransform.anchoredPosition.y));
        lchild.SetSiblingIndex(rindex);
        rchild.SetSiblingIndex(lindex);
    }
}
