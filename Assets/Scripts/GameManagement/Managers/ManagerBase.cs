using UnityEngine;

namespace GameManagement.Managers
{
    public abstract class ManagerBase: MonoBehaviour
    {
        public virtual void SetUp()
        {
        }
        
        protected virtual void OnEnable()
        {
            
        }
        
        protected virtual void Start()
        {
            
        }
        
        protected virtual void OnDisable()
        {
            
        }
    }
}