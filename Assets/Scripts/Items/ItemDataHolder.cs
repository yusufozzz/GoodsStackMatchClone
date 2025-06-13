using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utility;

namespace Items
{
    [CreateAssetMenu(fileName = "ItemDataHolder", menuName = "Items/Item Data Holder", order = 1)]
    public class ItemDataHolder: ScriptableSingletonMono<ItemDataHolder>
    {
        public ItemData[] ItemData => _loadedItemData;
        private ItemData[] _loadedItemData;
        
        public async Task LoadAllItemDataAsync()
        {
            var handle = Addressables.LoadAssetsAsync<ItemData>("ItemData", null);
            try
            {
                await handle.Task;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _loadedItemData = handle.Result.ToArray();
                }
            }
            finally
            {
                Addressables.Release(handle);
            }
        }
    }
}