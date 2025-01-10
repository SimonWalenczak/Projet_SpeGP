using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Procedural
{
    public class DoorGenerator : MonoBehaviour
    {
        [HideInInspector] public int NumberOfDoors;
        
        public void GenerateDoorsRandomPosition()
        {

            List<GameObject> wallPieces = RoomEditorManager.Instance.WallPieces.FindAll(wall => wall.CompareTag("Wall"));

            int numberOfDoorsToGenerate = Mathf.Min(NumberOfDoors, wallPieces.Count);

            for (int i = 0; i < numberOfDoorsToGenerate; i++)
            {
                int randomIndex = Random.Range(0, wallPieces.Count);
                GameObject wallToReplace = wallPieces[randomIndex];
                Vector3 position = wallToReplace.transform.position;
                Quaternion rotation = wallToReplace.transform.rotation;
                Vector3 scale = wallToReplace.transform.localScale;

                GameObject door = Instantiate(RoomEditorManager.Instance.RoomMeshes.DoorMesh, position, rotation);
                door.transform.localScale = scale;
                door.transform.parent = transform;
                door.GetComponent<WallPiece>().PieceType = PieceType.Door;
                
                RoomEditorManager.Instance.WallPieces.Remove(wallToReplace);
                Destroy(wallToReplace);

                RoomEditorManager.Instance.WallPieces.Add(door);

                wallPieces.RemoveAt(randomIndex);
            }
        }
    }
}
