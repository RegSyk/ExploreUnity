using System;
namespace Explore
{
    public class FuncPredicate : IPredicate
    {
        private readonly Func<bool> _func;

        public FuncPredicate(Func<bool> func) => this._func = func;
        public static FuncPredicate New(Func<bool> func) { return new FuncPredicate(func); }
        public bool Evaluate() => _func.Invoke();
    }
}
