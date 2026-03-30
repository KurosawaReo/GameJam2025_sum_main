using UnityEngine;

public class CameraSizeSettings : MonoBehaviour
{
    public Camera targetCamera; // 調整するカメラ
    public int mapWidth = 10;   // タイル横数
    public int mapHeight = 10;  // タイル縦数
    public Transform mapParent; // マップの親

    public float cellSize; // 自動計算されるセルサイズ

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        UpdateCellSize();
        AlignMap();
    }

    void UpdateCellSize()
    {
        // カメラのワールド高さ
        float worldHeight = targetCamera.orthographicSize * 2f;
        // カメラのワールド幅
        float worldWidth = worldHeight * targetCamera.aspect;

        // 横と縦のどちらにも収まるセルサイズを計算
        float cellWidth = worldWidth / mapWidth;
        float cellHeight = worldHeight / mapHeight;
        cellSize = Mathf.Min(cellWidth, cellHeight);

        Debug.Log($"Calculated cell size: {cellSize}");
    }

    void AlignMap()
    {
        if (mapParent != null)
        {
            // マップ全体のサイズ
            float mapWorldWidth = mapWidth * cellSize;
            float mapWorldHeight = mapHeight * cellSize;

            // カメラの中心にマップを配置（左下基準ではなく中央揃え）
            Vector3 newPos = new Vector3(-mapWorldWidth / 2f + cellSize / 2f,
                                         mapWorldHeight / 2f - cellSize / 2f,
                                         0f);
            mapParent.position = newPos;
        }
    }

    // 生成するオブジェクトをマスにスナップ
    public Vector3 GetSnappedPosition(Vector3 worldPos)
    {
        float x = Mathf.Round(worldPos.x / cellSize) * cellSize;
        float y = Mathf.Round(worldPos.y / cellSize) * cellSize;
        return new Vector3(x, y, worldPos.z);
    }
}
