using Enemy;

namespace Pool
{
    public class EnemyPool : BasePool<BaseEnemy>
    {
        private void Awake()
        {
            InitPool();
        }
    }
}