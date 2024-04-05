using Explore.Input;
using UnityEngine;

namespace Explore
{
    public abstract class ObjectInteractor<T> : MonoBehaviour where T : InteractableObject
    {
        [field: Header("State")]
        [field: SerializeField] public bool SelectMany { get; protected set; }
        [field: SerializeField] public bool CameraMovement { get; private set; }
        [field: SerializeField] public bool Rotator { get; private set; }
        [field: SerializeField] private ObjectSelector<T> _ObjectSelectorCurrent { get; set; }

        [field: Header("References")]
        [field: SerializeField] private MonoRegistry<T> _SelectedObjectRegistry { get; set; }
        [field: SerializeField] private MonoRegistry<T> _AllObjectsRegistry { get; set; }

        [field: SerializeField] private ObjectSelectorOne<T> _ObjectSelectorOne { get; set; }
        [field: SerializeField] private ObjectSelectorMany<T> _ObjectSelectorMany { get; set; }

        [field: SerializeField] private InputReaderBase _InputReader { get; set; }

        [field: SerializeField] private ObjectDetector<T> _ObjectDetector { get; set; }

        [field: SerializeField] private ObjectPinpointer<T> _ObjectPinpointer { get; set; }

        [field: SerializeField] private ObjectRuntimeRotator _RuntimeObjectRotator { get; set; }

        private StateMachine _StateMachine { get; set; }

        public ObjectInteractor<T> Init()
        {
            void InitReferences()
            {
                _ObjectDetector.SetAllObjectsRegistry(_AllObjectsRegistry);
                _ObjectPinpointer.SetObjectDetector(_ObjectDetector);
                _ObjectSelectorOne
                    .SetObjectDetector(_ObjectDetector)
                    .SetAllObjectsRegistry(_AllObjectsRegistry)
                    .SetSelectedObjectRegistry(_SelectedObjectRegistry);
                _ObjectSelectorMany
                    .SetObjectDetector(_ObjectDetector)
                    .SetAllObjectsRegistry(_AllObjectsRegistry)
                    .SetSelectedObjectRegistry(_SelectedObjectRegistry);
            }
            void InitStateMachine()
            {
                _StateMachine = new StateMachine();
                var stateHover = new StateHover()
                    .SetActionEnter(() =>
                    {
                        _ObjectSelectorCurrent = _ObjectSelectorOne;
                    })
                    .SetActionUpdate(() =>
                    {
                        _ObjectDetector.UpdateFrame();
                        _ObjectPinpointer.UpdateFrame();
                        HighlightObjects();
                    });
                var stateSelectMany = new StateSelectMany()
                    .SetActionEnter(() =>
                    {
                        _ObjectSelectorCurrent = _ObjectSelectorMany;
                    })
                    .SetActionUpdate(() =>
                    {
                        _ObjectDetector.UpdateFrame();
                        _ObjectPinpointer.UpdateFrame();
                        HighlightObjects();
                    });
                var stateSpectator = new StateSpectator();
                var stateRotator = new StateRotator()
                    .SetActionEnter(() =>
                    {
                        _RuntimeObjectRotator.SetEnabled(true);
                    })
                    .SetActionExit(() =>
                    {
                        _RuntimeObjectRotator.SetEnabled(false);
                    });

                _StateMachine.AddTransition(TransitionData.New()
                    .CreateTransition()
                    .From(stateHover)
                    .To(stateSelectMany)
                    .WithCondition(FuncPredicate.New(() => SelectMany))
                    .Build());
                _StateMachine.AddTransition(TransitionData.New()
                    .CreateTransition()
                    .From(stateSpectator)
                    .To(stateSelectMany)
                    .WithCondition(FuncPredicate.New(() => SelectMany && !CameraMovement))
                    .Build());
                _StateMachine.AddTransition(TransitionData.New()
                    .CreateTransition()
                    .From(stateRotator)
                    .To(stateSelectMany)
                    .WithCondition(FuncPredicate.New(() => SelectMany && !Rotator))
                    .Build());
                _StateMachine.AddTransition(TransitionData.New()
                    .CreateTransitionFromAnyState()
                    .To(stateHover)
                    .WithCondition(FuncPredicate.New(() => !(CameraMovement || SelectMany || Rotator)))
                    .Build());
                _StateMachine.AddTransition(TransitionData.New()
                    .CreateTransitionFromAnyState()
                    .To(stateSpectator)
                    .WithCondition(FuncPredicate.New(() => CameraMovement))
                    .Build());
                _StateMachine.AddTransition(TransitionData.New()
                    .CreateTransitionFromAnyState()
                    .To(stateRotator)
                    .WithCondition(FuncPredicate.New(() => Rotator))
                    .Build());

                _StateMachine.SetState(stateHover);
            }

            InitReferences();
            InitStateMachine();
           
            return this;
        }
        public ObjectInteractor<T> SetInputReader(InputReaderBase inputReader)
        {
            if (_InputReader == null)
            {
                _InputReader = inputReader;
            }
            return this;
        }
        public ObjectInteractor<T> SetSelectedObjectRegistry(MonoRegistry<T> registry)
        {
            if (_SelectedObjectRegistry == null)
            {
                _SelectedObjectRegistry = registry;
            }
            return this;
        }
        public ObjectInteractor<T> SetAllObjectsRegistry(MonoRegistry<T> registry)
        {
            if (_AllObjectsRegistry == null)
            {
                _AllObjectsRegistry = registry;
            }
            return this;
        }
        public ObjectInteractor<T> SetObjectDetector(ObjectDetector<T> objectDetector)
        {
            if (_ObjectDetector == null)
            {
                _ObjectDetector = objectDetector;
            }
            return this;
        }
        public ObjectInteractor<T> SetObjectPinpointer(ObjectPinpointer<T> objectPinpointer)
        {
            if (_ObjectPinpointer == null)
            {
                _ObjectPinpointer = objectPinpointer;
            }
            return this;
        }
        public ObjectInteractor<T> SetObjectSelectorOne(ObjectSelectorOne<T> objectSelector)
        {
            if (_ObjectSelectorOne == null)
            {
                _ObjectSelectorOne = objectSelector;
                _ObjectSelectorCurrent = _ObjectSelectorOne;
            }
            return this;
        }
        public ObjectInteractor<T> SetObjectSelectorMany(ObjectSelectorMany<T> objectSelector)
        {
            if (_ObjectSelectorMany == null)
            {
                _ObjectSelectorMany = objectSelector;
            }
            return this;
        }
        public ObjectInteractor<T> SetObjectRotator(ObjectRuntimeRotator objectRotator)
        {
            if (_RuntimeObjectRotator == null)
            {
                _RuntimeObjectRotator = objectRotator;
            }
            return this;
        }

