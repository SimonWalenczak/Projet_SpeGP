using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomExpand : MonoBehaviour
{
    #region Fields and Properties

    [Header("Room Meshes")] [SerializeField]
    private RoomMeshesData _roomMeshes;

    [Space(10)] [Header("Room Properties")] [SerializeField]
    private Vector2 _roomSize;

    [SerializeField] private bool _usingHandle;
    [SerializeField] private GameObject ExpandHandleLength;
    [SerializeField] private GameObject ExpandHandleWidth;

    private float _initialHandleLengthPosition;
    private float _initialHandleWidthPosition;

    [Space(10)] public bool UsingGenerationStep;

    [ConditionalHide("UsingGenerationStep", true)] [SerializeField]
    private float _regenerationStep;

    private float _regenerationStepSavedValue;

    [Space(10)] [field: Header("Door Generation")]
    public bool RamdomDoorPosition;

    [HideInInspector] public bool DefinedDoorPosition;

    [Space(10)] [ConditionalHide("RamdomDoorPosition", true)] [SerializeField]
    private int _numberOfDoors;

    private List<GameObject> Walls = new List<GameObject>();

    private List<GameObject> Pillars = new List<GameObject>();

    //Hidden
    private float _previousRoomSizeX;
    private float _previousRoomSizeY;

    private int _wallCount;
    private Vector2 _wallSize;

    #endregion

    #region Methods

    private void OnValidate()
    {
        DefinedDoorPosition = !RamdomDoorPosition;

        if (UsingGenerationStep)
        {
            _regenerationStepSavedValue = _regenerationStep;
        }
        else
        {
            _regenerationStepSavedValue = 0;
        }
    }

    private void Start()
    {
        _wallSize.x = _roomMeshes.WallMesh.GetComponent<MeshRenderer>().bounds.size.x;
        _wallSize.y = _roomMeshes.WallMesh.GetComponent<MeshRenderer>().bounds.size.x;

        _roomSize.x = _wallSize.x;
        _roomSize.y = _wallSize.y;

        _previousRoomSizeX = _roomSize.x;

        ExpandHandleLength.transform.position = new Vector3(transform.position.x + _wallSize.x,
            transform.position.y + 2,
            transform.position.z);
        ExpandHandleWidth.transform.position = new Vector3(transform.position.x, transform.position.y + 2,
            transform.position.z + _wallSize.y);

        _initialHandleLengthPosition = ExpandHandleLength.transform.position.x - (_wallSize.x / 2);
        _initialHandleWidthPosition = ExpandHandleWidth.transform.position.z - (_wallSize.y / 2);

        CreateWalls();
    }

    private void Update()
    {
        float diffA = 0;
        float diffB = 0;
        
        if (_usingHandle)
        {
            diffA = ExpandHandleLength.transform.position.x - _initialHandleLengthPosition;
            diffB = ExpandHandleWidth.transform.position.z - _initialHandleWidthPosition;
            
            _roomSize.x = _wallSize.x + diffA;
            _roomSize.y = _wallSize.y + diffB;
        }

        //Limit to minimum
        if (_roomSize.x <= _wallSize.x)
            _roomSize.x = _wallSize.x;
        if (_roomSize.y <= _wallSize.y)
            _roomSize.y = _wallSize.y;

        //Room X
        if (_roomSize.x > _previousRoomSizeX + _regenerationStepSavedValue)
        {
            _previousRoomSizeX = _roomSize.x;
            CreateWalls();
        }
        else if (_roomSize.x < _previousRoomSizeX - _regenerationStepSavedValue)
        {
            _previousRoomSizeX = _roomSize.x;
            CreateWalls();
        }

        //Room Y
        if (_roomSize.y > _previousRoomSizeY + _regenerationStepSavedValue)
        {
            _previousRoomSizeY = _roomSize.y;
            CreateWalls();
        }
        else if (_roomSize.y < _previousRoomSizeY - _regenerationStepSavedValue)
        {
            _previousRoomSizeY = _roomSize.y;
            CreateWalls();
        }
    }

    private void CreateWalls()
    {
        //Reset Wall List
        foreach (var wall in Walls)
        {
            Destroy(wall);
        }

        Walls.Clear();

        //Reset Pillar List
        foreach (var pillar in Pillars)
        {
            Destroy(pillar);
        }

        Pillars.Clear();

        // Define size of walls
        int wallCountX = Mathf.Max(1, (int)(_roomSize.x / _wallSize.x));
        int wallCountY = Mathf.Max(1, (int)(_roomSize.y / _wallSize.y));
        float scaleX = (_roomSize.x / wallCountX) / _wallSize.x;
        float scaleY = (_roomSize.y / wallCountY) / _wallSize.y;

        //Define if the wall piece is Wall or Door


        // Create first row of walls
        for (int i = 0; i < wallCountX; i++)
        {
            Vector3 positionX = transform.position +
                                new Vector3(-_roomSize.x / 2 + _wallSize.x * scaleX / 2 + i * scaleX * _wallSize.x, 0,
                                    +_roomSize.y / 2);
            Quaternion rotationX = transform.rotation;
            Vector3 localScaleX = new Vector3(scaleX, 1, 1);

            GameObject currentWallX = null;


            currentWallX = Instantiate(_roomMeshes.WallMesh, positionX, rotationX);


            currentWallX.transform.localScale = localScaleX;
            currentWallX.transform.parent = transform;
            Walls.Add(currentWallX);

            // Add pillar between walls
            if (i < wallCountX - 1)
            {
                Vector3 pillarPosition = positionX + new Vector3(scaleX * _wallSize.x / 2, 0, 0);
                GameObject pillar = Instantiate(_roomMeshes.PillarMesh, pillarPosition, Quaternion.identity);
                pillar.transform.localScale = new Vector3(1, 1, scaleY);
                pillar.transform.parent = transform;
                Pillars.Add(pillar);
            }
        }

        // Create second row of walls parallel to the first row
        for (int j = 0; j < wallCountX; j++)
        {
            Vector3 positionY = transform.position +
                                new Vector3(-_roomSize.x / 2 + _wallSize.x * scaleX / 2 + j * scaleX * _wallSize.x, 0,
                                    +_roomSize.y / 2 - _roomSize.y);
            Quaternion rotationY = transform.rotation;
            Vector3 localScaleY = new Vector3(scaleX, 1, 1);

            GameObject currentWallY = null;


            currentWallY = Instantiate(_roomMeshes.WallMesh, positionY, rotationY);


            currentWallY.transform.localScale = localScaleY;
            currentWallY.transform.parent = transform;
            Walls.Add(currentWallY);

            // Add pillar between walls
            if (j < wallCountX - 1)
            {
                Vector3 pillarPosition = positionY + new Vector3(scaleX * _wallSize.x / 2, 0, 0);
                GameObject pillar = Instantiate(_roomMeshes.PillarMesh, pillarPosition, Quaternion.identity);
                pillar.transform.localScale = new Vector3(1, 1, scaleY);
                pillar.transform.parent = transform;
                Pillars.Add(pillar);
            }
        }

        // Create left wall
        for (int k = 0; k < wallCountY; k++)
        {
            Vector3 positionLeft = transform.position +
                                   new Vector3(-_roomSize.x / 2, 0,
                                       -_roomSize.y / 2 + _wallSize.y * scaleY / 2 + k * scaleY * _wallSize.y);
            Quaternion rotationLeft = Quaternion.Euler(0, 90, 0); // Rotate 90 degrees for left wall
            Vector3 localScaleLeft = new Vector3(scaleY, 1, 1);

            GameObject currentWallLeft = null;

            currentWallLeft = Instantiate(_roomMeshes.WallMesh, positionLeft, rotationLeft);

            currentWallLeft.transform.localScale = localScaleLeft;
            currentWallLeft.transform.parent = transform;
            Walls.Add(currentWallLeft);

            // Add pillar between walls
            if (k < wallCountY - 1)
            {
                Vector3 pillarPosition = positionLeft + new Vector3(0, 0, scaleY * _wallSize.y / 2);
                GameObject pillar = Instantiate(_roomMeshes.PillarMesh, pillarPosition, Quaternion.identity);
                pillar.transform.localScale = new Vector3(scaleX, 1, 1);
                pillar.transform.parent = transform;
                Pillars.Add(pillar);
            }
        }

        // Create right wall
        for (int l = 0; l < wallCountY; l++)
        {
            Vector3 positionRight = transform.position +
                                    new Vector3(+_roomSize.x / 2, 0,
                                        -_roomSize.y / 2 + _wallSize.y * scaleY / 2 + l * scaleY * _wallSize.y);
            Quaternion rotationRight = Quaternion.Euler(0, -90, 0); // Rotate -90 degrees for right wall
            Vector3 localScaleRight = new Vector3(scaleY, 1, 1);

            GameObject currentWallRight = null;


            currentWallRight = Instantiate(_roomMeshes.WallMesh, positionRight, rotationRight);

            currentWallRight.transform.localScale = localScaleRight;
            currentWallRight.transform.parent = transform;
            Walls.Add(currentWallRight);

            // Add pillar between walls
            if (l < wallCountY - 1)
            {
                Vector3 pillarPosition = positionRight + new Vector3(0, 0, scaleY * _wallSize.y / 2);
                GameObject pillar = Instantiate(_roomMeshes.PillarMesh, pillarPosition, Quaternion.identity);
                pillar.transform.localScale = new Vector3(scaleX, 1, 1);
                pillar.transform.parent = transform;
                Pillars.Add(pillar);
            }
        }

        // Create corner pillars
        Vector3 cornerPosition1 = transform.position + new Vector3(-_roomSize.x / 2, 0, +_roomSize.y / 2);
        Vector3 cornerPosition2 = transform.position + new Vector3(-_roomSize.x / 2, 0, -_roomSize.y / 2);
        Vector3 cornerPosition3 = transform.position + new Vector3(+_roomSize.x / 2, 0, +_roomSize.y / 2);
        Vector3 cornerPosition4 = transform.position + new Vector3(+_roomSize.x / 2, 0, -_roomSize.y / 2);

        GameObject cornerPillar1 = Instantiate(_roomMeshes.PillarMesh, cornerPosition1, Quaternion.identity);
        GameObject cornerPillar2 = Instantiate(_roomMeshes.PillarMesh, cornerPosition2, Quaternion.identity);
        GameObject cornerPillar3 = Instantiate(_roomMeshes.PillarMesh, cornerPosition3, Quaternion.identity);
        GameObject cornerPillar4 = Instantiate(_roomMeshes.PillarMesh, cornerPosition4, Quaternion.identity);

        cornerPillar1.transform.localScale = new Vector3(scaleX, 1, scaleY);
        cornerPillar2.transform.localScale = new Vector3(scaleX, 1, scaleY);
        cornerPillar3.transform.localScale = new Vector3(scaleX, 1, scaleY);
        cornerPillar4.transform.localScale = new Vector3(scaleX, 1, scaleY);

        cornerPillar1.transform.parent = transform;
        cornerPillar2.transform.parent = transform;
        cornerPillar3.transform.parent = transform;
        cornerPillar4.transform.parent = transform;

        Pillars.Add(cornerPillar1);
        Pillars.Add(cornerPillar2);
        Pillars.Add(cornerPillar3);
        Pillars.Add(cornerPillar4);
    }

    // Method to generate doors based on the number specified by the user
    public void GenerateDoorsRandomPosition()
    {
        ResetWalls();

        // Filter out walls that are not doors
        List<GameObject> wallPieces = Walls.FindAll(wall => wall.CompareTag("Wall"));

        // Ensure the number of doors does not exceed the number of available wall pieces
        int numberOfDoorsToGenerate = Mathf.Min(_numberOfDoors, wallPieces.Count);

        // Generate doors
        for (int i = 0; i < numberOfDoorsToGenerate; i++)
        {
            int randomIndex = Random.Range(0, wallPieces.Count);
            GameObject wallToReplace = wallPieces[randomIndex];
            Vector3 position = wallToReplace.transform.position;
            Quaternion rotation = wallToReplace.transform.rotation;
            Vector3 scale = wallToReplace.transform.localScale;

            // Instantiate door mesh and replace the wall mesh
            GameObject door = Instantiate(_roomMeshes.DoorMesh, position, rotation);
            door.transform.localScale = scale;
            door.transform.parent = transform;

            // Remove the wall from the list and destroy it
            Walls.Remove(wallToReplace);
            Destroy(wallToReplace);

            // Add the door to the list
            Walls.Add(door);

            // Remove the wall piece from the list of available wall pieces
            wallPieces.RemoveAt(randomIndex);
        }
    }

    private void ResetWalls()
    {
        // Create a separate list to hold the walls that need to be reset
        List<GameObject> wallsToReset = new List<GameObject>();

        // Find walls that are doors and add them to the list of walls to reset
        foreach (var wall in Walls)
        {
            if (wall.CompareTag("Door"))
            {
                wallsToReset.Add(wall);
            }
        }

        // Reset the walls
        foreach (var wall in wallsToReset)
        {
            // Instantiate a new wall mesh at the same position, rotation, and scale as the door
            GameObject newWall =
                Instantiate(_roomMeshes.WallMesh, wall.transform.position, wall.transform.rotation);
            newWall.transform.localScale = wall.transform.localScale;
            newWall.transform.parent = transform;

            // Remove the door and add the new wall to the list of walls
            Walls.Remove(wall);
            Destroy(wall);

            Walls.Add(newWall);
        }
    }

    #endregion
}