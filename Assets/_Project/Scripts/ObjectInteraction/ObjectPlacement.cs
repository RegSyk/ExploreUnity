using System;
using UnityEngine;

namespace Explore
{
    public class ObjectPlacement : MonoBehaviour
    {
        [field: Header("State")]
        [field: SerializeField] public Vector3 MeshOffset { get; private set; }
        [field: SerializeField] public Vector3 MeshCenter { get; private set; }
                                public Vector3 Position => _MyTransform.position;
                                public TransformData TransformData => new TransformData()
                                {
                                    Position = Position,
                                    Rotation = _MyTransform.rotation
                                };
                                public Vector3 PositionWithOffset => Position - MeshOffset;
                                public TransformData OriginalLocalPosition 
                                    => _OriginalPositionBackedUp ? 
                                        _OriginalLocalPosition : throw new TransformBackupException();

        [field: SerializeField] private TransformData _OriginalLocalPosition { get; set; }
        [field: SerializeField] private Transform _TransformParentBackup { get; set; }
        private bool _OriginalPositionBackedUp;

        [field: Header("References")]
        [field: SerializeField] private MeshFilter _MyMeshFilter { get; set; }
        [field: SerializeField] private Transform _MyTransform { get; set; }

        public ObjectPlacement Init()
        {
            if (_MyTransform)
            {
                Debug.LogError("MyTransform is not null!");
            }
            if (_MyMeshFilter)
            {
                Debug.LogError("MyMeshFilter is not null!");
            }
            _MyTransform = transform;
            BackupTransformParent();
            _MyMeshFilter = gameObject.GetComponent<MeshFilter>();
            return this;
        }
        public ObjectPlacement BackupCurrentPosition()
        {
            _OriginalLocalPosition = new TransformData()
            {
                Position = _MyTransform.localPosition,
                Rotation = _MyTransform.localRotation
            };
            _OriginalPositionBackedUp = true;
            return this;
        }
        public ObjectPlacement GetInstance(out ObjectPlacement objectPlacement)
        {
            objectPlacement = this;
            return objectPlacement;
        }
        public ObjectPlacement RestorePosition()
        {
            if (!_OriginalPositionBackedUp)
            {
                throw new InvalidOperationException("Backup is not present");
            }
            _MyTransform.localPosition = _OriginalLocalPosition.Position;
            _MyTransform.localRotation = _OriginalLocalPosition.Rotation;
            return this;
        }
        public ObjectPlacement RestoreTransformParent()
        {
            _MyTransform.parent = _TransformParentBackup;
            return this;
        }
        public ObjectPlacement CalculateMeshOffset()
        {
            if (_MyMeshFilter == null)
            {
                throw new ArgumentNullException("Mesh filter is null!");
            }
            MeshCenter = _MyMeshFilter.mesh.CalculateMeshCenter();
            Vector3 meshCenterWorld = _MyTransform.TransformPoint(MeshCenter);
            MeshOffset = _MyTransform.position - meshCenterWorld;
            return this;
        }

        public void SetPosition(Vector3 position)
        {
            _MyTransform.position = position;
        }
        public void SetRotation(Quaternion rotation)
        {
            _MyTransform.rotation = rotation;
        }
        public void SetTransformData(TransformData transformData)
        {
            _MyTransform.position = transformData.Position;
            _MyTransform.rotation = transformData.Rotation;
        }

        private void BackupTransformParent()
        {
            _TransformParentBackup = _MyTransform.parent;
        }
    }
}
