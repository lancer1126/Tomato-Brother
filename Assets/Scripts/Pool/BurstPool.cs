using Effect;

namespace Pool
{
    public class BurstPool : BasePool<Burst>
    {
        public static BurstPool Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            InitPool();
        }
        
        protected override Burst ToCreate()
        {
            var ins = Instantiate(prefab, transform);
            ins.SetDeactivateAction(delegate { ReleaseFromPool(ins); });
            return ins;
        }
    }
}