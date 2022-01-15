using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Control
{
    public partial class PlayerController : MonoBehaviour
    {
        [SerializeField] float maxNavMeshProjectionDistance = 1f;

        Health health;
        public bool controllable = true;

        [SerializeField] float raycastRadius = 1f;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;

            if (health.IsDead()) 
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;

            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this)) 
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            float[] distances = new float[hits.Length];
            for (var i = 0; i < hits.Length; i++) distances[i] = hits[i].distance;
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            bool hasHit = RaycastNavmesh(out Vector3 target);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavmesh(out Vector3 target)
        {
            target = new Vector3();
            
            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);
            if (!hasHit) return false;

            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out NavMeshHit navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;
            
            target = navMeshHit.position;

            return true;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type) return mapping;
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
