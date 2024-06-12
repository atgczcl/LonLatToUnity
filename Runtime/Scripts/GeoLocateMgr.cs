using ATGC.GEO;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ATGC.GEO
{
    public class GeoLocateMgr : MonoSingletonNoDontDestroy<GeoLocateMgr>
    {

        /// <summary>
        /// 基准对象
        /// </summary>
        public LocaterObj CenterLocaterObj;
        /// <summary>
        /// 缩放比例
        /// </summary>
        public float Scale = 100;

        /// <summary>
        /// 所有LocaterObj的集合
        /// </summary>
        public List<LocaterObj> m_LocaterObjs = new List<LocaterObj>();

        private void Start()
        {
            //m_LocaterObjs 初始化
            m_LocaterObjs = FindObjectsByType<LocaterObj>(FindObjectsSortMode.InstanceID).ToList();
            UpdateLocaterObjsByBase();
        }

        /// <summary>
        /// 通过BaseLocaterObj计算其他所有LocaterObj的位置
        /// </summary>
        public void UpdateLocaterObjsByBase()
        {
            if (CenterLocaterObj == null)
            {
                return;
            }
            CenterLocaterObj.InitGeoCoordinateModel();
            // 遍历所有LocaterObj, 所有的obj球面坐标减去BaseLocaterObj的SphereCoords
            foreach (LocaterObj obj in m_LocaterObjs)
            {
                if (obj == CenterLocaterObj)
                {
                    continue;
                }
                //通过centerLocaterObj计算obj的坐标
                obj.InitGeoCoordinateModel();
                obj.m_GeoCoordinateModel.CalculateInfo(CenterLocaterObj.m_GeoCoordinateModel.WebMercatorPos);
                //计算缩放后的unity坐标
                Vector2 centerUnityPos = new Vector2(CenterLocaterObj.transform.position.x, CenterLocaterObj.transform.position.z);
                var scaledUnityPos = obj.m_GeoCoordinateModel.CalculateScaledUnityPos(centerUnityPos, Scale);
                obj.transform.position = new Vector3(scaledUnityPos.x, 0, scaledUnityPos.y);
            }
        }

        /// <summary>
        /// 通过string longitude,latitude 计算unity坐标
        /// </summary>
        /// <param name="lonLatStr"></param>
        /// <returns></returns>
        public Vector2 CalculateUnityPos(string lonLatStr)
        {
            //初始化GeoCoordinateModel
            GeoCoordinateModel geoCoordinateModel = new GeoCoordinateModel(lonLatStr);
            //中心点unity坐标
            Vector2 centerUnityPos = new Vector2(CenterLocaterObj.transform.position.x, CenterLocaterObj.transform.position.z);
            //计算缩放后的unity坐标
            var scaledUnityPos = geoCoordinateModel.CalculateScaledUnityPos(centerUnityPos, Scale);
            return scaledUnityPos;
        }
        



    }
}