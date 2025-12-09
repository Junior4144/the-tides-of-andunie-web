using System;
using UnityEngine;


namespace ___Scripts.Controllers.Level_Selector
{
    public class TreeMapLODController : MonoBehaviour
    {
        [SerializeField] private GameObject treeGameObject;
        private Camera _cam;

        private void Start()
        {
            _cam = CameraManager.Instance.GetCamera();
        }

        private void Update()
        {
            var zoom = _cam.orthographicSize;

            if(zoom >= 150f)
            {
                treeGameObject.SetActive(false);
            }
            else
            {
                treeGameObject.SetActive(true);
            }
        }
    }
}

