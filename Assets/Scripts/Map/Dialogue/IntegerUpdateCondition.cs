using System;

namespace Project
{
    public class IntegerUpdateCondition : DialogueCondition
    {
        private Func<int, int, bool> _condition;
        private string _currentValue;
        private int _targetValue;

        private LocationVariabelsData _locationVariabelsData;
        public IntegerUpdateCondition(string currentValue, LocationVariabelsData locationVariabelsData)
        {
            _currentValue = currentValue;
            _locationVariabelsData = locationVariabelsData;
            _targetValue = (int)_locationVariabelsData.GetType().GetField(_currentValue).GetValue(_locationVariabelsData) + 1;
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
            return _condition((int)_locationVariabelsData.GetType().GetField(_currentValue).GetValue(_locationVariabelsData), _targetValue);
        }
    }
}
