using ClosedXML.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Numerics;


public class MyMenu 
{
    [MenuItem("MyMenu/Test")]
    public static void Test()
    {
        Debug.Log("test");
    }

    [MenuItem("MyMenu/XlsxToAsset")]
    public static void XlsxToAsset()
    {
        var gameData = ReadFromXlsx<GameData>("Assets/xlsx/data.xlsx");

        AssetDatabase.CreateAsset(gameData, "Assets/Resources/data.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();


        //foreach (var data in gameData.Arbeit)
        //{
        //    Debug.Log(data.code + ", " + data.name + ", " + data.pay.ToString());
        //}



    }


    public static T ReadFromXlsx<T>(string xlsxPath)
    {
        var result = Activator.CreateInstance<T>();
        XLWorkbook workbook;
        var fs = File.Open(xlsxPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using (fs)
        {
            workbook = new XLWorkbook(fs);
        }

        //T�� �� ����Ʈ���� �ʵ带 ä�����Ѵ�.


        var tType = typeof(T);
        var tFiledInfos = tType.GetFields();

        // GameDate�� �ʵ� ��ȸ
        foreach (var fi in tFiledInfos)
        {
            IXLWorksheet sheet;
            workbook.TryGetWorksheet(fi.Name, out sheet);
            //var resultFType = rFieldInfos[resultIdx].FieldType;

            var fiType = fi.FieldType;

            // ���׸� Ÿ��, ����Ʈ������ Ȯ��
            if (fiType.IsGenericType && fiType.GetGenericTypeDefinition() == typeof(List<>))
            {
                IList dataList = GetListFromSheet(sheet, fiType);

                fi.SetValue(result, dataList);
            }


        }

        return result;
    }

    private static IList GetListFromSheet(IXLWorksheet sheet, Type fiType)
    {
        // �����͸� ���� ����Ʈ �ν��Ͻ�
        var dataList = Activator.CreateInstance(fiType) as IList;
        // ����Ʈ Add ���� ���� �ҷ���
        var method = fiType.GetMethod("Add");

        // � Ÿ�� ����Ʈ���� ����
        var currType = fiType.GetGenericArguments()[0];

        // �Ӽ��� �о�� (ù ��° ��)
        var firstRow = sheet.Row(1);
        int columnIdx = 1;

        // ù��° �� �о��
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
        // �� �ึ�� �����ų �ν��Ͻ� ����
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

        // Ÿ�Կ� �°� ����
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
            // ���� ǥ����� ��
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
            
            // ���� ǥ����� ��
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

}

