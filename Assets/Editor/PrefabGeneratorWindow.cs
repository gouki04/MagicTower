using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MagicTower.Editor
{
    /// <summary>
    /// 基于文章《Unity2D研究院之自动生成动画、AnimationController、Prefab（一）》
    /// http://www.xuanyusong.com/archives/3243
    /// </summary>
    public partial class PrefabGeneratorWindow : EditorWindow
    {
        public abstract class Generator
        {
            protected string mAnimationPath;
            protected string mAnimatorControllerPath;
            protected string mPrefabPath;

            public void Init(string animation_path, string animator_controller_path, string prefab_path)
            {
                mAnimationPath = animation_path;
                mAnimatorControllerPath = animator_controller_path;
                mPrefabPath = prefab_path;
            }

            public abstract void Generate();
        }

        private MonsterGenerator mMonsterGenerator;
        private PlayerGenerator mPlayerGenerator;
        private TerrainGenerator mTerrainGenerator;

        public string ANIMATION_PATH = "Assets/Resources/Animation/";
        public string ANIMATOR_CONTROLLER_PATH = "Assets/Resources/Animation/";
        public string PREFAB_PATH = "Assets/Resources/Prefabs/";

        [MenuItem("Window/Prefab Generator")]
        public static void ShowWindow()
        {
            Utils.SpriteSheetManager.Instance.Load("tile");

            EditorWindow.GetWindow(typeof(PrefabGeneratorWindow));
        }

        public PrefabGeneratorWindow()
        {
            mMonsterGenerator = new MonsterGenerator();
            mMonsterGenerator.Init(ANIMATION_PATH, ANIMATOR_CONTROLLER_PATH, PREFAB_PATH);

            mPlayerGenerator = new PlayerGenerator();
            mPlayerGenerator.Init(ANIMATION_PATH, ANIMATOR_CONTROLLER_PATH, PREFAB_PATH);

            mTerrainGenerator = new TerrainGenerator();
            mTerrainGenerator.Init(ANIMATION_PATH, ANIMATOR_CONTROLLER_PATH, PREFAB_PATH);
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Monster");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Begin Index");
            mMonsterGenerator.BeginIndex = EditorGUILayout.IntField(mMonsterGenerator.BeginIndex);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("End Index");
            mMonsterGenerator.EndIndex = EditorGUILayout.IntField(mMonsterGenerator.EndIndex);
            EditorGUILayout.EndHorizontal();

            if (!mMonsterGenerator.IsGenerating)
            {
                if (GUILayout.Button("Generate"))
                {
                    mMonsterGenerator.Generate();
                }
            }
            else
            {
                mMonsterGenerator.Update();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Player");
            if (GUILayout.Button("Generate"))
            {
                mPlayerGenerator.Generate();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Terrain");
            if (GUILayout.Button("Generate"))
            {
                mTerrainGenerator.Generate();
            }
            EditorGUILayout.EndVertical();
        }

        
    }
}
