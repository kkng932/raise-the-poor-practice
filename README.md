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
## Dependency Injection

## 뷰포트 크기에 맞게 동적으로 셀 표시

## EventBus
