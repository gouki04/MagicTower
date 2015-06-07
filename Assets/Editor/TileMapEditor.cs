using UnityEditor;
using UnityEngine;
using MagicTower;
using Utils;
using System.Collections.Generic;

namespace MagicTower.Editor
{
    [CustomEditor(typeof(EditorData.TileMap))]
    public class TileMapEditor : UnityEditor.Editor
    {
        EditorData.TileMap mTilemap;
        
        public void OnEnable()
        {
//            mTilemap = (EditorData.TileMap)target;

            //SceneView.onSceneGUIDelegate += TileMapUpdate;
        }

        //void TileMapUpdate(SceneView scene_view)
        //{
        //    Event e = Event.current;
        //    if (e.type == EventType.MouseDown)
        //    {
        //        Ray r = Camera.current.ScreenPointToRay(
        //                        new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));

        //        var mouse_pos = r.origin;

        //        var local_pos = mTilemap.gameObject.transform.InverseTransformPoint(mouse_pos);
        //        local_pos.x = Mathf.Floor(local_pos.x + 0.0f);
        //        local_pos.y = Mathf.Floor(local_pos.y + 0.0f);
        //        local_pos.z = 0.0f;

        //        if (local_pos.x < 0 || local_pos.x >= mTilemap.Width || local_pos.y < 0 || local_pos.y >= mTilemap.Height)
        //            return;

        //        var obj = new GameObject("Tile");

        //        var renderer = obj.AddComponent<SpriteRenderer>();

        //        if (Selecting.SelectedList is MonsterList)
        //            renderer.sprite = (Selecting.SelectedList as MonsterList).GetSprite(Selecting.SelectedIndex);
        //        else if (Selecting.SelectedList is ItemList)
        //            renderer.sprite = (Selecting.SelectedList as ItemList).GetSprite(Selecting.SelectedIndex);

        //        mTilemap.SetTile((int)local_pos.y, (int)local_pos.x, obj);
        //        //obj.transform.parent = mTilemap.transform;
        //        //obj.transform.localPosition = local_pos;
        //    }
        //}

        //public override void OnInspectorGUI()
        //{
        //    SceneView.RepaintAll();
        //}
    } 
}