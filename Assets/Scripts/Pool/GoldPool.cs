using Player.Loot;

namespace Pool
{
    public class GoldPool : BasePool<Gold>
    {
        public static GoldPool Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            InitPool();
        }
    }
}