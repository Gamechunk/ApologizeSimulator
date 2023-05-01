using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ShopItems/Item")]
public class ShopItemBase : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
    public bool IsBought { get; set; }
}
