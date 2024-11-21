using Common;
using Procedural;
using UnityEngine;

namespace Managers
{
    public class RoomEditorManager : Singleton<RoomEditorManager>
    {
        [field: SerializeField] public UIManager UIManager { get; private set; }
        [field: SerializeField] public RoomExpand RoomExpand { get; private set; }
    }
}
