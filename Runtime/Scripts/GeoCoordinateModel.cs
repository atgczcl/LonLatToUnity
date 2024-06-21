using Mapbox.Unity.Utilities;
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
        /// <summary>
        /// 计算的web距离
        /// </summary>
        public double calc_web_distance;
        /// <summary>
        /// 计算单位向量
        /// </summary>
        public Vector2d calc_directionVector;
        /// <summary>
        /// 计算的web经纬度
        /// </summary>
        public Vector2d calc_WebMercatorNormal;
        /// <summary>
        /// 计算的unity坐标
        /// </summary>
        public Vector2d calc_unityPos;
        /// <summary>
        /// unity的世界坐标
        /// </summary>
        public Vector2d unityPos_world;
        /// <summary>
        /// unity的本地坐标
        /// </summary>
        public Vector2d unityPos_local;

        public string LonLatStr;

        public GeoCoordinateModel()
        { }

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
        /// 设置世界unity坐标
        /// </summary>
        /// <param name="unityPos"></param>
        public void SetWorldUnityPos(Vector2 unityPos)
        {
            unityPos_world = new Vector2d(unityPos.x, unityPos.y);
        }
        
        /// <summary>
        /// 设置本地unity坐标
        /// </summary>
        /// <param name="unityPos"></param>
        public void SetLocalUnityPos(Vector2 unityPos)
        {
            unityPos_local = new Vector2d(unityPos.x, unityPos.y);
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
            calc_web_distance = Vector2d.Distance(WebMercatorPos, centerWebMercatorPos);
            // 计算方向向量
            calc_directionVector = WebMercatorPos - centerWebMercatorPos;
            // 计算单位向量
            calc_WebMercatorNormal = calc_directionVector.normalized;

        }

        /// <summary>
        /// 计算缩放后的unity坐标，假设基准点的方向dir(0,0,0)和web mercator方向相同
        /// </summary>
        /// <param name="centerUnityPos"> 中心点的unity坐标 x,z水平坐标</param>
        /// <param name="scale"></param>
        public Vector2 CalculateScaledUnityPos(Vector2 centerUnityPos, float scale)
        {
            //计算延长线上新点的位置
            if (scale == 0) return centerUnityPos;
            Vector2d centerWebMercatorPos = new Vector2d(centerUnityPos.x, centerUnityPos.y);
            calc_unityPos = centerWebMercatorPos + (calc_WebMercatorNormal * calc_web_distance / scale);
            return new Vector2((float)calc_unityPos.x, (float)calc_unityPos.y);
        }

        /// <summary>
        /// 计算缩放后的unity坐标，假设基准点的方向dir(0,0,0)和web mercator方向相同
        /// </summary>
        /// <param name="centerUnityPos"> 中心点的unity坐标 x,z水平坐标</param>
        /// <param name="scale"></param>
        public Vector2 CalculateScaledUnityPos(Vector2d centerUnityPos, float scale)
        {
            //计算延长线上新点的位置
            if (scale == 0) return new Vector2((float)centerUnityPos.x, (float)centerUnityPos.y);
            calc_unityPos = centerUnityPos + (calc_WebMercatorNormal * calc_web_distance / scale);
            return new Vector2((float)calc_unityPos.x, (float)calc_unityPos.y);
        }

        /// <summary>
        /// 知道某个点的坐标，scale,中心点经纬度，计算出某个点经纬度
        /// </summary>
        /// <param name="centerUnityPos"></param>
        /// <param name="scale"></param>
        /// <param name="centerLonLat"></param>
        /// <returns></returns>
        public GeoCoordinateModel CalculateLonLat(Vector2d unityPos, GeoCoordinateModel centerLonLat, float scale)
        {
            //GeoCoordinateModel lonLat = new GeoCoordinateModel();
            //计算离web unity中心点的距离
            calc_web_distance = Vector2d.Distance(unityPos, centerLonLat.unityPos_world)*scale;
            // 计算方向向量
            calc_directionVector = unityPos - centerLonLat.unityPos_world;
            // 计算单位向量
            calc_WebMercatorNormal = calc_directionVector.normalized;
            //计算web摩卡托坐标
            WebMercatorPos = centerLonLat.WebMercatorPos + (calc_WebMercatorNormal * calc_web_distance);
            //计算经纬度
            calc_unityPos = unityPos;
            Vector2d lonLatPos = GeoUtils.WebMercatorToLonLat(WebMercatorPos.x, WebMercatorPos.y);
            longitude = lonLatPos.x;
            latitude = lonLatPos.y;
            LonLatStr = longitude.ToString() + "," + latitude.ToString();
            return this;
        }
    }
}