using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomExpand : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] private Vector2 _roomSize;
    [SerializeField] private float _valueRegeneration;
    private float _previousRoomSizeX;
    private float _previousRoomSizeY;

    [Space(10)] [Header("Walls Meshes")] [SerializeField]
    private GameObject _wallMesh;

    [SerializeField] private GameObject _doorMesh;
    [SerializeField] private GameObject _pillarMesh;

    [SerializeField] private int _numberOfDoors;

    [Space(10)] public List<GameObject> Walls = new List<GameObject>();
    public List<GameObject> Pillars = new List<GameObject>();

    //Hidden
    private int _wallCount;
    private Vector2 _wallSize;

    private GameObject _currentMesh;

    #endregion

    #region Methods

    private void Start()
    {
        _wallSize.x = _wallMesh.GetComponent<MeshRenderer>().bounds.size.x;
        _wallSize.y = _wallMesh.GetComponent<MeshRenderer>().bounds.size.x;

        _roomSize.x = _wallSize.x;
        _roomSize.y = _wallSize.y;

        _previousRoomSizeX = _roomSize.x;

        _currentMesh = _wallMesh;

        CreateWalls();
    }

    private void Update()
    {
        //Limit to minimum
        if (_roomSize.x <= _wallSize.x)
            _roomSize.x = _wallSize.x;
        if (_roomSize.y <= _wallSize.y)
            _roomSize.y = _wallSize.y;

        //Room X
        if (_roomSize.x > _previousRoomSizeX + _valueRegeneration)
        {
            _previousRoomSizeX = _roomSize.x;
            CreateWalls();
        }
        else if (_roomSize.x < _previousRoomSizeX - _valueRegeneration)
        {
            _previousRoomSizeX = _roomSize.x;
            CreateWalls();
        }

        //Room Y
        if (_roomSize.y > _previousRoomSizeY + _valueRegeneration)
        {
            _previousRoomSizeY = _roomSize.y;
            CreateWalls();
        }
        else if (_roomSize.y < _previousRoomSizeY - _valueRegeneration)
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

            var rand = Random.Range(0, 2);
            if (rand < 1)
            {
                currentWallX = Instantiate(_wallMesh, positionX, rotationX);
            }
            else if (rand < 2)
            {
                currentWallX = Instantiate(_doorMesh, positionX, rotationX);
            }

            currentWallX.transform.localScale = localScaleX;
            currentWallX.transform.parent = transform;
            Walls.Add(currentWallX);

            // Add pillar between walls
            if (i < wallCountX - 1)
            {
                Vector3 pillarPosition = positionX + new Vector3(scaleX * _wallSize.x / 2, 0, 0);
                GameObject pillar = Instantiate(_pillarMesh, pillarPosition, Quaternion.identity);
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

            var rand = Random.Range(0, 2);
            if (rand < 1)
            {
                currentWallY = Instantiate(_wallMesh, positionY, rotationY);
            }
            else if (rand < 2)
            {
                currentWallY = Instantiate(_doorMesh, positionY, rotationY);
            }

            currentWallY.transform.localScale = localScaleY;
            currentWallY.transform.parent = transform;
            Walls.Add(currentWallY);

            // Add pillar between walls
            if (j < wallCountX - 1)
            {
                Vector3 pillarPosition = positionY + new Vector3(scaleX * _wallSize.x / 2, 0, 0);
                GameObject pillar = Instantiate(_pillarMesh, pillarPosition, Quaternion.identity);
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

            var rand = Random.Range(0, 2);
            if (rand < 1)
            {
                currentWallLeft = Instantiate(_wallMesh, positionLeft, rotationLeft);
            }
            else if (rand < 2)
            {
                currentWallLeft = Instantiate(_doorMesh, positionLeft, rotationLeft);
            }

            currentWallLeft.transform.localScale = localScaleLeft;
            currentWallLeft.transform.parent = transform;
            Walls.Add(currentWallLeft);

            // Add pillar between walls
            if (k < wallCountY - 1)
            {
                Vector3 pillarPosition = positionLeft + new Vector3(0, 0, scaleY * _wallSize.y / 2);
                GameObject pillar = Instantiate(_pillarMesh, pillarPosition, Quaternion.identity);
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

            var rand = Random.Range(0, 2);
            if (rand < 1)
            {
                currentWallRight = Instantiate(_wallMesh, positionRight, rotationRight);
            }
            else if (rand < 2)
            {
                currentWallRight = Instantiate(_doorMesh, positionRight, rotationRight);
            }

            currentWallRight.transform.localScale = localScaleRight;
            currentWallRight.transform.parent = transform;
            Walls.Add(currentWallRight);

            // Add pillar between walls
            if (l < wallCountY - 1)
            {
                Vector3 pillarPosition = positionRight + new Vector3(0, 0, scaleY * _wallSize.y / 2);
                GameObject pillar = Instantiate(_pillarMesh, pillarPosition, Quaternion.identity);
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

        GameObject cornerPillar1 = Instantiate(_pillarMesh, cornerPosition1, Quaternion.identity);
        GameObject cornerPillar2 = Instantiate(_pillarMesh, cornerPosition2, Quaternion.identity);
        GameObject cornerPillar3 = Instantiate(_pillarMesh, cornerPosition3, Quaternion.identity);
        GameObject cornerPillar4 = Instantiate(_pillarMesh, cornerPosition4, Quaternion.identity);

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
    public void GenerateDoors()
    {
        for (int i = 0; i < _numberOfDoors; i++)
        {
            int randomWallIndex = Random.Range(0, Walls.Count);
            GameObject wallToReplace = Walls[randomWallIndex];
            Vector3 position = wallToReplace.transform.position;
            Quaternion rotation = wallToReplace.transform.rotation;
            Vector3 scale = wallToReplace.transform.localScale;

            // Instantiate door mesh and replace the wall mesh
            GameObject door = Instantiate(_doorMesh, position, rotation);
            door.transform.localScale = scale;
            door.transform.parent = transform;

            // Remove the wall from the list and destroy it
            Walls.RemoveAt(randomWallIndex);
            Destroy(wallToReplace);

            // Add the door to the list
            Walls.Add(door);
        }
    }

    #endregion
}