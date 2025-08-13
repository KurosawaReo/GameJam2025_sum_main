/*
   - MyLib.RegisterScript -
   ver.2025/08/13
*/
using UnityEngine;

/// <summary>
/// MyLib�Ŏg��ScriptableObject�W.
/// </summary>
namespace MyLib.RegisterScript
{
    /// <summary>
    /// �g�p����prefab�̃p�[�c��o�^����.
    /// Create > MyTools > MapParts�ŐV�����o����.
    /// </summary>
    [CreateAssetMenu(fileName = "New Parts", menuName = "MyTools/MapParts")]
    public class MapParts : ScriptableObject
    {
        public GameObject[] prefabs; //�����GameObject��o�^�ł���.
    }
}