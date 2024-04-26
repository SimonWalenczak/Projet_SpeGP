using System;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionObject
{
    MustBeOn = 0,
    MustStuckTo = 1,
    MustInFrontOf = 2
}

public enum ObjToPlace
{
    Table = 0,
    Librairy = 1,
    Barrel = 2,
}

public enum ObjCondition
{
    Wall = 1,
    Door = 2,
    Table = 3,
}

[Serializable]
public class ObjectCondition
{
    public ObjToPlace ObjToPlace;
    public ConditionObject ConditionObject;
    public ObjCondition ObjCondition;
}

[CreateAssetMenu(fileName = "Data", menuName = "Room Meshes", order = 1)]
public class RoomMeshesData : ScriptableObject
{
    [field: SerializeField] public GameObject WallMesh { get; private set; }
    [field: SerializeField] public GameObject DoorMesh { get; private set; }
    [field: SerializeField] public GameObject PillarMesh { get; private set; }

    [field: SerializeField] public List<ObjectCondition> ObjectConditions;
}