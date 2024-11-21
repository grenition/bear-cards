using Assets.Scripts.Map;
using System.Collections.Generic;
using UnityEngine;

public class MapController: MonoBehaviour
{
    private List<List<InteractivePoint>> _pointCollections;
    public void Create(List<List<InteractivePoint>> pointCollection)
    {
        _pointCollections = pointCollection;

        pointCollection[0][0].Pass();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && TryGetComponent(out InteractivePoint interactivePoint))
            {
                Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
            }
        }
    }
}
