using System;

namespace Project
{
    public class ConditionEquales : DialogueCondition
    {
        private Func<int, int, bool> _condition;
        private string _currentValue;
        private int _targetValue;

        private LocationVariabelsData _locationVariabelsData;
        public ConditionEquales(string first, int target)
        {
            _currentValue = first;
            _targetValue = target;
            _condition = (a, b) => { return a == b; };
        }

        public override void Update(LocationVariabelsData locationVariabelsData)
        {
            _locationVariabelsData = locationVariabelsData;
        }

        public void UpdateTarget(int target)
        {
            _targetValue = target;
        }

        public override bool GetResult()
        {
            var value = (int)_locationVariabelsData.GetType().GetField(_currentValue).GetValue(_locationVariabelsData);
            return _condition(value, _targetValue);
        }
    }
}
