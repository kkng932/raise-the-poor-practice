using UnityEngine;
using UnityEngine.UI;

public class ShopItemTableViewCell : TableViewCell<Arbeit>
{
    [SerializeField] private Text DescTxt;

    // 셀의 내용을 갱신하는 메서드를 오버라이드
    public override void UpdateContent(Arbeit itemData)
    {
        DescTxt.text = itemData.name;
    }
}
