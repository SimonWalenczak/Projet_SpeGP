using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public List<LeafletMenu> LeafletMenus;

        public void OnClickDoorGeneration()
        {
            RoomEditorManager.Instance.GenerateDoors();
        }

        public void ResetAllLeafletMenus()
        {
            foreach (var leafletMenu in LeafletMenus)
            {
                if (leafletMenu.isOpen)
                    leafletMenu.ToggleMenu();
            }
        }
    }
}