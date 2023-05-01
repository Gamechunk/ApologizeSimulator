using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TabType
{
    Head,
    Top,
    Bot,
}

public class TabMenuController : MonoBehaviour
{
    public TabMenuData _tabData;

    private Dictionary<TabType, List<ShopItemBase>> _shopData_d = new Dictionary<TabType, List<ShopItemBase>>();

    private void Awake()
    {
        for (int i = 0; i < _tabData.tabs.Length; i++)
        {
            int iLocal = i;
            _tabData.tabs[i].onClick.AddListener(() => VisualiseItems((TabType)iLocal));
        }
    }

    public void AddToShop(ShopItemBase itemData, TabType tabType)
    {
        if (_shopData_d.TryGetValue(tabType, out var list))
            list.Add(itemData);
        else
            _shopData_d.Add(tabType, new List<ShopItemBase>() { itemData });
    }

    private void VisualiseItems(TabType tab)
    {
        //TODO: rewrite
        for (int i = 0; i < _tabData.contentContainer.childCount; i++)
        {
            _tabData.contentContainer.GetChild(i).gameObject.SetActive(false);
        }

        if (!_shopData_d.TryGetValue(tab, out var items))
            return;

        for (int i = 0; i < items.Count; i++)
        {
            //visualise items
            var miniView = Instantiate(_tabData.miniViewPrefab, _tabData.contentContainer);
            miniView.name = "miniView";

            //TODO: rewrite
            miniView.transform.GetChild(0).GetComponent<Image>().sprite = items[i].Sprite;
            miniView.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].Cost.ToString();
        }
        

    }
}
