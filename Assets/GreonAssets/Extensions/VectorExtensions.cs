using UnityEngine;

namespace GreonAssets.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 Clamp(this Vector3 vector, Vector3 minVector, Vector3 maxVector)
        {
            vector.x = Mathf.Clamp(vector.x, minVector.x, maxVector.x);
            vector.y = Mathf.Clamp(vector.y, minVector.y, maxVector.y);
            vector.z = Mathf.Clamp(vector.z, minVector.z, maxVector.z);
            return vector;
        }
    }
    public static class Vector2Extensions
    {
        public static Vector2 Clamp(this Vector2 vector, Vector2 minVector, Vector2 maxVector)
        {
            vector.x = Mathf.Clamp(vector.x, minVector.x, maxVector.x);
            vector.y = Mathf.Clamp(vector.y, minVector.y, maxVector.y);
            return vector;
        }
    }
}
