using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtension
    {
        /// <summary>
        ///     获取组件, 如无则添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            return go.GetComponent<T>() ?? go.AddComponent<T>();
        }

        /// <summary>
        ///     根据指定路径查找组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T FindComponent<T>(this GameObject go, string name) where T : Component
        {
            var t = go.transform.Find(name);
            return t == null ? null : t.GetComponent<T>();
        }

        /// <summary>
        ///     根据指定路径查找物件
        /// </summary>
        /// <param name="go"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject FindObject(this GameObject go, string name)
        {
            var t = go.transform.Find(name);
            return t == null ? null : t.gameObject;
        }

        /// <summary>
        ///     不指定路径, 在子物件中根据名字查找物件
        /// </summary>
        /// <remarks>
        ///     <para>有性能问题, 只能在Editor模式下使用</para>
        /// </remarks>
        /// <param name="go"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject FindObjectInChildren(this GameObject go, string name)
        {
            var t = go.transform.FindInChildren(name);
            return t == null ? null : t.gameObject;
        }

        /// <summary>
        ///     不指定路径, 在子物件中根据名字查找物件
        /// </summary>
        /// <remarks>
        ///     <para>有性能问题, 只能在Editor模式下使用</para>
        /// </remarks>
        /// <param name="go"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<GameObject> FindObjectsInChildren(this GameObject go, string name)
        {
            if (!go) return new List<GameObject>();
            var all = go.transform.FindAllInChildren(name);
            return from t in all where t select t.gameObject;
        }

        /// <summary>
        ///     不指定路径, 在子物件中根据名字查找物件
        /// </summary>
        /// <remarks>
        ///     <para>有性能问题, 只能在Editor模式下使用</para>
        /// </remarks>
        /// <param name="transform"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform FindInChildren(this Transform transform, string name)
        {
            if (Application.isPlaying)
                Debug.LogWarning("Use Transform.FindInChildren may cause perfermance issue");

            var children = transform.GetComponentsInChildren<Transform>(true);
            return children.FirstOrDefault(child => child.name.Trim() == name);
        }

        /// <summary>
        ///     不指定路径, 在子物件中根据名字查找所有物件
        /// </summary>
        /// <remarks>
        ///     <para>有性能问题, 只能在Editor模式下使用</para>
        /// </remarks>
        /// <param name="transform"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<Transform> FindAllInChildren(this Transform transform, string name)
        {
            if (Application.isPlaying)
                Debug.LogWarning("Use Transform.FindAllInChildren may cause perfermance issue");

            var children = transform.GetComponentsInChildren<Transform>(true);
            return children.Where(child => child.name.Trim() == name);
        }

        /// <summary>
        ///     获取物件在在场景或者预制体中的路径
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static string GetPath(this GameObject go)
        {
            if (go == null) return "";
            var t = go.transform;
            string path = null;
            while (t != null)
            {
                if (string.IsNullOrEmpty(path))
                    path = t.name;
                else
                    path = t.name + "/" + path;
                t = t.parent;
            }
            return path;
        }

        public static bool ParticleSystemIsLoop(this GameObject go)
        {
            if (go == null)
                return false;
            var particleSystems = go.GetComponentsInChildren<ParticleSystem>(true);
            if (particleSystems.IsNullOrEmpty())
                return false;

            return particleSystems.Select(ps => ps.main).Any(mainModule => mainModule.loop);
        }

        /// <summary>
        /// 计算游戏物件下粒子特效持续时间
        /// -1 代表无效
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static float GetParticleSystemDuration(this GameObject go)
        {
            if (go == null)
                return -1;
            var particleSystems = go.GetComponentsInChildren<ParticleSystem>(true);
            if (particleSystems.IsNullOrEmpty())
                return -1;

            bool isLoop = false;
            float totalDuration = 0;
            foreach (var ps in particleSystems)
            {
                var mainModule = ps.main;
                if (mainModule.loop) isLoop = true;
                float startDelay = mainModule.startDelay.constant;
                float lifetime = 0;
                if (mainModule.startLifetime.mode == ParticleSystemCurveMode.Constant)
                    lifetime = mainModule.startLifetime.constant;
                else if (mainModule.startLifetime.mode == ParticleSystemCurveMode.TwoConstants)
                    lifetime = mainModule.startLifetime.constantMax;
                float particleDuration = startDelay + mainModule.duration + lifetime;
                if (particleDuration > totalDuration)
                    totalDuration = particleDuration;
            }

            if (isLoop)
                return -1;

            return totalDuration;
        }
    }
}
