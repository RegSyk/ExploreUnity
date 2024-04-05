using System;
namespace Explore
{
    public abstract class StateBase : IState
    {
        private Action _ActionUpdate;
        private Action _ActionFixedUpdate;
        private Action _ActionEnter;
        private Action _ActionExit;

        public void OnEnter() => _ActionEnter?.Invoke();
        public void OnUpdate() => _ActionUpdate?.Invoke();
        public void OnFixedUpdate() => _ActionFixedUpdate?.Invoke();
        public void OnExit() => _ActionExit?.Invoke();

        public StateBase SetActionUpdate(Action updateAction)
        {
            _ActionUpdate += updateAction;
            return this;
        }
        public StateBase SetActionFixedUpdate(Action fixedUpdateAction)
        {
            _ActionFixedUpdate += fixedUpdateAction;
            return this;
        }
        public StateBase SetActionExit(Action exitAction)
        {
            _ActionExit += exitAction;
            return this;
        }
        public StateBase SetActionEnter(Action enterAction)
        {
            _ActionEnter += enterAction;
            return this;
        }
    }
}
