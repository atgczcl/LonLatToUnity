using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATGC.GEO
{
    /// <summary>
    /// 坐标类
    /// </summary>
    public class LocaterObj : MonoBehaviour
    {
        public string pixelInput = "114.117645,22.531821"; //输入的像素坐标
        /// <summary>
        /// 纬度
        /// </summary>
        private double latitude = 34.05f;   // 纬度
        /// <summary>
        /// 经度
        /// </summary>
        private double longitude = -118.25f; // 经度

        /// <summary>
        /// SGGeoCoordinateModel 坐标
        /// </summary>
        [SerializeField]
        public GeoCoordinateModel m_GeoCoordinateModel;

        /// <summary>
        /// 初始化GeoCoordinateModel
        /// </summary>
        public void InitGeoCoordinateModel()
        {
            if (!string.IsNullOrEmpty(pixelInput))
            {
                //分解出经纬度
                string[] strs = pixelInput.Split(',');
                if (strs.Length == 2)
                {
                    double.TryParse(strs[0], out longitude);
                    double.TryParse(strs[1], out latitude);
                }
            }
            m_GeoCoordinateModel = new GeoCoordinateModel(longitude, latitude);
        }


    }
}