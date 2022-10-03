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
## Dependency Injection

## 뷰포트 크기에 맞게 동적으로 셀 표시

## EventBus
