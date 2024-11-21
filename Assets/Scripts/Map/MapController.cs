using System.Collections.Generic;
using static Assets.Scripts.Map.PointOfInterestGenerator;

public class MapController
{
    private List<List<InteresPointEntity>> _pointCollections;
    public void Create(List<List<InteresPointEntity>> pointCollection)
    {
        _pointCollections = pointCollection;

        pointCollection[0][0].InteractivePoint.Pass();
    }
}
