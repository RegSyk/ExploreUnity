using System.Linq;

namespace Explore
{
    public abstract class ObjectSelectorOne<T> : ObjectSelector<T> where T : InteractableObject
    {
        protected override void OnSelectInternal()
        {
            if(_LastSelectedObject != _DetectedObject)
            {
                if(_LastSelectedObject)
                {
                    _SelectedObjectRegistry.Unregister(_LastSelectedObject);
                    InvokeOnObjectSelected(_LastSelectedObject, false);
                }
                if (_DetectedObject)
                {
                    int selectedElementsCount = _SelectedObjectRegistry.Elements.Count();
                    if (_DetectedObject.Selected)
                    {
                        if (selectedElementsCount == 1)
                        {
                            _SelectedObjectRegistry.Unregister(_DetectedObject);
                            InvokeOnObjectSelected(_DetectedObject, false);
                        }
                        else
                        {
                            _SelectedObjectRegistry.Clear();
                            _SelectedObjectRegistry.Register(_DetectedObject);
                            InvokeOnObjectSelected(_DetectedObject, true);
                        }
                    }
                    else
                    {
                        if (selectedElementsCount != 0)
                        {
                            _SelectedObjectRegistry.Clear();
                        }
                        _SelectedObjectRegistry.Register(_DetectedObject);
                        InvokeOnObjectSelected(_DetectedObject, true);
                    }
                }
                else
                {
                    _SelectedObjectRegistry.Clear();
                    InvokeOnObjectSelected(null);
                }
            }
            if(_DetectedObject == null)
            {
                _SelectedObjectRegistry.Clear();
            }
        }
    }
}
