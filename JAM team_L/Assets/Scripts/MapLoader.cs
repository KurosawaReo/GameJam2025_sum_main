using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class CSVMapLoader : MonoBehaviour
{
    public Transform mapRoot; 
    public CSVMapPrefabSetting prefabSet;
    public float cellSize = 1f; // セルサイズを追加
    public Vector3 startPos;    // スタート位置をインスペクタで設定（エディタと同じ基準にする）

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string difficultyName = ConvertSceneNameToDifficulty(sceneName);
        LoadMap(difficultyName);
    }

    string ConvertSceneNameToDifficulty(string sceneName)
    {
        switch (sceneName)
        {
            case "StageEasy":
                return "Easy";

            case "StageNormal":
                return "Normal";

            case "StageHard":
                return "Hard";

            default:
                Debug.LogWarning($"シーン名 '{sceneName}' に対応する難易度が見つかりません。デフォルトでEasyを使用します。");
                return "Easy";
        }
    }


    void LoadMap(string difficulty)
    {
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
                            Vector3 pos = new Vector3(x + 0.5f, -y - 0.5f, 0) * cellSize;
                            Instantiate(prefab, startPos + pos, Quaternion.identity, mapRoot);
                        }
                    }
                }
            }
        }

        Debug.Log($"マップロード完了: {difficulty}");
    }
}
