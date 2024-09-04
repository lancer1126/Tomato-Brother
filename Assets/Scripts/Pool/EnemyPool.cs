using Enemy;

namespace Pool
{
    public class EnemyPool : BasePool<BaseEnemy>
    {
        private void Awake()
        {
            InitPool();
        }

        protected override BaseEnemy ToCreate()
        {
            var ins = Instantiate(prefab, transform);
            ins.SetDeactivateAction(delegate { ReleaseFromPool(ins); });
            ins.SetActiveAction(GetFromPool);

            return ins;
        }
    }
}