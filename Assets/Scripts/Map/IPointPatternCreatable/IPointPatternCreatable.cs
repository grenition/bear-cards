using System.Collections.Generic;

namespace Assets.Scripts.Map
{
    public interface IPointPatternCreatable
    {
        public List<PointOfInterestGenerator.InteresPointEntity> Create(
            ref List<PointOfInterestGenerator.InteresPointEntity> lastLevelPoint,
            List<InterestingPointConfig> pointsSet);
    }
}