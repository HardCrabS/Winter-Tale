using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        [SerializeField] Texture2D walkCursor;
        [SerializeField] Texture2D enemyCursor;
        [SerializeField] Texture2D destroyableCursor;
        [SerializeField] Vector2 cursorHotSpot = new Vector2(96, 96);

        float maxRaycastDepth = 100f; // Hard coded value
        const int CLICK_TO_WALK_LAYER = 9;

        public delegate void OnMouseOverEnemy(EnemyAI enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        public delegate void OnMouseOverTerrain(Vector3 destination);
        public event OnMouseOverTerrain onMouseOverTerrain;

        public delegate void OnMouseOverDestroyable(Destroyable destroyable);
        public event OnMouseOverDestroyable onMouseOverDestroyable;

        void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return; // Stop looking for other objects
            }
            else
            {
                PerformRaycasts();
            }
        }

        void PerformRaycasts()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //layer priorities
            if(RaycastForEnemy(ray)) { return; }
            if (RaycastForDestroyable(ray)) { return; }
            if (RaycastForWalkable(ray)) { return; }
        }
        private bool RaycastForEnemy(Ray ray)
        {
            RaycastHit raycastHit;
            Physics.Raycast(ray, out raycastHit, maxRaycastDepth);
            if (raycastHit.collider == null) return false;
            var goHit = raycastHit.collider.gameObject;
            var hitEnemy = goHit.GetComponent<EnemyAI>();
            if (hitEnemy)
            {
                Cursor.SetCursor(enemyCursor, cursorHotSpot, CursorMode.Auto);
                onMouseOverEnemy(hitEnemy);
                return true;
            }
            return false;
        }
        private bool RaycastForDestroyable(Ray ray)
        {
            RaycastHit raycastHit;
            Physics.Raycast(ray, out raycastHit, maxRaycastDepth);
            if (raycastHit.collider == null) return false;
            var goHit = raycastHit.collider.gameObject;
            var hitdestroyable = goHit.GetComponent<Destroyable>();
            if (hitdestroyable)
            {
                Cursor.SetCursor(destroyableCursor, cursorHotSpot, CursorMode.Auto);
                onMouseOverDestroyable(hitdestroyable);
                return true;
            }
            return false;
        }
        private bool RaycastForWalkable(Ray ray)
        {
            RaycastHit raycastHit;
            LayerMask potentiallyWalkableLayer = 1 << CLICK_TO_WALK_LAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out raycastHit, maxRaycastDepth, potentiallyWalkableLayer);
            if (potentiallyWalkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotSpot, CursorMode.Auto);
                onMouseOverTerrain?.Invoke(raycastHit.point);
                return true;
            }
            return false;
        }

        public bool OnTerrainHasSubs()
        {
            return onMouseOverTerrain != null;
        }
    }
}