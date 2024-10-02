using System;
using ScriptObj;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class SelectUI : MonoBehaviour
    {
        [SerializeField]
        private Bag bag;
        [SerializeField]
        private GameObject player;
        [SerializeField]
        private ShopProduct shopProduct;
        [SerializeField]
        private AudioClip confirmAudio;

        private int _characterIndex;
        private int _weaponIndex;
        private float _volume;
        private VisualElement _rootEl;
        private GameObject _weapon;
        private Vector3 _cameraDefaultPos;

        private void Awake()
        {
            _rootEl = GetComponent<UIDocument>().rootVisualElement;
            _weapon = player.transform.Find("Weapon").gameObject;
            _volume = Setting.Instance.GetVolume() / 100f;
            _cameraDefaultPos = new Vector3(0, 0, -10);
        }

        private void Start()
        {
            bag.Init();
            LoadCharacter();
            LoadWeapon();

            // 注册点击Start后的回调方法
            _rootEl.Q<Button>("GoButton").clicked += GoToFight;
        }

        private void LoadCharacter()
        {
            var characterLisUI = _rootEl.Q("CharacterList");
            characterLisUI.Clear();

            var playerAnimator = player.GetComponent<Animator>();
            var charItemVta = Resources.Load<VisualTreeAsset>("CharacterItem");
            for (var i = 0; i < shopProduct.characterList.Count; i++)
            {
                var index = i;
                var item = shopProduct.characterList[i];
                var characterUI = charItemVta.Instantiate();
                CustomCharacterStyle(characterUI, item);
                characterLisUI.Add(characterUI);

                // 注册当鼠标移到该元素时的回调方法
                characterUI.RegisterCallback<MouseUpEvent>(evt =>
                {
                    _characterIndex = index;
                    // 将当前Player动画替换为选中的Character的动画
                    playerAnimator.runtimeAnimatorController = item.characterAnimator;
                    AudioManager.Instance.Play(confirmAudio, _cameraDefaultPos);
                }, TrickleDown.TrickleDown);

                // 注册点击时的背景色变换回调方法
                SetItemEvent(characterUI, "item");
            }
        }

        private void LoadWeapon()
        {
            var weaponListUI = _rootEl.Q("WeaponList");
            weaponListUI.Clear();

            var weaponSprite = _weapon.GetComponent<SpriteRenderer>();
            var weaponItemVta = Resources.Load<VisualTreeAsset>("CharacterItem");
            for (var i = 0; i < shopProduct.weaponList.Count; i++)
            {
                var index = i;
                var item = shopProduct.weaponList[i];
                var weaponUI = weaponItemVta.Instantiate();
                CustomWeaponStyle(weaponUI, item);
                weaponListUI.Add(weaponUI);

                // 注册鼠标点击回调方法
                weaponUI.RegisterCallback<MouseUpEvent>(evt =>
                {
                    _weaponIndex = index;
                    // 在武器列表中点击后将玩家的武器替换为选中的
                    weaponSprite.sprite = item.weaponImg;
                    AudioManager.Instance.Play(confirmAudio, _cameraDefaultPos);
                }, TrickleDown.TrickleDown);

                SetItemEvent(weaponUI, "item");
            }
        }

        private void GoToFight()
        {
            AudioManager.Instance.Play(confirmAudio, _cameraDefaultPos);
            bag.character = shopProduct.characterList[_characterIndex];
            bag.weaponList.Add(shopProduct.weaponList[_weaponIndex]);
            SceneManager.LoadScene("Fight");
        }

        private void CustomCharacterStyle(TemplateContainer itemUI, Character item)
        {
            // UI中CharacterItem元素的背景图片设置为预设的
            itemUI.Q("img").style.backgroundImage = new StyleBackground(item.characterImg);
            // 获取存放图片的元素并设置缩放
            var itemElement = itemUI.Q("item");
            itemElement.style.paddingLeft = 20;
            itemElement.style.paddingRight = 20;
            itemElement.style.paddingTop = 20;
            itemElement.style.paddingBottom = 20;
        }

        private void CustomWeaponStyle(TemplateContainer itemUI, Weapon item)
        {
            // UI中WeaponItem元素的背景图片设置为预设的
            itemUI.Q("img").style.backgroundImage = new StyleBackground(item.weaponImg);
            // 获取存放图片的元素并设置缩放
            var itemElement = itemUI.Q("item");
            itemElement.style.paddingLeft = 40;
            itemElement.style.paddingRight = 40;
            itemElement.style.paddingTop = 40;
            itemElement.style.paddingBottom = 40;
        }

        private void SetItemEvent(TemplateContainer tc, string eleName)
        {
            tc.RegisterCallback<MouseOverEvent>(
                evt => { tc.Q(eleName).style.backgroundColor = new Color(255, 255, 255, .4f); },
                TrickleDown.TrickleDown);
            tc.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                tc.Q(eleName).style.backgroundColor = new Color(0, 0, 0, .5f);
            });
        }
    }
}