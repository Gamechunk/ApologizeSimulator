using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [field: SerializeField] public ShopItemBase[] ItemDatas { get; private set; }

    private TabMenuController _tabController;

    private void Awake()
    {
        _tabController = GetComponentInChildren<TabMenuController>();
    }

    private void Start()
    {
        //TODO: rewrite
        _tabController.AddToShop(ItemDatas[0], TabType.Head);
        _tabController.AddToShop(ItemDatas[1], TabType.Head);
        _tabController.AddToShop(ItemDatas[2], TabType.Head);
        _tabController.AddToShop(ItemDatas[3], TabType.Top);
    }

}