        private void HighlightObjects()
        {
            foreach(var e in _AllObjectsRegistry.Elements)
            {
                if (e.Selected)
                {
                    if (!e.SelectedHighlighted)
                    {
                        e.HighlightSelected(e.Selected);
                    }
                }
                else if (e.Pinpointed)
                {
                    if (!e.PinpointedHighlighted)
                    {
                        e.HighlightPinpoint(e.Pinpointed);
                    }
                }
                else
                {
                    if (e.SelectedHighlighted != false)
                    {
                        e.HighlightSelected(false);
                    }
                    if (e.PinpointedHighlighted != false)
                    {
                        e.HighlightPinpoint(false);
                    }
                }
            }
        }

        private void OnMoveCamera(bool startMoving)
        {
            CameraMovement = startMoving;
        }
        private void OnSelectMany(bool selectionStarted)
        {
            SelectMany = selectionStarted;
        }
        private void OnSelect()
        {
            if (CameraMovement || Rotator)
            {
                return;
            }
            _ObjectSelectorCurrent.OnSelect();
        }
        private void OnRotate(bool rotationStarted)
        {
            Rotator = rotationStarted;
        }

        private void Update()
        {
            _StateMachine.Update();
        }
        private void FixedUpdate()
        {
            _StateMachine.FixedUpdate();
        }
        private void OnEnable()
        {
            _InputReader.OnSelectManyEvent += OnSelectMany;
            _InputReader.OnSelectEvent += OnSelect;
            _InputReader.OnMoveCameraEvent += OnMoveCamera;
            _InputReader.OnRotateEvent += OnRotate;
        }
        private void OnDisable()
        {
            _InputReader.OnSelectManyEvent -= OnSelectMany;
            _InputReader.OnSelectEvent -= OnSelect;
            _InputReader.OnMoveCameraEvent -= OnMoveCamera;
            _InputReader.OnRotateEvent -= OnRotate;
        }
    }
}
