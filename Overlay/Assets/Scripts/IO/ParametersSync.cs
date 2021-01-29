using UnityEngine;

namespace IO
{
    public class ParametersSync : MonoBehaviour
    {
        public RectTransform pivotTransform;
        
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public void Update()
        {
            var pos = pivotTransform.position;
            X = pos.x;
            Y = pos.y;
            Z = pos.z;
        }
    }
}