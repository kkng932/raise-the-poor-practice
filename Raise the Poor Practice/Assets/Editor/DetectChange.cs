using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


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