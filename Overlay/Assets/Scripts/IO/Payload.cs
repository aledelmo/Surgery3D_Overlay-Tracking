using Controller;
using UnityEngine;

namespace IO
{
    public class PacketOut
    {
        public float X;
        public float Y;
        public float Z;
        public string Image;
    }
    
    public class PacketIn
    {
        public float X;
        public float Y;
        public float Z;
    }
    
    public class Payload : MonoBehaviour
    {
        public AsyncReadback ar;
        public ParametersSync parametersSync;
        public ModelController modelController;

        public string Encode()
        {
            var packet = new PacketOut {X = parametersSync.X, Y = parametersSync.Y, Z = parametersSync.Z,
                Image = ar.Frame};
            return JsonUtility.ToJson(packet);
        }

        public void Decode(string reply)
        {
            var json = JsonUtility.FromJson<PacketIn>(reply);
            Dispatcher.RunOnMainThread(() => modelController.MoveWithVector(json));
        }
    }
}