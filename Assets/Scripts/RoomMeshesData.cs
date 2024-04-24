using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Room Meshes", order = 1)]
public class RoomMeshesData : ScriptableObject
{
    [field: SerializeField] public GameObject WallMesh { get; private set; }
    [field: SerializeField] public GameObject DoorMesh { get; private set; }
    [field: SerializeField] public GameObject PillarMesh { get; private set; }
}