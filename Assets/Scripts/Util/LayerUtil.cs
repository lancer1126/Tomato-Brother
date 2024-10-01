using UnityEngine;

namespace Util
{
    public static class LayerUtil
    {
        private static int? _enemyLayer;

        public static int GetEnemy()
        {
            _enemyLayer ??= LayerMask.GetMask("Enemy");
            return (int)_enemyLayer;
        }
    }
}