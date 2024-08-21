using System.Collections;
using System.IO;
using LitJson;
using UnityEngine;

namespace System
{
    public class Setting
    {
        public static Setting Instance => _instance ??= new Setting();

        public delegate void SettingChanged(JsonData settingJson);

        public static event SettingChanged OnSettingChanged;

        private static Setting _instance;
        private JsonData _settingObj;
        private string _settingPath;

        private Setting()
        {
            _settingObj = GetSettingLocal();
        }

        /// <summary>
        /// 以json格式保存配置
        /// </summary>
        /// <param name="obj"></param>
        public void SaveJson(JsonData obj)
        {
            _settingObj = obj;
            var jsonContent = JsonMapper.ToJson(obj);
            File.WriteAllText(_settingPath, jsonContent);

            OnSettingChanged?.Invoke(_settingObj);
        }

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
        /// 获取音效
        /// </summary>
        /// <returns></returns>
        public int GetMusic()
        {
            if (!HasProp(SettingProp.Music))
            {
                return 100;
            }

            var music = _settingObj[SettingProp.Music.ToString()];
            return music.IsInt ? (int)music : 100;
        }

        /// <summary>
        /// 判断序列化数据里是否保存了音量设置
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        private bool HasProp(SettingProp propName)
        {
            return ((IDictionary)_settingObj).Contains(propName.ToString());
        }

        /// <summary>
        /// 从本地获取配置文件
        /// </summary>
        /// <returns></returns>
        private JsonData GetSettingLocal()
        {
            _settingPath = Application.persistentDataPath + "/setting.json";
            if (!File.Exists(_settingPath))
            {
                File.Create(_settingPath).Dispose();
            }

            var jsonContent = File.ReadAllText(_settingPath);
            return JsonMapper.ToObject(jsonContent);
        }
    }

    public enum SettingProp
    {
        Volume,
        Music
    }
}