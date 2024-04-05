using System;

namespace Explore
{
    public class TransitionData
    {
        public bool FromAnyState { get; private set; }
        public IState From { get; private set;  }
        public IState To { get; private set;  }
        public IPredicate Condition { get; private set;  }

        public static Builder New()
        {
            return new();
        }

        protected TransitionData(IState from, IState to, IPredicate condition, bool fromAnyState)
        {
            From = from;
            To = to;
            Condition = condition;
            FromAnyState = fromAnyState;
        }

        public class Builder
        {
            private TransitionData _data;

            public Builder CreateTransition()
            {
                if(_data != null)
                {
                    throw new InvalidOperationException("Transition data is already created!");
                }
                _data = new TransitionData(null, null, null, false);
                return this;
            }
            public Builder From(IState from)
            {
                if(_data.FromAnyState)
                {
                    throw new InvalidOperationException("This transition is from any state!");
                }
                _data.From = from;
                return this;
            }
            public Builder CreateTransitionFromAnyState()
            {
                if (_data != null)
                {
                    throw new InvalidOperationException("Transition data is already created!");
                }
                _data = new TransitionData(null, null, null, true);
                return this;
            }
            public Builder To(IState to)
            {
                _data.To = to;
                return this;
            }
            public Builder WithCondition(IPredicate condition)
            {
                _data.Condition = condition;
                return this;
            }

            public TransitionData Build()
            {
                if(_data.From == null && !_data.FromAnyState)
                {
                    throw new ArgumentNullException(nameof(_data.From));
                }
                if (_data.To == null)
                {
                    throw new ArgumentNullException(nameof(_data.To));
                }
                if(_data.Condition == null)
                {
                    throw new ArgumentNullException(nameof(_data.Condition));
                }
                return _data;
            }
        }
    }
}
