using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BillBoardSystem : MonoBehaviour
{
    [SerializeField] protected CameraSystem _cameraSystem;

    [SerializeField] protected BillboardBase[] _horizontalBillboards;

    protected virtual void Awake()
    {
        for (int i = 0; i < _horizontalBillboards.Length; i++)
        {
            _horizontalBillboards[i].SetCamera(_cameraSystem.Camera);
        }
    }

    [ContextMenu("Find Billboards")]
    private void FindBillboards()
    {
        _horizontalBillboards = FindObjectsOfType<BillboardBase>(true);
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}
