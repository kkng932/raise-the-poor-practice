raise-the-poor-practice  
===================================
copy practice of raise the poor
1. [close XML, reflection](#closed-xml-reflection)
2. [Dependency Injection](#Dependency-Injection)
3. [EventBus](#EventBus)

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

            dataList.Add(temp);
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
