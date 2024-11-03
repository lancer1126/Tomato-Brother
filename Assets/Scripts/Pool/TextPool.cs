using Effect;
using TMPro;
using UI;
using UnityEngine;

namespace Pool
{
    public class TextPool : BasePool<HurtText>
    {
        public static TextPool Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            InitPool();
        }

        protected override HurtText ToCreate()
        {
            var ins = Instantiate(prefab, transform);
            ins.SetDeactivateAction(delegate { ReleaseFromPool(ins); });
            return ins;
        }

        public HurtText GetText(Vector3 pos, string str)
        {
            return GetText(pos, str, Color.red);
        }

        public HurtText GetText(Vector3 pos, string str, float fontSize)
        {
            return GetText(pos, str, Color.red, fontSize);
        }

        public HurtText GetText(Vector3 pos, string str, Color color, float fontSize = 2f)
        {
            var textObj = GetFromPool();
            textObj.transform.position = GetRandomPos(pos);
            var tmp = textObj.GetComponent<TextMeshPro>();
            tmp.text = str;
            tmp.color = color;
            tmp.fontSize = fontSize;
            tmp.fontStyle = FontStyles.Bold;
            return textObj;
        }

        private static Vector3 GetRandomPos(Vector3 pos)
        {
            var x = Random.Range(-0.5f, 0.5f);
            var y = Random.Range(0, 1f);
            return pos + new Vector3(x, y, 0);
        }
    }
}