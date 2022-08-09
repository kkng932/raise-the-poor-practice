using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class TableViewController<T> : ViewController
{
    protected List<T> tableData = new List<T>();        // ����Ʈ �׸��� �����͸� ����
    [SerializeField] private RectOffset padding;        // ��ũ�� �� ������ �е�
    [SerializeField] private float spacingHeight = 4.0f; // �� ���� ����

    // Scroll Rect ������Ʈ�� ĳ���Ѵ�
    private ScrollRect cachedScrollRect;
    public ScrollRect CachedScrollRect
    {
        get
        {
            if (cachedScrollRect == null)
            {
                cachedScrollRect = GetComponent<ScrollRect>();
            }
            return cachedScrollRect;
        }
    }

    // �ν��Ͻ��� �ε��� �� ȣ��ȴ�
    protected virtual void Awake()
    {

    }

    // ����Ʈ �׸� �����ϴ� ���� ���̸� ��ȯ�ϴ� �޼���
    protected float CellHeightAtIndex(int index)
    {
        // ���� ���� ��ȯ�ϴ� ó���� ����� Ŭ�������� �����Ѵ�
        return 150.0f;
    }

    // ��ũ�� �� ���� ��ü�� ���̸� �����ϴ� �޼���
    protected void UpdateContentSize()
    {
        // ��ũ�� �� ���� ��ü�� ���̸� ����Ѵ�
        float contentHeight = 0.0f;
        for (int i = 0; i < tableData.Count; i++)
        {
            contentHeight += CellHeightAtIndex(i);
            if (i > 0) { contentHeight += spacingHeight; }
        }

        // ��ũ�� �� ������ ���̸� �����Ѵ�
        Vector2 sizeDelta = CachedScrollRect.content.sizeDelta;
        sizeDelta.y = padding.top + contentHeight + padding.bottom;
        CachedScrollRect.content.sizeDelta = sizeDelta;
    }

    // ���� �޼��� ���� 
    [SerializeField] private GameObject cellBase;   // ���� ���� ��
    private LinkedList<TableViewCell<T>> cells = new LinkedList<TableViewCell<T>>();

    // �ν��Ͻ��� �ε��� �� Awake �޼��� ������ ȣ��ȴ�
    protected virtual void Start()
    {
        // ���� ���� ���� ��Ȱ��ȭ �صд�
        cellBase.SetActive(false);

        // Scroll Rect ������Ʈ�� ���� On Value Changed �̺�Ʈ�� �̺�Ʈ �����ʸ� �����Ѵ�
        CachedScrollRect.onValueChanged.AddListener(OnScrollPosChanged);
    }

    // ���� �����ϴ� �޼���
    private TableViewCell<T> CreateCellForindex(int index)
    {
        // ���� ���� ���� �̿��� ���ο� ���� �����Ѵ�
        GameObject obj = Instantiate(cellBase) as GameObject;
        obj.SetActive(true);
        TableViewCell<T> cell = obj.GetComponent<TableViewCell<T>>();

        // �θ� ��Ҹ� �ٲٸ� �������̳� ũ�⸦ �Ҿ�����Ƿ� ������ �����صд�.
        Vector3 scale = cell.transform.localScale;
        Vector2 sizeDelta = cell.CachedRectTransform.sizeDelta;
        Vector2 offsetMin = cell.CachedRectTransform.offsetMin;
        Vector2 offsetMax = cell.CachedRectTransform.offsetMax;

        cell.transform.SetParent(cellBase.transform.parent);

        // ���� �����ϰ� ũ�⸦ �����Ѵ�
        cell.transform.localScale = scale;
        cell.CachedRectTransform.sizeDelta = sizeDelta;
        cell.CachedRectTransform.offsetMin = offsetMin;
        cell.CachedRectTransform.offsetMax = offsetMax;

        // ������ �ε����� ���� ����Ʈ �׸� �����ϴ� ���� ������ �����Ѵ�
        UpdateCellForIndex(cell, index);
        cells.AddLast(cell);

        return cell;
    }

    // ���� ������ �����ϴ� �޼���
    private void UpdateCellForIndex(TableViewCell<T> cell, int index)
    {
        // ���� �����ϴ� ����Ʈ �׸��� �ε����� �����Ѵ�
        cell.DataIndex = index;

        if (cell.DataIndex >= 0 && cell.DataIndex <= tableData.Count - 1)
        {
            // ���� �����ϴ� ����Ʈ �׸��� �ִٸ� ���� Ȱ��ȭ�ؼ� ������ �����ϰ� ���̸� �����Ѵ�
            cell.gameObject.SetActive(true);
            cell.UpdateContent(tableData[cell.DataIndex]);
            cell.Height = CellHeightAtIndex(cell.DataIndex);
        }
        else
        {
            // ���� �����ϴ� ����Ʈ �׸��� ���ٸ� ���� ��Ȱ��ȭ���� ǥ�õ��� �ʰ� �Ѵ�
            cell.gameObject.SetActive(false);
        }
    }

    // visibleRect�� ���ǿ� visibleRect�� �����ϴ� �޼��� ����
    private Rect visibleRect;   // ����Ʈ �׸��� ���� ���·� ǥ���ϴ� ������ ��Ÿ���� �簢��
    [SerializeField] private RectOffset visibleRectPadding; // visibleRect�� �е�

    // visibleRect�� �����ϱ� ���� �޼���
    private void UpdateVisibleRect()
    {
        // visibleRect�� ��ġ�� ��ũ�� �� ������ �������κ��� ������� ��ġ��
        visibleRect.x = CachedScrollRect.content.anchoredPosition.x + visibleRectPadding.left;
        visibleRect.y = -CachedScrollRect.content.anchoredPosition.y + visibleRectPadding.top;

        // visibleRect�� ũ��� ��ũ�� ���� ũ�� + �е�
        visibleRect.width = CachedRectTransform.rect.width + visibleRectPadding.left + visibleRectPadding.right;
        visibleRect.height = CachedRectTransform.rect.height + visibleRectPadding.top + visibleRectPadding.bottom;
    }

    // ���̺� ���� ǥ�� ������ �����ϴ� ó���� ����
    protected void UpdateContents()
    {
        UpdateContentSize();       // ��ũ�� �� ������ ũ�⸦ �����Ѵ�
        UpdateVisibleRect();    // visibleRect�� �����Ѵ�

        if (cells.Count < 1)
        {
            // ���� �ϳ��� ���� ���� visibleRect�� ������ ���� ù ��° ����Ʈ �׸��� ã�Ƽ�
            // �׿� �����ϴ� ���� �ۼ��Ѵ�
            Vector2 cellTop = new Vector2(0.0f, -padding.top);
            for (int i = 0; i < tableData.Count; i++)
            {
                float cellHeight = CellHeightAtIndex(i);
                Vector2 cellBottom = cellTop + new Vector2(0.0f, -cellHeight);
                if ((cellTop.y <= visibleRect.y && cellTop.y >= visibleRect.y - visibleRect.height)
                    || (cellBottom.y <= visibleRect.y && cellBottom.y >= visibleRect.y - visibleRect.height))
                {
                    TableViewCell<T> cell = CreateCellForindex(i);
                    cell.Top = cellTop;
                    break;
                }
                cellTop = cellBottom + new Vector2(0.0f, spacingHeight);
            }

            // visibleRect�� ������ �� ���� ������ ���� �ۼ��Ѵ�
            FillVisibleRectWithCells();
        }
        else
        {
            // �̹� ���� ���� ���� ù ��° ������ ������� �����ϴ� ����Ʈ �׸���
            // �ε����� �ٽ� �����ϰ� ��ġ�� ������ �����Ѵ�
            LinkedListNode<TableViewCell<T>> node = cells.First;
            UpdateCellForIndex(node.Value, node.Value.DataIndex);
            node = node.Next;

            while (node != null)
            {
                UpdateCellForIndex(node.Value, node.Previous.Value.DataIndex + 1);
                node.Value.Top = node.Previous.Value.Bottom + new Vector2(0.0f, -spacingHeight);
                node = node.Next;
            }

            // visibleRect�� ������ �� ���� ������ ���� �ۼ��Ѵ�
            FillVisibleRectWithCells();
        }
    }

    //visibleRect ������ ǥ�õ� ��ŭ�� ���� �ۼ��ϴ� �޼���
    private void FillVisibleRectWithCells()
    {
        // ��
        if (cells.Count < 1)
            return;

        // ǥ�õ� ������ ���� �����ϴ� ����Ʈ �׸��� ���� ����Ʈ �׸��� �ְ� 
        // ���� �� ���� visibleRect�� ������ ���´ٸ� �����ϴ� ���� �ۼ��Ѵ�
        TableViewCell<T> lastCell = cells.Last.Value;
        int nextCellDataindex = lastCell.DataIndex + 1;
        Vector2 nextCellTop = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);

        while (nextCellDataindex < tableData.Count && nextCellTop.y >= visibleRect.y - visibleRect.height)
        {
            TableViewCell<T> cell = CreateCellForindex(nextCellDataindex);
            cell.Top = nextCellTop;

            lastCell = cell;
            nextCellDataindex = lastCell.DataIndex + 1;
            nextCellTop = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);
        }
    }

    // ���� �ٽ� �̿��ϴ� ó���� ���� 
    private Vector2 prevScrollPos;  // �ٷ� ���� ��ũ�� ��ġ�� ����

    // ��ũ�� �䰡 ��ũ�� ���� �� ȣ��ȴ�
    public void OnScrollPosChanged(Vector2 scrollPos)
    {
        // visibleRect
        UpdateVisibleRect();
        // ��ũ���� ���⿡ ���� ���� �ٽ� �̿��� ǥ�ø� �����Ѵ�
        ReuseCells((scrollPos.y < prevScrollPos.y) ? 1 : -1);

        prevScrollPos = scrollPos;

    }

    // ���� �ٽ� �̿��ؼ� ǥ�ø� �����ϴ� �޼���
    private void ReuseCells(int scrollDirection)
    {
        if (cells.Count < 1)
            return;

        if (scrollDirection > 0)
        {
            // ���� ��ũ���ϰ� ���� ���� visibleRect�� ������ �������� ���� �ִ� ����
            // �Ʒ��� ���� ������� �̵����� ������ �����Ѵ�
            TableViewCell<T> firstCell = cells.First.Value;
            while (firstCell.Bottom.y > visibleRect.y)
            {
                TableViewCell<T> lastCell = cells.Last.Value;
                UpdateCellForIndex(firstCell, lastCell.DataIndex + 1);
                firstCell.Top = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);

                cells.AddLast(firstCell);
                cells.RemoveFirst();
                firstCell = cells.First.Value;
            }

            // visibleRect�� ������ ���� �ȿ� �� ���� ������ ���� �ۼ��Ѵ�
            FillVisibleRectWithCells();
        }
        else if (scrollDirection < 0)
        {
            // �Ʒ��� ��ũ���ϰ� ���� ��� visibleRrect�� ������ �������� �Ʒ��� �ִ� ����
            // ���� ���� ������� �̵����� ������ �����Ѵ�
            TableViewCell<T> lastCell = cells.Last.Value;
            while (lastCell.Top.y < visibleRect.y - visibleRect.height)
            {
                TableViewCell<T> firstCell = cells.First.Value;
                UpdateCellForIndex(lastCell, firstCell.DataIndex - 1);
                lastCell.Bottom = firstCell.Top + new Vector2(0.0f, spacingHeight);

                cells.AddFirst(lastCell);
                cells.RemoveLast();
                lastCell = cells.Last.Value;

            }
        }
    }
}
