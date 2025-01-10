using System.Collections.Generic;
using Common;
using Procedural;
using UnityEngine;

namespace Managers
{
    public enum EditorState
    {
        PrimalExpand = 0,
        RoomModifier = 1
    }
    
    public class RoomEditorManager : Singleton<RoomEditorManager>
    {
        [field: SerializeField] public UIManager UIManager { get; private set; }
        [field: SerializeField] public RoomPrimalExpand RoomPrimalExpand { get; private set; }
        [field: SerializeField] public RoomModifier RoomModifier { get; private set; }
        [field: SerializeField] public DoorGenerator DoorGenerator { get; private set; }
        
        [Header("Room Meshes")] [SerializeField] public RoomMeshesData RoomMeshes;
        [HideInInspector] public List<GameObject> WallPieces = new List<GameObject>();
        [HideInInspector] public List<GameObject> Pillars = new List<GameObject>();

        [Space] public EditorState EditorState;
        
        public void GenerateDoors()
        {
            RoomModifier.ResetWalls();
            DoorGenerator.GenerateDoorsRandomPosition();
        }
    }
}
