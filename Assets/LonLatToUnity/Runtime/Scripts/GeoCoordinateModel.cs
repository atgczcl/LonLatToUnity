using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATGC.GEO
{
    /// <summary>
    /// 地理坐标模型
    /// </summary>
    [Serializable]
    public class GeoCoordinateModel
    {
        /// <summary>
        /// 纬度
        /// </summary>
        public double latitude = 34.05f;   // 纬度
        /// <summary>
        /// 经度
        /// </summary>
        public double longitude = -118.25f; // 经度

        /// <summary>
        /// 墨卡托投影坐标
        /// </summary>
        public Vector2d WebMercatorPos;
        public double calc_distance;
        //计算单位向量
        public Vector2d calc_directionVector;
        public Vector2d calc_unitVector;
        public Vector2d calc_unityPos;

        public string LonLatStr { get; }

        public GeoCoordinateModel(double lon, double lat)
        {
            latitude = lat;
            longitude = lon;
            CalculateWebMercatorPos();
        }

        public GeoCoordinateModel(string lonLatStr)
        {
            LonLatStr = lonLatStr;
            if (!string.IsNullOrEmpty(LonLatStr))
            {
                //分解出经纬度
                string[] strs = LonLatStr.Split(',');
                if (strs.Length == 2)
                {
                    double.TryParse(strs[0], out longitude);
                    double.TryParse(strs[1], out latitude);
                }
            }
        }

        /// <summary>
        /// 计算墨卡托投影坐标
        /// </summary>
        public void CalculateWebMercatorPos()
        {
            WebMercatorPos = GeoUtils.LonLatToWebMercator(longitude, latitude);
        }

        /// <summary>
        /// 获取相对坐标
        /// </summary>
        /// <param name="centerPos"></param>
        /// <returns></returns>
        public Vector2d GetRelativePos(Vector2d centerPos)
        {
            return new Vector2d(WebMercatorPos.x - centerPos.x, WebMercatorPos.y - centerPos.y);
        }

        /// <summary>
        /// CalculateInfo - 计算信息
        /// </summary>
        public void CalculateInfo(Vector2d centerWebMercatorPos)
        {
            //计算离中心点的距离
            calc_distance = Vector2d.Distance(WebMercatorPos, centerWebMercatorPos);
            //计算单位向量
            calc_directionVector = WebMercatorPos - centerWebMercatorPos;
            calc_unitVector = calc_directionVector.normalized;

        }

        /// <summary>
        /// 计算缩放后的unity坐标
        /// </summary>
        /// <param name="centerUnityPos"> 中心点的unity坐标 x,z水平坐标</param>
        /// <param name="scale"></param>
        public Vector2 CalculateScaledUnityPos(Vector2 centerUnityPos, float scale)
        {
            //计算延长线上新点的位置
            if (scale == 0) return centerUnityPos;
            Vector2d centerWebMercatorPos = new Vector2d(centerUnityPos.x, centerUnityPos.y);
            calc_unityPos = centerWebMercatorPos + (calc_unitVector * calc_distance / scale);
            return new Vector2((float)calc_unityPos.x, (float)calc_unityPos.y);
        }
    }
}