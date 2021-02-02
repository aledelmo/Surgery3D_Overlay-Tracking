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
        public float Rx;
        public float Ry;
        public float Rz;
    }
    
    public class Payload : MonoBehaviour
    {
        public AsyncReadback asyncReadback;
        public ParametersSync parametersSync;
        public ModelController modelController;

        public string Encode()
        {
            var packet = new PacketOut {X = parametersSync.X, Y = parametersSync.Y, Z = parametersSync.Z,
                Image = asyncReadback.Frame};
            return JsonUtility.ToJson(packet);
        }

        public void Decode(string reply)
        {
            var json = JsonUtility.FromJson<PacketIn>(reply);
            var tra = new Vector3(json.X, json.Y, json.Z);
            var rot = new Vector3(json.Rx, json.Ry, json.Rz);
            Dispatcher.RunOnMainThread(() => modelController.MoveWithVector(tra, rot));
        }
    }
}