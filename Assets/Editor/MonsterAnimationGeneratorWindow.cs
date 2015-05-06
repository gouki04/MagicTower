using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MagicTower.Editor
{
    public class Helper
    {
        public static AnimationClip CreateAnimationClip(
            string animation_name, 
            float animation_duration, 
            float frame_rate, 
            string[] frame_names, 
            bool loop, 
            string asset_path)
        {
            AnimationClip clip = new AnimationClip();
            AnimationUtility.SetAnimationType(clip, ModelImporterAnimationType.Generic);

            EditorCurveBinding curve_binding = new EditorCurveBinding();
            curve_binding.type = typeof(SpriteRenderer);
            curve_binding.path = "";
            curve_binding.propertyName = "m_Sprite";

            ObjectReferenceKeyframe[] key_frames = new ObjectReferenceKeyframe[3];

            float frame_time = animation_duration / frame_names.Length;
            for (int i = 0; i < frame_names.Length; ++i)
            {
                var sprite = Utils.SpriteSheetManager.Instance.GetSprite(frame_names[i]);

                key_frames[i] = new ObjectReferenceKeyframe();
                key_frames[i].time = frame_time * i;
                key_frames[i].value = sprite;
            }

            clip.frameRate = frame_rate;

            if (loop)
            {
                //设置为循环动画
                SerializedObject serialized_clip = new SerializedObject(clip);
                AnimationClipSettings clip_settings = new AnimationClipSettings(serialized_clip.FindProperty("m_AnimationClipSettings"));
                clip_settings.loopTime = true;
                serialized_clip.ApplyModifiedProperties();
            }

            AnimationUtility.SetObjectReferenceCurve(clip, curve_binding, key_frames);
            AssetDatabase.CreateAsset(clip, asset_path);
            AssetDatabase.SaveAssets();

            return clip;
        }

        public static AnimatorController CreateEmptyAnimatorController(string asset_path)
        {
            return AnimatorController.CreateAnimatorControllerAtPath(asset_path);
        }

        public static AnimatorController CreateSimpleAnimatorController(AnimationClip clip, string asset_path)
        {
            var animator_controller = CreateEmptyAnimatorController(asset_path);

            var layer = animator_controller.GetLayer(0);
            var sm = layer.stateMachine;

            var state = sm.AddState(clip.name);
            state.SetAnimationClip(clip, layer);

            AssetDatabase.SaveAssets();

            return animator_controller;
        }

        class AnimationClipSettings
        {
            SerializedProperty m_Property;

            private SerializedProperty Get(string property) { return m_Property.FindPropertyRelative(property); }

            public AnimationClipSettings(SerializedProperty prop) { m_Property = prop; }

            public float startTime { get { return Get("m_StartTime").floatValue; } set { Get("m_StartTime").floatValue = value; } }
            public float stopTime { get { return Get("m_StopTime").floatValue; } set { Get("m_StopTime").floatValue = value; } }
            public float orientationOffsetY { get { return Get("m_OrientationOffsetY").floatValue; } set { Get("m_OrientationOffsetY").floatValue = value; } }
            public float level { get { return Get("m_Level").floatValue; } set { Get("m_Level").floatValue = value; } }
            public float cycleOffset { get { return Get("m_CycleOffset").floatValue; } set { Get("m_CycleOffset").floatValue = value; } }

            public bool loopTime { get { return Get("m_LoopTime").boolValue; } set { Get("m_LoopTime").boolValue = value; } }
            public bool loopBlend { get { return Get("m_LoopBlend").boolValue; } set { Get("m_LoopBlend").boolValue = value; } }
            public bool loopBlendOrientation { get { return Get("m_LoopBlendOrientation").boolValue; } set { Get("m_LoopBlendOrientation").boolValue = value; } }
            public bool loopBlendPositionY { get { return Get("m_LoopBlendPositionY").boolValue; } set { Get("m_LoopBlendPositionY").boolValue = value; } }
            public bool loopBlendPositionXZ { get { return Get("m_LoopBlendPositionXZ").boolValue; } set { Get("m_LoopBlendPositionXZ").boolValue = value; } }
            public bool keepOriginalOrientation { get { return Get("m_KeepOriginalOrientation").boolValue; } set { Get("m_KeepOriginalOrientation").boolValue = value; } }
            public bool keepOriginalPositionY { get { return Get("m_KeepOriginalPositionY").boolValue; } set { Get("m_KeepOriginalPositionY").boolValue = value; } }
            public bool keepOriginalPositionXZ { get { return Get("m_KeepOriginalPositionXZ").boolValue; } set { Get("m_KeepOriginalPositionXZ").boolValue = value; } }
            public bool heightFromFeet { get { return Get("m_HeightFromFeet").boolValue; } set { Get("m_HeightFromFeet").boolValue = value; } }
            public bool mirror { get { return Get("m_Mirror").boolValue; } set { Get("m_Mirror").boolValue = value; } }
        }
    }

    /// <summary>
    /// 基于文章《Unity2D研究院之自动生成动画、AnimationController、Prefab（一）》
    /// http://www.xuanyusong.com/archives/3243
    /// </summary>
    public class MonsterPrefabGeneratorWindow : EditorWindow
    {
        public int BeginIndex = 1;
        public int EndIndex = 31;

        public string ANIMATION_PATH = "Assets/Resources/Animation/";
        public string ANIMATOR_CONTROLLER_PATH = "Assets/Resources/Animation/";
        public string PREFAB_PATH = "Assets/Resources/Prefabs/";

        [MenuItem("Window/Monster Prefab Generator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MonsterPrefabGeneratorWindow));
        }

        AnimationClip GenerateAnimationClip(int index)
        {
            return Helper.CreateAnimationClip("monster_" + index,
                1f, 30f, new string[] { "monster_" + index, "monster_" + index + "_1", "monster_" + index }, true, string.Format("{0}monster_{1}.anim", ANIMATION_PATH, index));

            //string animation_name = "monster_" + index;
            //AnimationClip clip = new AnimationClip();
            //AnimationUtility.SetAnimationType(clip, ModelImporterAnimationType.Generic);

            //EditorCurveBinding curve_binding = new EditorCurveBinding();
            //curve_binding.type = typeof(SpriteRenderer);
            //curve_binding.path = "";
            //curve_binding.propertyName = "m_Sprite";

            //ObjectReferenceKeyframe[] key_frames = new ObjectReferenceKeyframe[3];

            //float frame_time = 1 / 3f;
            //for (int i = 0; i < 3; ++i)
            //{
            //    Sprite sprite = null;
            //    if (i == 0 || i == 2)
            //        sprite = Utils.SpriteSheetManager.Instance.GetSprite("monster_" + index);
            //    else if (i == 1)
            //        sprite = Utils.SpriteSheetManager.Instance.GetSprite("monster_" + index + "_1");

            //    key_frames[i] = new ObjectReferenceKeyframe();
            //    key_frames[i].time = frame_time * i;
            //    key_frames[i].value = sprite;
            //}

            //clip.frameRate = 30;

            ////设置为循环动画
            //SerializedObject serialized_clip = new SerializedObject(clip);
            //AnimationClipSettings clip_settings = new AnimationClipSettings(serialized_clip.FindProperty("m_AnimationClipSettings"));
            //clip_settings.loopTime = true;
            //serialized_clip.ApplyModifiedProperties();

            //AnimationUtility.SetObjectReferenceCurve(clip, curve_binding, key_frames);
            //AssetDatabase.CreateAsset(clip, string.Format("{0}monster_{1}.anim", ANIMATION_PATH, index));
            //AssetDatabase.SaveAssets();

            //return clip;
        }

        AnimatorController GenerateAnimationController(List<AnimationClip> clips, int index)
        {
            var animator_controller = AnimatorController.CreateAnimatorControllerAtPath(
                string.Format("{0}monster_{1}.controller", ANIMATOR_CONTROLLER_PATH, index));

            var layer = animator_controller.GetLayer(0);
            var sm = layer.stateMachine;
            foreach (var newClip in  clips)
            {
                var state = sm.AddState(newClip.name);
                state.SetAnimationClip(newClip, layer);
                //var trans = sm.AddAnyStateTransition(state);
                //trans.RemoveCondition(0);
            }
            AssetDatabase.SaveAssets();
            return animator_controller;
        }

        void GeneratePrefab(AnimatorController animator_controller, int index)
        {
            var go = new GameObject();
            go.name = "monster_" + index;

            var sprite_renderer = go.AddComponent<SpriteRenderer>();
            sprite_renderer.sprite = Utils.SpriteSheetManager.Instance["monster_" + index];

            var animator = go.AddComponent<Animator>();
            animator.runtimeAnimatorController = animator_controller;

            PrefabUtility.CreatePrefab(string.Format("{0}monster_{1}.prefab", PREFAB_PATH, index), go);
        }

        void Generate()
        {
            Utils.SpriteSheetManager.Instance.Load("tile");

            for (int i = BeginIndex; i <= EndIndex; ++i)
            {
                //var clips = new List<AnimationClip>();
                //clips.Add(GenerateAnimationClip(i));
                
                //var animator_controller = GenerateAnimationController(clips, i);
                var clip = GenerateAnimationClip(i);
                var animator_controller = Helper.CreateSimpleAnimatorController(clip, 
                    string.Format("{0}monster_{1}.controller", ANIMATOR_CONTROLLER_PATH, i));

                GeneratePrefab(animator_controller, i);
            }
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Begin Index");
            BeginIndex = EditorGUILayout.IntField(BeginIndex);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("End Index");
            EndIndex = EditorGUILayout.IntField(EndIndex);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Generate"))
            {
                Utils.Logger.LogDebug("Click Generate!!!!");
                Generate();
            }
        }

        
    }
}
