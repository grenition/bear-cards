using System;

namespace Project
{
    public class BooleanUpdateCondition : DialogueCondition
    {
        private Func<bool, bool, bool> _condition;
        private string _currentValue;
        private bool _targetValue;

        private LocationVariabelsData _locationVariabelsData;
        public BooleanUpdateCondition(string first)
        {
            _currentValue = first;
            _targetValue = true;
            _condition = (a, b) => { return a == b; };
        }

        public override void Update(LocationVariabelsData locationVariabelsData)
        {
            _locationVariabelsData = locationVariabelsData;
        }

        public void UpdateTarget(bool target)
        {
            _targetValue = target;
        }

        public override bool GetResult()
        {
            var fieldValue = _locationVariabelsData.GetType().GetField(_currentValue).GetValue(_locationVariabelsData);
            return _condition((bool)fieldValue, _targetValue);
        }
    }
}
