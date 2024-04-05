using KBCore.Refs;
using System;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Explore.UI
{
    public class MainUI : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField, Anywhere] private Button _ButtonSave  { get; set; }
        [field: SerializeField, Anywhere] private Button _ButtonLoad  { get; set; }
        [field: SerializeField, Anywhere] private Button _ButtonReset { get; set; }
        [field: SerializeField, Anywhere] private Button _ButtonQuit  { get; set; }

        public MainUI Init()
        {
            if(_ButtonSave == null)
            {
                throw new ArgumentNullException(nameof(_ButtonSave));
            }
            if(_ButtonLoad == null)
            {
                throw new ArgumentNullException(nameof(_ButtonLoad));
            }
            if(_ButtonReset == null)
            {
                throw new ArgumentNullException(nameof(_ButtonReset));
            }
            if(_ButtonQuit == null)
            {
                throw new ArgumentNullException(nameof(_ButtonQuit));
            }
#if UNITY_EDITOR
            _ButtonQuit.onClick.AddListener(EditorApplication.ExitPlaymode);
#else
            _ButtonQuit.onClick.AddListener(Application.Quit);
#endif
            return this;
        }
        public MainUI SetActionSave(Action saveAction)
        {
            _ButtonSave.onClick.AddListener(() => saveAction());
            return this;
        }
        public MainUI SetActionLoad(Action loadAction)
        {
            _ButtonLoad.onClick.AddListener(() => loadAction());
            return this;
        }
        public MainUI SetActionReset(Action resetAction)
        {
            _ButtonReset.onClick.AddListener(() => resetAction());
            return this;
        }
    }
}
