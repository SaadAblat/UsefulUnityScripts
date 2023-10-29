using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScrolling : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public RectTransform contentPanelTransform;
    public HorizontalLayoutGroup HLG;

    public RectTransform[] ItemList;

    private void Start()
    {
        int ItemsToAdd = Mathf.CeilToInt(viewPortTransform.rect.width
            / (ItemList[0].rect.width+HLG.spacing));
        for (int i = 0; i < ItemsToAdd; i++) 
        {
            RectTransform RT = Instantiate(ItemList[i%ItemList.Length],
                contentPanelTransform);
            RT.SetAsLastSibling();
        }
        for (int i = 0; i < ItemsToAdd; i++)
        {
            int num = ItemList.Length - i - 1;
            while (num < 0)
            {
                num += ItemList.Length;
            }
            RectTransform RT = Instantiate(ItemList[num],
                contentPanelTransform);
            RT.SetAsFirstSibling();
        }
        contentPanelTransform.localPosition = new Vector3((0 - (ItemList[0].rect.width + HLG.spacing) *
            ItemsToAdd), contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z); 

    }
    private void Update()
    {
        if(contentPanelTransform.localPosition.x > 0)
        {
            Canvas.ForceUpdateCanvases();
            contentPanelTransform.localPosition -= new Vector3(ItemList.Length * (ItemList[0].rect.width
               + HLG.spacing), 0, 0);
        }
        if (contentPanelTransform.localPosition.x < 0-(ItemList.Length*(ItemList[0].rect.width+HLG.spacing)))
        {
            Canvas.ForceUpdateCanvases();
            contentPanelTransform.localPosition += new Vector3(ItemList.Length * (ItemList[0].rect.width
                + HLG.spacing), 0, 0);
        }
    }
}
