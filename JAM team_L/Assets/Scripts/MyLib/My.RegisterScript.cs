/*
   - MyLib.RegisterScript -
   ver.2025/08/13
*/
using UnityEngine;

/// <summary>
/// MyLibで使うScriptableObject集.
/// </summary>
namespace MyLib.RegisterScript
{
    /// <summary>
    /// 使用するprefabのパーツを登録する.
    /// Create > MyTools > MapPartsで新しく出せる.
    /// </summary>
    [CreateAssetMenu(fileName = "New Parts", menuName = "MyTools/MapParts")]
    public class MapParts : ScriptableObject
    {
        public GameObject[] prefabs; //これでGameObjectを登録できる.
    }
}