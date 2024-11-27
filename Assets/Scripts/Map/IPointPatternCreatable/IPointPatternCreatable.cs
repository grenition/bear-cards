using System.Collections.Generic;

namespace Assets.Scripts.Map
{
    public interface IPointPatternCreatable
    {
        public List<InteractivePoint> Create(
            ref List<InteractivePoint> lastLevelPoint,
            List<string> pointsSet);
    }
}