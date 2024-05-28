using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MagicTower.Editor
{
    public partial class PrefabGeneratorWindow : EditorWindow
    {
        public class MonsterGenerator : Generator
        {
            public int BeginIndex = 1;
            private int CurIndex = 0;
            public int EndIndex = 31;

            public bool IsGenerating = false;

            AnimationClip GenerateAnimationClip(int index)
            {
                return Helper.CreateAnimationClip("monster_" + index,
                    1f, 30f,
                    new string[] { "monster_" + index, "monster_" + index + "_1", "monster_" + index },
                    true, string.Format("{0}monster_{1}.anim", m_AnimationPath, index));
            }

            void GeneratePrefab(UnityEditor.Animations.AnimatorController animator_controller, int index)
            {
                var go = new GameObject();
                go.name = "monster_" + index;

                var sprite_renderer = go.AddComponent<SpriteRenderer>();
                sprite_renderer.sprite = Utils.SpriteSheetManager.Instance["monster_" + index];

                var animator = go.AddComponent<Animator>();
                animator.runtimeAnimatorController = animator_controller;

                PrefabUtility.CreatePrefab(string.Format("{0}monster_{1}.prefab", m_PrefabPath, index), go);

                DestroyImmediate(go);
            }

            public override void Generate()
            {
                IsGenerating = true;
                CurIndex = BeginIndex;

                //Utils.SpriteSheetManager.Instance.Load("tile");

                //for (int i = BeginIndex; i <= EndIndex; ++i)
                //{
                //    var clip = GenerateAnimationClip(i);
                //    var animator_controller = Helper.CreateSimpleAnimatorController(clip,
                //        string.Format("{0}monster_{1}.controller", mAnimatorControllerPath, i));

                //    GeneratePrefab(animator_controller, i);
                //}
            }

            public void Update()
            {
                if (IsGenerating)
                {
                    var clip = GenerateAnimationClip(CurIndex);
                    var animator_controller = Helper.CreateSimpleAnimatorController(clip,
                        string.Format("{0}monster_{1}.controller", m_AnimatorControllerPath, CurIndex));

                    GeneratePrefab(animator_controller, CurIndex);

                    ++CurIndex;

                    if (CurIndex > EndIndex)
                        IsGenerating = false;
                }
            }
        }
    }
}
