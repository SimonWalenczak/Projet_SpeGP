using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Procedural
{
    public class RoomModifier : MonoBehaviour
    {
        public void ResetWalls()
        {
            List<GameObject> wallsToReset = new List<GameObject>();

            foreach (var wall in RoomEditorManager.Instance.WallPieces)
            {
                if (wall.CompareTag("Door"))
                {
                    wallsToReset.Add(wall);
                }
            }

            foreach (var wall in wallsToReset)
            {
                GameObject newWall = Instantiate(RoomEditorManager.Instance.RoomMeshes.WallMesh, wall.transform.position, wall.transform.rotation);
                newWall.transform.localScale = wall.transform.localScale;
                newWall.transform.parent = transform;

                RoomEditorManager.Instance.WallPieces.Remove(wall);
                Destroy(wall);

                RoomEditorManager.Instance.WallPieces.Add(newWall);
            }
        }
    }
}