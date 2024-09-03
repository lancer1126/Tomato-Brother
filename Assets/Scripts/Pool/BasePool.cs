using UnityEngine;
using UnityEngine.Pool;

namespace Pool
{
    public class BasePool<T> : MonoBehaviour where T : Component
    {
        public T prefab;
        private ObjectPool<T> _pool;

        public void SetPrefab(T obj) => prefab = obj;

        protected void InitPool(int defaultSize = 100, int maxSize = 500, bool onCheck = true)
        {
            _pool = new ObjectPool<T>(ToCreate, ToGet, ToRelease, ToDestroy, onCheck, defaultSize, maxSize);
        }

        protected virtual T ToCreate() => Instantiate(prefab, transform);
        protected virtual void ToGet(T obj) => obj.gameObject.SetActive(true);
        protected virtual void ToRelease(T obj) => obj.gameObject.SetActive(false);
        protected virtual void ToDestroy(T obj) => Destroy(obj.gameObject);
    }
}