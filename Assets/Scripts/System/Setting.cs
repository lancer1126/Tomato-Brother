using System.Collections;
using LitJson;

namespace System
{
    public class Setting
    {
        public static Setting Instance => _instance ??= new Setting();

        public delegate void SettingChanged(JsonData settingJson);

        public static event SettingChanged OnSettingChanged;

        private static Setting _instance;
        private JsonData _settingObj;

        /// <summary>
        /// 获取音量
        /// </summary>
        /// <returns></returns>
        public int GetVolume()
        {
            if (!HasProp(SettingProp.Volume))
            {
                return 100;
            }

            var volume = _settingObj[SettingProp.Volume.ToString()];
            return volume.IsInt ? (int)volume : 100;
        }

        /// <summary>
        /// 判断序列化数据里是否保存了音量设置
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        private bool HasProp(SettingProp propName)
        {
            return ((IDictionary)_settingObj).Contains(propName);
        }
    }

    public enum SettingProp
    {
        Volume
    }
}