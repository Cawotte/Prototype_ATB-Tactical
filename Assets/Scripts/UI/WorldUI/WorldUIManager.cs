
namespace Cawotte.Tactical.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using Cawotte.Utils;

    public class WorldUIManager : Singleton<WorldUIManager>
    {

        [SerializeField] Camera mainCamera;
        [SerializeField] Transform worldCanvas;
        [SerializeField] Transform charactersUIParent;

        public Transform WorldCanvas { get => worldCanvas; }
        public Transform CharactersUIParent { get => charactersUIParent; }
        
    }
}
