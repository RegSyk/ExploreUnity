using Cinemachine;
using KBCore.Refs;
using System.Collections.Generic;
using Explore.Persistence;
using UnityEngine;
using Explore.UI;
using Explore.Input;

namespace Explore
{
    [DefaultExecutionOrder(-9999)]
    public class ApplicationBootstrapper : MonoBehaviour
    {
        [field: Header("Camera")]
        [field: SerializeField, Scene] private SpectatorCameraController _CameraController { get; set; }
        [field: SerializeField, Scene] private CinemachineVirtualCamera _CinemachineFreeLook { get; set; }
        [field: Space]

        [field: Header("Object interaction")]
        [field: SerializeField, Scene] private ObjectInteractorTeeth _TeethInteractor { get; set; }
        [field: SerializeField, Scene] private ObjectSelectorOneTooth _ObjectSelectorOne { get; set; }
        [field: SerializeField, Scene] private ObjectSelectorManyTeeth _ObjectSelectorMany { get; set; }
        [field: SerializeField, Scene] private ObjectPinpointerTeeth _ObjectPinpointer { get; set; }
        [field: SerializeField, Scene] private ObjectDetectorTeeth _ObjectDetector { get; set; }

        [field: SerializeField, Scene] private SelectableTeethRegistry _AllSelectableTeethRegistry { get; set; }
        [field: SerializeField, Scene] private SelectedTeethRegistry _SelectedTeethRegistry { get; set; }
        [field: Space]

        [field: Header("Interactable objects")]
        [field: SerializeField, Anywhere] private GameObject _MouthObject { get; set; }
        [field: SerializeField] private Transform _MouthTransform { get; set; }
        [field: SerializeField] private List<Transform> _TeethTransforms { get; set; }
        [field: SerializeField] private InteractableObjectMouth _InteractableObjectMouth { get; set; }
        [field: SerializeField] private ObjectRuntimeRotator _RuntimeObjectRotator { get; set; }
        [field: Space]

        [field: Header("Input")]
        [field: SerializeField, Scene] private InputReaderBase _InputReader { get; set; }
        [field: Space]

        [field: Header("UI")]
        [field: SerializeField, Scene] private MainUI _MainUI { get; set; }
        [field: Space]

        private IDataService _DataService { get; set; }

        private const string k_TeethNodesName = "teeth.";
        private const string k_AppDataName = "AppData";

        private void Awake() => Init();

        private void Init()
        {
            //Configure input
            _InputReader
                .Init()
                .SetEnabled(true);

            //Configure interactable objects
            _InteractableObjectMouth = _MouthObject
                .AddComponent<InteractableObjectMouth>()
                .Init()
                .SetEnabledDragAndDrop(false) as InteractableObjectMouth;

            _MouthTransform = _MouthObject.transform;
            _TeethTransforms = _MouthTransform.FindChildrenByName(k_TeethNodesName);
            _TeethTransforms.ForEach(t => t.gameObject
                .AddComponent<InteractableObjectTooth>()
                .Init()
            );

            _RuntimeObjectRotator = _MouthObject
                .AddComponent<ObjectRuntimeRotator>()
                .SetTarget(_InteractableObjectMouth)
                .Init()
                .SetEnabled(false);

            _TeethInteractor
                .SetInputReader(_InputReader)
                .SetSelectedObjectRegistry(_SelectedTeethRegistry)
                .SetAllObjectsRegistry(_AllSelectableTeethRegistry)
                .SetObjectDetector(_ObjectDetector)
                .SetObjectPinpointer(_ObjectPinpointer)
                .SetObjectSelectorOne(_ObjectSelectorOne)
                .SetObjectSelectorMany(_ObjectSelectorMany)
                .SetObjectRotator(_RuntimeObjectRotator)
                .Init()
                ;

            //Configure camera
            _CameraController
                .SetCamera(_CinemachineFreeLook)
                .SetInputReader(_InputReader);

            //Configure data
            _DataService = new FileDataService(new JsonAppDataSerializer());

            //Configure UI
            _MainUI
                .SetActionReset(() =>
                {
                    foreach (var e in _AllSelectableTeethRegistry.Elements)
                    {
                        e.RestorePosition();
                    }
                })
                .SetActionSave(() =>
                {
                    _DataService.Save(new AppData()
                    {
                        Name = k_AppDataName,
                        RootTransformData = _InteractableObjectMouth.TransformData
                    });
                })
                .SetActionLoad(() =>
                {
                    AppData data = _DataService.Load(k_AppDataName);
                    _InteractableObjectMouth.SetTransformData(data.RootTransformData);
                })
                .Init()
                ;
        }
    }
}
