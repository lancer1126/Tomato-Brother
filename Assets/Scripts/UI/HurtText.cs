using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class HurtText : MonoBehaviour
    {
        private Action _releaseAction;

        private void OnEnable()
        {
            StartCoroutine(HideText());
        }

        public void SetActiveAction(Action releaseObj)
        {
            _releaseAction = releaseObj;
        }

        private IEnumerator HideText()
        {
            transform.DOBlendableMoveBy(new Vector2(0, 1f), 0.25f);
            yield return new WaitForSeconds(0.25f);
            transform.DOBlendableMoveBy(new Vector2(0, -0.5f), 0.25f);
            yield return new WaitForSeconds(0.25f);
            
            _releaseAction.Invoke();
        }
    }
}