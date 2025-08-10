using UnityEngine;
using System;

public class MapLoader : MonoBehaviour
{
    public CSVMapPrefabSetting prefabSet;
    public string mapFileName = "Stage1"; // 拡張子不要（Resources内のファイル名）

    public float cellSize = 1f;

    void Start()
    {
        LoadAndGenerateMap(mapFileName);
    }

    void LoadAndGenerateMap(string fileName)
    {
        // CSVファイル読み込み（Resources/ の中）
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError("CSVファイルが見つかりません: " + fileName);
            return;
        }

        string[] lines = csvFile.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int height = lines.Length;
        int width = lines[0].Split(',').Length;

        int[,] grid = new int[width, height];

        // CSV -> grid 変換
        for (int y = 0; y < height; y++)
        {
            string[] values = lines[y].Split(',');
            for (int x = 0; x < width; x++)
            {
                int.TryParse(values[x], out grid[x, y]);
            }
        }

        // マップ生成
        GenerateMap(grid, width, height);
    }

    void GenerateMap(int[,] grid, int width, int height)
    {
        GameObject parent = new GameObject("GeneratedMap");

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int id = grid[x, y];
                if (id > 0 && id < prefabSet.prefabs.Length && prefabSet.prefabs[id] != null)
                {
                    GameObject obj = Instantiate(prefabSet.prefabs[id], parent.transform);
                    obj.transform.position = new Vector3(x + 0.5f, -y - 0.5f, 0) * cellSize;
                    obj.transform.localScale = Vector3.one * cellSize;
                }
            }
        }
    }
}
