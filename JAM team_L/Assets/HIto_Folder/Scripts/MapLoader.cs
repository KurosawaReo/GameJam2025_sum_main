using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class CSVMapLoader : MonoBehaviour
{
    public Transform mapRoot; // マップを生成する親オブジェクト
    public CSVMapPrefabSetting prefabSet; // プレハブの設定（ScriptableObject）

    void Start()
    {
        string difficultyName = SceneManager.GetActiveScene().name; // シーン名が難易度名
        LoadMap(difficultyName);
    }

    void LoadMap(string difficulty)
    {
        // CSVファイル名を "Maps/Easy.csv" のようにしておく
        string filePath = Path.Combine(Application.streamingAssetsPath, "Maps", difficulty + ".csv");

        if (!File.Exists(filePath))
        {
            Debug.LogError($"CSVファイルが見つかりません: {filePath}");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);

        for (int y = 0; y < lines.Length; y++)
        {
            string[] cells = lines[y].Split(',');

            for (int x = 0; x < cells.Length; x++)
            {
                int id;
                if (int.TryParse(cells[x], out id))
                {
                    if (id >= 0 && id < prefabSet.prefabs.Length)
                    {
                        GameObject prefab = prefabSet.prefabs[id];
                        if (prefab != null)
                        {
                            Instantiate(prefab, new Vector3(x, -y, 0), Quaternion.identity, mapRoot);
                        }
                    }
                }
            }
        }

        Debug.Log($"マップロード完了: {difficulty}");
    }
}
