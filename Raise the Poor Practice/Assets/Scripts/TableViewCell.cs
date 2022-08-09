using UnityEngine;

public class TableViewCell<T> : ViewController
{
    // ���� ������ �����ϴ� �޼���
    public virtual void UpdateContent(T itemData)
    {
        // ���� ó���� ����� Ŭ�������� �����Ѵ�
    }

    // ���� �����ϴ� ����Ʈ �׸��� �ε����� ����
    public int DataIndex { get; set; }

    // ���� ���̸� ��Ÿ���� �Ӽ�
    public float Height
    {
        get { return CachedRectTransform.sizeDelta.y; }
        set
        {
            Vector2 sizeDelta = CachedRectTransform.sizeDelta;
            sizeDelta.y = value;
            CachedRectTransform.sizeDelta = sizeDelta;
        }
    }

    // �� ���� ���� ��ġ�� ��Ÿ���� �Ӽ�
    public Vector2 Top
    {
        get
        {
            Vector3[] corners = new Vector3[4];
            CachedRectTransform.GetLocalCorners(corners);
            return CachedRectTransform.anchoredPosition + new Vector2(0.0f, corners[1].y);
        }
        set
        {
            Vector3[] corners = new Vector3[4];
            CachedRectTransform.GetLocalCorners(corners);
            CachedRectTransform.anchoredPosition = value - new Vector2(0.0f, corners[1].y);
        }
    }

    // �� �Ʒ��� ���� ��ġ�� ��Ÿ���� �Ӽ�
    public Vector2 Bottom
    {
        get
        {
            Vector3[] corners = new Vector3[4];
            CachedRectTransform.GetLocalCorners(corners);
            return CachedRectTransform.anchoredPosition + new Vector2(0.0f, corners[3].y);
        }
        set
        {
            Vector3[] corners = new Vector3[4];
            CachedRectTransform.GetLocalCorners(corners);
            CachedRectTransform.anchoredPosition = value - new Vector2(0.0f, corners[3].y);
        }
    }
}
