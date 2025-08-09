using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// CSVからマップデータを読み書き・編集・プレハブ配置できるエディタウィンドウ
/// </summary>
public class CSVloader : EditorWindow
{
    #region 変数

    /// <summary>
    /// プレハブの設定アセット（ScriptableObject）
    /// </summary>
    CSVMapPrefabSetting prefabSet;

    /// <summary>
    /// マップの幅・高さ
    /// </summary>
    int width = 10;
    int height = 10;

    /// <summary>
    /// マップのセルサイズ（ワールド空間）
    /// </summary>
    float cellSize = 1f;

    /// <summary>
    /// マップデータの2D配列（タイルIDを格納） [x, y]
    /// </summary>
    int[,] grid;

    /// <summary>
    /// 現在選択中のタイルID
    /// </summary>
    int selectTile = 1;

    /// <summary>
    /// スクロール位置
    /// </summary>
    Vector2 scrollPos;

    /// <summary>
    /// CSVファイルの保存先パス
    /// </summary>
    string fillPath = "Assets/Hito_Folder/Resources/map.csv";

    #endregion

    #region メニュー画面

    [MenuItem("Tools/CSV Map Editor")]
    public static void OpenWindow()
    {
        GetWindow<CSVloader>("CSV Map Editor");
    }

    #endregion

    #region Unity Lifecycle

    private void OnEnable()
    {
        grid = new int[width, height];
    }

    private void OnGUI()
    {
        DrawSettings();
        DrawPrefabSelector();
        DrawMapGrid();
        DrawButtons();
    }

    #endregion

    #region GUI 設定

    /// <summary>
    /// マップサイズなどの基本設定項目
    /// </summary>
    void DrawSettings()
    {
        EditorGUILayout.LabelField("Map Settings", EditorStyles.boldLabel);

        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);
        cellSize = EditorGUILayout.FloatField("Cell Size", cellSize);

        // サイズが変わったら新しく配列を作る
        if (grid == null || grid.GetLength(0) != width || grid.GetLength(1) != height)
        {
            grid = new int[width, height];
        }

        EditorGUILayout.Space();
    }

    /// <summary>
    /// プレハブセットの選択とサムネイルによるタイル選択
    /// </summary>
    void DrawPrefabSelector()
    {
        prefabSet = (CSVMapPrefabSetting)EditorGUILayout.ObjectField("Prefab Setting", prefabSet, typeof(CSVMapPrefabSetting), false);

        if (prefabSet != null && prefabSet.prefabs != null && prefabSet.prefabs.Length > 0)
        {
            EditorGUILayout.LabelField("Select Tile (Click Thumbnail):");

            int columns = 4;
            int count = prefabSet.prefabs.Length;
            int rows = Mathf.CeilToInt((float)count / columns);

            for (int y = 0; y < rows; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < columns; x++)
                {
                    int index = y * columns + x;
                    if (index >= count) break;

                    GameObject prefab = prefabSet.prefabs[index];
                    if (prefab == null) continue;

                    Texture2D preview = AssetPreview.GetAssetPreview(prefab);
                    if (preview == null) preview = AssetPreview.GetMiniThumbnail(prefab);

                    if (GUILayout.Button(preview, GUILayout.Width(64), GUILayout.Height(64)))
                    {
                        selectTile = index;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.LabelField($"Current Tile ID: {selectTile}");
        }
        else
        {
            EditorGUILayout.HelpBox("プレハブセットを設定してください", MessageType.Warning);
        }

        EditorGUILayout.Space();
    }

    /// <summary>
    /// タイルマップグリッドを表示し、クリックで編集
    /// </summary>
    void DrawMapGrid()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int y = 0; y < height; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < width; x++)
            {
                string label = grid[x, y].ToString();
                if (GUILayout.Button(label, GUILayout.Width(30), GUILayout.Height(30)))
                {
                    if (Event.current.button == 1)
                        grid[x, y] = 0; // 右クリックで消去
                    else
                        grid[x, y] = selectTile; // 左クリックで配置
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space();
    }

    /// <summary>
    /// 操作用ボタン群（Clear, Save, Load, Generate）
    /// </summary>
    void DrawButtons()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear Map"))
        {
            grid = new int[width, height];
        }

        if (GUILayout.Button("Save Map"))
        {
            SaveMap();
        }

        if (GUILayout.Button("Load Map"))
        {
            LoadMap();
        }

        if (GUILayout.Button("Generate Map"))
        {
            GenerateMap();
        }

        EditorGUILayout.EndHorizontal();
    }

    #endregion

    #region CSVMap保存・読み込み・生成

    /// <summary>
    /// マップデータをCSV形式で保存
    /// </summary>
    void SaveMap()
    {
        using (StreamWriter sw = new StreamWriter(fillPath))
        {
            for (int y = 0; y < height; y++)
            {
                string[] row = new string[width];
                for (int x = 0; x < width; x++)
                    row[x] = grid[x, y].ToString();

                sw.WriteLine(string.Join(",", row));
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Map saved to " + fillPath);
    }

    /// <summary>
    /// CSVファイルからマップデータを読み込み
    /// </summary>
    void LoadMap()
    {
        if (!File.Exists(fillPath))
        {
            Debug.LogError("マップファイルが見つかりません: " + fillPath);
            return;
        }

        string[] lines = File.ReadAllLines(fillPath);
        height = lines.Length;
        width = lines[0].Split(',').Length;
        grid = new int[width, height];

        for (int y = 0; y < height; y++)
        {
            string[] values = lines[y].Split(',');
            for (int x = 0; x < width; x++)
                int.TryParse(values[x], out grid[x, y]);
        }

        Debug.Log("Map loaded from " + fillPath);
    }

    /// <summary>
    /// ウィンドウの左下と右上のワールド座標を取得
    /// </summary>
    public static (Vector3 lb, Vector3 rt) GetWindowPos()
    {
        Vector3 lb = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 rt = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        return (lb, rt);
    }

    /// <summary>
    /// ワールド座標をローカル座標に変換
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="wPos"></param>
    /// <returns></returns>
    public static Vector2 WPosToLPos(GameObject obj, Vector2 wPos)
    {
        var IPos = obj.transform.InverseTransformPoint(wPos);
        return IPos;
    }
    /// <summary>
    /// マップ上にプレハブを配置して生成
    /// </summary>
    void GenerateMap()
    {
        if (prefabSet == null || prefabSet.prefabs == null || prefabSet.prefabs.Length == 0)
        {
            Debug.LogError("マップ生成に使うプレファブがありません");
            return;
        }

        GameObject oldMap = GameObject.Find("GeneratedMap");
        if (oldMap != null)
            DestroyImmediate(oldMap);

        GameObject parent = new GameObject("GeneratedMap");

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int id = grid[x, y];
                if (id > 0 && id < prefabSet.prefabs.Length && prefabSet.prefabs[id] != null)
                {
                    Vector3 pos = new Vector3(x, -y, 0) * cellSize;
                    Vector3 startPos = new Vector3(GetWindowPos().lb.x, GetWindowPos().rt.y, 0);
                    GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefabSet.prefabs[id]);
                    obj.transform.position = startPos + pos;
                    obj.transform.localScale = Vector3.one * cellSize;
                    obj.transform.SetParent(parent.transform);
                }
            }
        }
    }

    #endregion
}
