using UnityEngine;
using UnityEngine.EventSystems;

namespace Controller
{
    public class ModelController : MonoBehaviour
    {
        public GameObject model;
        private float RotationX { get; set; }
        private float RotationY { get; set; }
        
        private Camera _cameraMain;
        private Vector3 _screenPoint;
        private Vector3 _offset;
        private bool _isPanning;
        private const float SensX = 300.0f;
        private const float SensY = 300.0f;
        private Renderer _ren;

        private void Awake()
        {
            Debug.Assert(Camera.main != null, "Camera.main != null");
            _cameraMain = Camera.main;
            
            var tr = new Vector3(0, 0, 0);
            foreach (Transform child in model.transform)
            {
                _ren = child.gameObject.GetComponentInChildren<Renderer>();
                tr += _ren.bounds.center;
            }

            tr /= model.transform.childCount;
            model.transform.position -= tr;
            var moveZ = FitContent();

            var cameraTransform = _cameraMain.transform;
            cameraTransform.position = transform.position + new Vector3(0, 0, -moveZ);
        }
        
        private float FitContent()
        {
            var maxBound = 0F;

            foreach (Transform child in model.transform)
            {
                _ren = child.gameObject.GetComponentInChildren<Renderer>();
                var currentMax = _ren.bounds.size.magnitude;
                if (currentMax > maxBound) maxBound = currentMax;
            }

            return maxBound * Mathf.Atan(60);
        }
        
        private void Update()
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) RotateModel();
            
            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                _screenPoint = _cameraMain.WorldToScreenPoint(gameObject.transform.position);
                _offset = transform.position -
                         _cameraMain.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                             _screenPoint.z));

                _isPanning = Input.GetMouseButton(1);
            }

            _isPanning &= Input.GetMouseButton(1);
            if (!_isPanning) return;
            
            var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);

            var curPosition = _cameraMain.ScreenToWorldPoint(curScreenPoint) + _offset;
            transform.position = curPosition;
        }

        private void LateUpdate()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && _cameraMain.fieldOfView > 1) 
                _cameraMain.fieldOfView--;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && _cameraMain.fieldOfView <= 75) 
                _cameraMain.fieldOfView++;
        }

        private void RotateModel()
        {
            RotationX = Input.GetAxis("Mouse X") * SensX * Time.deltaTime;
            RotationY = Input.GetAxis("Mouse Y") * SensY * Time.deltaTime;
            transform.Rotate (new Vector3(RotationY,-RotationX,0), Space.World);
        }

        public void MoveWithVector(IO.PacketIn packet)
        {
            var vec = new Vector3(packet.X, packet.Y, packet.Z);
            transform.position += vec;
        }
    }
}