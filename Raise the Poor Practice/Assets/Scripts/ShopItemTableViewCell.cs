using UnityEngine;
using UnityEngine.UI;

public class ShopItemTableViewCell : TableViewCell<Arbeit>
{
    [SerializeField] private Text DescTxt;

    // ���� ������ �����ϴ� �޼��带 �������̵�
    public override void UpdateContent(Arbeit itemData)
    {
        DescTxt.text = itemData.name;
    }
}
