using Assets.Scripts.Map;
using System.Collections.Generic;
using static Assets.Scripts.Map.PointOfInterestGenerator;

public class MapController
{
    private List<List<InteractivePoint>> _pointCollections;
    public void Create(List<List<InteractivePoint>> pointCollection)
    {
        _pointCollections = pointCollection;

        pointCollection[0][0].Pass();
    }
}
