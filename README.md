raise-the-poor-practice  
===================================
copy practice of raise the poor
1. [close XML, reflection](#closed-xml-reflection)
2. [Dependency Injection](#Dependency-Injection)
3. [뷰포트 크기에 맞게 동적으로 셀  표시](#뷰포트-크기에-맞게-동적으로-셀-표시)
4. [EventBus](#EventBus)

## closed XML, reflection

close XML을 이용해 엑셀파일을 데이터로 변환
```C#
    public static T ReadFromXlsx<T>(string xlsxPath)
    {
        var result = Activator.CreateInstance<T>();
        XLWorkbook workbook;
        var fs = File.Open(xlsxPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using (fs)
        {
            workbook = new XLWorkbook(fs);
        }
        var tType = typeof(T);
        var tFiledInfos = tType.GetFields();
        foreach (var fi in tFiledInfos)
        {
            IXLWorksheet sheet;
            workbook.TryGetWorksheet(fi.Name, out sheet);
            var fiType = fi.FieldType;
            // 제네릭 타입, 리스트형인지 확인
            if (fiType.IsGenericType && fiType.GetGenericTypeDefinition() == typeof(List<>))
            {
                IList dataList = GetListFromSheet(sheet, fiType);
                fi.SetValue(result, dataList);
            }
        }
        return result;
    }

```
불러와서 1행의 정보를 읽은 후 데이터 타입에 맞게 
```C#

    private static IList GetListFromSheet(IXLWorksheet sheet, Type fiType)
    {
        // 데이터를 담을 리스트 인스턴스
        var dataList = Activator.CreateInstance(fiType) as IList;
        // 리스트 Add 쓰기 위해 불러옴
        var method = fiType.GetMethod("Add");

        // 어떤 타입 리스트인지 저장
        var currType = fiType.GetGenericArguments()[0];

        // 속성값 읽어옴 (첫 번째 줄)
        var firstRow = sheet.Row(1);
        int columnIdx = 1;

        // 첫번째 행 읽어옴
        List<string> columns = new List<string>();
        while (true)
        {
            var cell = firstRow.Cell(columnIdx);
            if (cell == null)
            {
                break;
            }
            var column = cell.GetString();
            if (string.IsNullOrEmpty(column))
            {
                break;
            }
            columns.Add(column);
            columnIdx++;
        }


        int rowIdx = 2;
        while (true)
        {

            var currentRow = sheet.Row(rowIdx);
            if (string.IsNullOrEmpty(currentRow.Cell(1).GetString()))
            {
                break;
            }

            object temp = GetInstanceFromRow(currType, columns, currentRow);

            // Add
            dataList.Add(temp);
            //                    method.Invoke(dataList, new object[] { temp });
            rowIdx++;

        }

        return dataList;
    }

    private static object GetInstanceFromRow(Type currType, List<string> columns, IXLRow currentRow)
    {
        // 각 행마다 저장시킬 인스턴스 생성
        var temp = Activator.CreateInstance(currType);
        var columnIdx = 1;
        while (true)
        {
            var cell = currentRow.Cell(columnIdx);
            if (cell == null)
            {
                break;
            }
            var column = cell.GetString();

            if (string.IsNullOrEmpty(column))
            {
                break;
            }

            var fieldName = columns[columnIdx - 1];
            var fieldInfo = currType.GetField(fieldName);
            var ft = fieldInfo.FieldType;
            SetValue(temp, column, fieldInfo, ft);
            columnIdx++;
        }
        return temp;
    }

    private static void SetValue(object temp, string column, System.Reflection.FieldInfo fieldInfo, Type ft)
    {

        // 타입에 맞게 저장
        if (ft == typeof(int))
        {
            fieldInfo.SetValue(temp, int.Parse(column));
        }
        else if (ft == typeof(double))
        {
            fieldInfo.SetValue(temp, double.Parse(column));
        }
        else if (ft == typeof(float))
        {
            fieldInfo.SetValue(temp, float.Parse(column));
        }
        else if (ft == typeof(ulong))
        {
            // 지수 표기법일 때
            if (column.Contains("E") || column.Contains("e"))
            {
                int exponent = int.Parse(column[0].ToString());
                int fraction = int.Parse(column.Substring(column.IndexOf("+")));
                ulong ucolumn = (ulong)exponent;
                for (int i = 0; i < fraction; i++)
                    ucolumn *= 10;
                fieldInfo.SetValue(temp, ucolumn);

            }
            else
            {
                fieldInfo.SetValue(temp, ulong.Parse(column));
            }
            
        }
        else if (ft == typeof(BigInteger))
        {
            
            // 지수 표기법일 때
            if (column.Contains("E") || column.Contains("e"))
            {
                //int exponent = int.Parse(column[0].ToString());
                int fraction = int.Parse(column.Substring(column.IndexOf("+")));
                BigInteger bcolumn = BigInteger.Parse(column[0].ToString());
                for (int i = 0; i < fraction; i++)
                    bcolumn *= 10;
                //Debug.Log(bcolumn);
                fieldInfo.SetValue(temp, bcolumn);

            }
            else
            {
                //Debug.Log(column);
                fieldInfo.SetValue(temp, BigInteger.Parse(column));
            }

        }
        else if (ft == typeof(bool))
        {
            if (column.ToUpper().Equals("TRUE"))
            {
                fieldInfo.SetValue(temp, true);
            }
            else
                fieldInfo.SetValue(temp, false);
        }
        else if (ft == typeof(string))
        {
            fieldInfo.SetValue(temp, column);
        }
        
    }
```
엑셀 파일이 수정되면 자동으로 변환
```C#
class DetectChange : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {

        foreach (string str in importedAssets)
        {
            // 바뀐 데이터가 원하는 데이터인지
            if (str == "Assets/xlsx/data.xlsx")
            {
                MyMenu.XlsxToAsset();
                Debug.Log("DetectChange");
            }

        }
        foreach (string str in deletedAssets)
        {
            Debug.Log("Deleted Asset: " + str);
        }

        for (int i = 0; i < movedAssets.Length; i++)
        {
            Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
        }


    }
}
```
## Dependency Injection
```C#
public class InjectContainer
{
    static InjectContainer _Instance = new InjectContainer();
    public static InjectContainer Instance { get => _Instance; }

    Dictionary<Type, object> dic = new Dictionary<Type, object>();

    public void Regist<T>(T obj)
    {
        dic[typeof(T)] = obj;
    }

    public object Get(Type t)
    {
        return dic[t];
    }

    public void Inject<T>(T obj)
    {
        var type = typeof(T);

        //obj 필드중에 Inject 가 있는 필드를 찾아서
        //등록된 값을 obj의 필드에 넣어주는함수
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
        BindingFlags.DeclaredOnly | BindingFlags.Instance);

        foreach (var f in fields)
        {

            if (f.GetCustomAttribute<Inject>(false) != null)
            {
                f.SetValue(obj, Get(f.FieldType));
            }

        }
    }

    internal void Reset()
    {
        dic = new Dictionary<Type, object>();
    }
}

public class Inject : Attribute
{

}


public class InjectObj
{
    InjectContainer _InjectContainer;

    bool isInjected = false;

    public void Inject<T>(T t)
    {
        if (isInjected)
        {
            return;
        }
        isInjected = true;

        if (_InjectContainer == null)
        {
            _InjectContainer = InjectContainer.Instance;
        }

        _InjectContainer.Inject(t);

    }
}

```
사용할 때
```C#
[Inject]
    UserData userData;

    InjectObj InjectObj = new InjectObj();

    private void OnEnable()
    {
        InjectObj.Inject(this);
    }
```
## 뷰포트 크기에 맞게 동적으로 셀 표시
# 셀을 재활용하는 테이블 뷰  

ViewController.cs
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ViewController : MonoBehaviour
{
    // Rect Transform 컴포넌트를 캐시한다
    private RectTransform cachedRectTransform;
    public RectTransform CachedRectTransform
    {
        get
        {
            if (cachedRectTransform == null)
            {
                cachedRectTransform = GetComponent<RectTransform>();
            }
            return cachedRectTransform;
        }
    }
    // 뷰의 타이틀을 가져와서 설정하는 프로퍼티
    public virtual string Title { get { return ""; } set { } }


}
```
TableViewCell.cs
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableViewCell<T> : ViewController
{
    // 셀의 내용을 갱신하는 메서드
    public virtual void UpdateContent(T itemData)
    {
        // 실제 처리는 상속한 클래스에서 구현한다
    }

    // 셀에 대응하는 리스트 항목의 인덱스를 저장
    public int DataIndex { get; set; }

    // 셀의 높이를 나타내는 속성
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

    // 셀 위쪽 끝의 위치를 나타내는 속성
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

    // 셀 아래쪽 끝의 위치를 나타내는 속성
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
```

SpriteSheetManager.cs
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetManager 
{
    // 스프라이트 시트에 포함된 스프라이트를 캐시하는 딕셔너리
    private static Dictionary<string, Dictionary<string, Sprite>> spriteSheets = new Dictionary<string, Dictionary<string, Sprite>>();

    // 스프라이트 시트에 포함된 스프라이트를 읽어 들여 캐시하는 메서드
    public static void Load(string path)
    {
        if (!spriteSheets.ContainsKey(path))
        {
            spriteSheets.Add(path, new Dictionary<string, Sprite>());
        }

        // 스프라이트를 읽어 들여 이름과 관련지어서 캐시한다.
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);
        foreach(Sprite sprite in sprites)
        {
            if (!spriteSheets[path].ContainsKey(sprite.name))
            {
                
                spriteSheets[path].Add(sprite.name, sprite);
            }
        }
    }
    // 스프라이트 이름을 통해 스프라이트 시트에 포함된 스프라이트를 반환하는 메서드
    public static Sprite GetSpriteByName(string path, string name)
    {
        if (spriteSheets.ContainsKey(path) && spriteSheets[path].ContainsKey(name))
        {
            return spriteSheets[path][name];
        }
        return null;
    }

}
```
TableViewController.cs
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class TableViewController<T> : ViewController
{
    protected List<T> tableData = new List<T>();        // 리스트 항목의 데이터를 저장
    [SerializeField] private RectOffset padding;        // 스크롤 할 내용의 패딩
    [SerializeField] private float spacingHeight = 4.0f; // 각 셀의 간격

    // Scroll Rect 컴포넌트를 캐시한다
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

    // 인스턴스를 로드할 때 호출된다
    protected virtual void Awake()
    {

    }

    // 리스트 항목에 대응하는 셀의 높이를 반환하는 메서드
    protected float CellHeightAtIndex(int index)
    {
        // 실제 값을 반환하는 처리는 상속한 클래스에서 구현한다
        return 150.0f;
    }

    // 스크롤 할 내용 전체의 높이를 갱신하는 메서드
    protected void UpdateContentSize()
    {
        // 스크롤 할 내용 전체의 높이를 계산한다
        float contentHeight = 0.0f;
        for (int i =0;i<tableData.Count;i++)
        {
            contentHeight += CellHeightAtIndex(i);
            if (i > 0) { contentHeight += spacingHeight; }
        }

        // 스크롤 할 내용의 높이를 설정한다
        Vector2 sizeDelta = CachedScrollRect.content.sizeDelta;
        sizeDelta.y = padding.top + contentHeight + padding.bottom;
        CachedScrollRect.content.sizeDelta = sizeDelta;
    }

    // 내용 메서드 구현 
    [SerializeField] private GameObject cellBase;   // 복사 원본 셀
    private LinkedList<TableViewCell<T>> cells = new LinkedList<TableViewCell<T>>();

    // 인스턴스를 로드할 때 Awake 메서드 다음에 호출된다
    protected virtual void Start()
    {
        // 복사 원본 셀은 비활성화 해둔다
        cellBase.SetActive(false);

        // Scroll Rect 컴포넌트에 속한 On Value Changed 이벤트의 이벤트 리스너를 설정한다
        CachedScrollRect.onValueChanged.AddListener(OnScrollPosChanged);
    }

    // 셀을 생성하는 메서드
    private TableViewCell<T> CreateCellForindex(int index)
    {
        // 복사 원본 셀을 이용해 새로운 셀을 생성한다
        GameObject obj = Instantiate(cellBase) as GameObject;
        obj.SetActive(true);
        TableViewCell<T> cell = obj.GetComponent<TableViewCell<T>>();

        // 부모 요소를 바꾸면 스케일이나 크기를 잃어버리므로 변수에 저장해둔다.
        Vector3 scale = cell.transform.localScale;
        Vector2 sizeDelta = cell.CachedRectTransform.sizeDelta;
        Vector2 offsetMin = cell.CachedRectTransform.offsetMin;
        Vector2 offsetMax = cell.CachedRectTransform.offsetMax;

        cell.transform.SetParent(cellBase.transform.parent);

        // 셀의 스케일과 크기를 설정한다
        cell.transform.localScale = scale;
        cell.CachedRectTransform.sizeDelta = sizeDelta;
        cell.CachedRectTransform.offsetMin = offsetMin;
        cell.CachedRectTransform.offsetMax = offsetMax;

        // 지정된 인덱스가 붙은 리스트 항목에 대응하는 셀로 내용을 갱신한다
        UpdateCellForIndex(cell, index);
        cells.AddLast(cell);

        return cell;
    }

    // 셀의 내용을 갱신하는 메서드
    private void UpdateCellForIndex(TableViewCell<T> cell, int index)
    {
        // 셀에 대응하는 리스트 항목의 인덱스를 설정한다
        cell.DataIndex = index;
        
        if(cell.DataIndex >= 0 && cell.DataIndex <= tableData.Count - 1)
        {
            // 셀에 대응하는 리스트 항목이 있다면 셀을 활성화해서 내용을 갱신하고 높이를 설정한다
            cell.gameObject.SetActive(true);
            cell.UpdateContent(tableData[cell.DataIndex]);
            cell.Height = CellHeightAtIndex(cell.DataIndex);
        }
        else
        {
            // 셀에 대응하는 리스트 항목이 없다면 셀을 비활성화시켜 표시되지 않게 한다
            cell.gameObject.SetActive(false);
        }
    }

    // visibleRect의 정의와 visibleRect를 갱신하는 메서드 구현
    private Rect visibleRect;   // 리스트 항목을 셀의 형태로 표시하는 범위를 나타내는 사각형
    [SerializeField] private RectOffset visibleRectPadding; // visibleRect의 패딩

    // visibleRect를 갱신하기 위한 메서드
    private void UpdateVisibleRect()
    {
        // visibleRect의 위치는 스크롤 할 내용의 기준으로부터 상대적인 위치다
        visibleRect.x = CachedScrollRect.content.anchoredPosition.x + visibleRectPadding.left;
        visibleRect.y = -CachedScrollRect.content.anchoredPosition.y + visibleRectPadding.top;

        // visibleRect의 크기는 스크롤 뷰의 크기 + 패딩
        visibleRect.width = CachedRectTransform.rect.width + visibleRectPadding.left + visibleRectPadding.right;
        visibleRect.height = CachedRectTransform.rect.height + visibleRectPadding.top + visibleRectPadding.bottom;
    }

    // 테이블 뷰의 표시 내용을 갱신하는 처리를 구현
    protected void UpdateContents()
    {
        UpdateContentSize();       // 스크롤 할 내용의 크기를 갱신한다
        UpdateVisibleRect();    // visibleRect를 갱신한다

        if(cells.Count < 1)
        {
            // 셀이 하나도 없을 때는 visibleRect의 범위에 들어가는 첫 번째 리스트 항목을 찾아서
            // 그에 대응하는 셀을 작성한다
            Vector2 cellTop = new Vector2(0.0f, -padding.top);
            for (int i = 0;i<tableData.Count;i++)
            {
                float cellHeight = CellHeightAtIndex(i);
                Vector2 cellBottom = cellTop + new Vector2(0.0f, -cellHeight);
                if ((cellTop.y <= visibleRect.y && cellTop.y>=visibleRect.y-visibleRect.height)
                    ||(cellBottom.y <= visibleRect.y && cellBottom.y >= visibleRect.y - visibleRect.height))
                {
                    TableViewCell<T> cell = CreateCellForindex(i);
                    cell.Top = cellTop;
                    break;
                }
                cellTop = cellBottom + new Vector2(0.0f, spacingHeight);
            }

            // visibleRect의 범위에 빈 곳이 있으면 셀을 작성한다
            FillVisibleRectWithCells();
        }
        else
        {
            // 이미 셀이 있을 때는 첫 번째 셀부터 순서대로 대응하는 리스트 항목의
            // 인덱스를 다시 설정하고 위치와 내용을 갱신한다
            LinkedListNode<TableViewCell<T>> node = cells.First;
            UpdateCellForIndex(node.Value, node.Value.DataIndex);
            node = node.Next;

            while(node !=null)
            {
                UpdateCellForIndex(node.Value, node.Previous.Value.DataIndex + 1);
                node.Value.Top = node.Previous.Value.Bottom + new Vector2(0.0f, -spacingHeight);
                node = node.Next;
            }

            // visibleRect의 범위에 빈 곳이 있으면 셀을 작성한다
            FillVisibleRectWithCells();
        }
    }

    //visibleRect 범위에 표시될 만큼의 셀을 작성하는 메서드
    private void FillVisibleRectWithCells()
    {
        // 셀
        if (cells.Count < 1)
            return;

        // 표시된 마지막 셀에 대응하는 리스트 항목의 다음 리스트 항목이 있고 
        // 또한 그 셀이 visibleRect의 범위에 들어온다면 대응하는 셀을 작성한다
        TableViewCell<T> lastCell = cells.Last.Value;
        int nextCellDataindex = lastCell.DataIndex + 1;
        Vector2 nextCellTop = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);

        while(nextCellDataindex < tableData.Count && nextCellTop.y >= visibleRect.y - visibleRect.height)
        {
            TableViewCell<T> cell = CreateCellForindex(nextCellDataindex);
            cell.Top = nextCellTop;

            lastCell = cell;
            nextCellDataindex = lastCell.DataIndex + 1;
            nextCellTop = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);
        }
    }

    // 셀을 다시 이용하는 처리를 구현 
    private Vector2 prevScrollPos;  // 바로 전의 스크롤 위치를 저장
    
    // 스크롤 뷰가 스크롤 됐을 때 호출된다
    public void OnScrollPosChanged(Vector2 scrollPos)
    {
        // visibleRect
        UpdateVisibleRect();
        // 스크롤한 방향에 따라 셀을 다시 이용해 표시를 갱신한다
        ReuseCells((scrollPos.y < prevScrollPos.y) ? 1 : -1);

        prevScrollPos = scrollPos;

    }

    // 셀을 다시 이용해서 표시를 갱신하는 메서드
    private void ReuseCells(int scrollDirection)
    {
        if (cells.Count < 1)
            return;
        
        if(scrollDirection>0)
        {
            // 위로 스크롤하고 있을 때는 visibleRect에 지정된 범위보다 위에 있는 셀을
            // 아래를 향해 순서대로 이동시켜 내용을 갱신한다
            TableViewCell<T> firstCell = cells.First.Value;
            while(firstCell.Bottom.y>visibleRect.y)
            {
                TableViewCell<T> lastCell = cells.Last.Value;
                UpdateCellForIndex(firstCell, lastCell.DataIndex + 1);
                firstCell.Top = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);

                cells.AddLast(firstCell);
                cells.RemoveFirst();
                firstCell = cells.First.Value;
            }

            // visibleRect에 지정된 범위 안에 빈 곳이 있으면 셀을 작성한다
            FillVisibleRectWithCells();
        }
        else if (scrollDirection<0)
        {
            // 아래로 스크롤하고 있을 대는 visibleRrect에 지정된 범위보다 아래에 있는 셀을
            // 위로 향해 순서대로 이동시켜 내용을 갱신한다
            TableViewCell<T> lastCell = cells.Last.Value;
            while(lastCell.Top.y < visibleRect.y - visibleRect.height)
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

```

ShopItemTableViewController.cs
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ShopItemTableViewController : TableViewController<ShopItemData>
{
    // 리스트 항목의 데이터를 읽어 들이는 메서드
    private void LoadData()
    {

        // 데이터 소스로부터 가져오기
        tableData = new List<ShopItemData>()
        {
            new ShopItemData { iconName = "drink1", name = "hi1", price=100, description="des"},
            new ShopItemData { iconName = "drink2", name = "hi2", price=100, description="des"},
            new ShopItemData { iconName = "drink3", name = "hi3", price=100, description="des"},
            new ShopItemData { iconName = "drink4", name = "hi4", price=100, description="des"},
            new ShopItemData { iconName = "drink5", name = "hi5", price=100, description="des"},
            new ShopItemData { iconName = "drink6", name = "hi6", price=100, description="des"},
            new ShopItemData { iconName = "drink7", name = "hi7", price=100, description="des"},
            new ShopItemData { iconName = "drink8", name = "hi8", price=100, description="des"}
        }; 
        

        UpdateContents();
    }
    // 원래 가격에 따라 다른 크기로 하는 코드였다.  
    //protected override float CellHeightAtIndex(int index)
    //{
    //    return 128.0f;
    //}
    // 인스턴스가 로드될 때 Awake 메서드 다음에 호출된다
    protected override void Awake()
    {
        base.Awake();
        SpriteSheetManager.Load("IconAtlas");
    }
    // 인스턴스가 로드될 때 Awake 메서드 다음에 호출된다
    protected override void Start()
    {
        base.Start();
        LoadData();
    }
    
}

```

ShopItemTableViewCell.cs
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 리스트 항목 데이터 클래스를 정의
public class ShopItemData
{
    public string iconName;
    public string name;
    public int price;
    public string description;
}
// TableViewCell<T> 클래스를 상속한다
public class ShopItemTableViewCell : TableViewCell<ShopItemData>
{

    
    [SerializeField] private Image iconImage;
    [SerializeField] private Text nameLabel;
    [SerializeField] private Text priceLabel;

    // 셀의 내용을 갱신하는 메서드를 오버라이트한다
    public override void UpdateContent(ShopItemData itemData)
    {
        nameLabel.text = itemData.name;
        priceLabel.text = itemData.price.ToString();

        iconImage.sprite = SpriteSheetManager.GetSpriteByName("IconAtlas", itemData.iconName);
        
    }
    
}
```

## EventBus
```C#
public class EventBus 
{

    static Dictionary<System.Type, List<Delegate>> _handlerDic = new Dictionary<Type, List<Delegate>>();
 
    public static void Publish<T>(T ev)
    {
        var type = typeof(T);

        if (_handlerDic.ContainsKey(type) == false)
            return;

        foreach (var action in _handlerDic[type])
        {
            (action as System.Action<T>)(ev);
        }

    }

    public static void Subscribes<T>(System.Action<T> handler)
    {
        var type = typeof(T);
        if (_handlerDic.ContainsKey(type) == false)
        {
            _handlerDic.Add(type,new List<Delegate>());
        }
        _handlerDic[type].Add(handler);
    }

    public static void Unsubscribes<T>(System.Action<T> handler)
    {
        var type = typeof(T);
        if (_handlerDic.ContainsKey(type) == false)
            return;
        _handlerDic[type].Remove(handler as System.Action);
    }

}

```
