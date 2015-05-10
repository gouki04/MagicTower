using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MagicTower.Editor
{
    public partial class PrefabGeneratorWindow : EditorWindow
    {
        public class TerrainGenerator : Generator
        {
            public void GenerateWall()
            {
                var go = new GameObject();
                go.name = "wall";

                var sprite_renderer = go.AddComponent<SpriteRenderer>();
                sprite_renderer.sprite = Utils.SpriteSheetManager.Instance["wall_0"];

                PrefabUtility.CreatePrefab(string.Format("{0}wall.prefab", mPrefabPath), go);

                DestroyImmediate(go);
            }

            public void GenerateFloor()
            {
                var go = new GameObject();
                go.name = "floor";

                var sprite_renderer = go.AddComponent<SpriteRenderer>();
                sprite_renderer.sprite = Utils.SpriteSheetManager.Instance["floor_0"];

                PrefabUtility.CreatePrefab(string.Format("{0}floor.prefab", mPrefabPath), go);

                DestroyImmediate(go);
            }

            public void GenerateSky()
            {
                var go = new GameObject();
                go.name = "sky";

                var clip = Helper.CreateAnimationClip("sky", 1f, 30f,
                    new string[] { "sky_0", "sky_0_1", "sky_0" }, true,
                    mAnimationPath + "sky.anim");

                var animator_controller = Helper.CreateSimpleAnimatorController(clip,
                    mAnimatorControllerPath + "sky.controller");

                var sprite_renderer = go.AddComponent<SpriteRenderer>();
                sprite_renderer.sprite = Utils.SpriteSheetManager.Instance["sky_0"];

                var animator = go.AddComponent<Animator>();
                animator.runtimeAnimatorController = animator_controller;

                PrefabUtility.CreatePrefab(string.Format("{0}sky.prefab", mPrefabPath), go);

                DestroyImmediate(go);
            }

            public void GenerateWater()
            {
                var go = new GameObject();
                go.name = "water";

                var clip = Helper.CreateAnimationClip("water", 1f, 30f,
                    new string[] { "water_0", "water_0_1", "water_0" }, true,
                    mAnimationPath + "water.anim");

                var animator_controller = Helper.CreateSimpleAnimatorController(clip,
                    mAnimatorControllerPath + "water.controller");

                var sprite_renderer = go.AddComponent<SpriteRenderer>();
                sprite_renderer.sprite = Utils.SpriteSheetManager.Instance["water_0"];

                var animator = go.AddComponent<Animator>();
                animator.runtimeAnimatorController = animator_controller;

                PrefabUtility.CreatePrefab(string.Format("{0}water.prefab", mPrefabPath), go);

                DestroyImmediate(go);
            }

            public override void Generate()
            {
                GenerateFloor();
                GenerateWall();
                GenerateSky();
                GenerateWater();
            }
        }
    }
}
