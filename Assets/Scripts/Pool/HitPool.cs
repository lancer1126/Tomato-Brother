using Effect;

namespace Pool
{
    public class HitPool : BasePool<Hit>
    {
        public static HitPool Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            InitPool();
        }
        
        protected override Hit ToCreate()
        {
            var ins = Instantiate(prefab, transform);
            ins.SetDeactivateAction(delegate { ReleaseFromPool(ins); });
            return ins;
        }
    }
}