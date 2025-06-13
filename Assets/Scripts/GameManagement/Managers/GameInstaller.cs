using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Items;
using UnityEngine;

namespace GameManagement.Managers
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField]
        private List<ManagerBase> managerList;

        private Dictionary<Type, ManagerBase> _managerMap;

        private async void Awake()
        {
            SetTargetFrame();
            await SetUpManagersAsync();
        }

        // Sets the target frame rate for the application.
        private static void SetTargetFrame()
        {
            Application.targetFrameRate = 60;
        }

        // Initializes all managers in the game.
        private async Task SetUpManagersAsync()
        {
            await LoadAddressable();

            _managerMap = new Dictionary<Type, ManagerBase>();

            foreach (var manager in managerList)
            {
                if (manager == null) continue;
                var type = manager.GetType();
                if (!_managerMap.ContainsKey(type))
                {
                    _managerMap[type] = manager;
                }
                else
                {
                    Debug.LogWarning($"Duplicate manager type: {type.Name}");
                }
            }

            ManagerAccess.SetUp(_managerMap);

            foreach (var manager in _managerMap.Values)
            {
                manager.SetUp();
            }
        }
        
        // Loads addressable assets required for the game.
        private static async Task LoadAddressable()
        {
            await GameSettings.LoadAsync();
            await ItemDataHolder.LoadAsync();
            await ItemDataHolder.Instance.LoadAllItemDataAsync();
        }
    }
}