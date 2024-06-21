using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATGC.GEO
{
    public class GeoUtils
    {
        /// <summary>
        /// 地球半径乘以π，用于Web墨卡托投影计算
        /// </summary>
        public const double OriginShift = 2 * Math.PI * EarthRadiusMean / 2.0;
        /// <summary>
        /// 地球平均半径，单位：米
        /// </summary>
        public const float EarthRadiusMean = 6371000f; // 地球半径，单位：米
        /// <summary>
        /// 地球赤道的半径，单位：米
        /// </summary>
        public const float EarthRadiusEquator = 6378137f; // 地球赤道的半径，单位：米
        /// <summary>
        /// 将经纬度坐标转换为Web墨卡托投影坐标
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static Vector2d LonLatToWebMercator(double lon, double lat)
        {
            // 经度转换
            double x = lon * OriginShift / 180.0;

            // 纬度转换
            //double latRad = lat * Math.PI / 180.0;
            double y = Math.Log(Math.Tan((90 + lat) * Math.PI / 360.0)) / (Math.PI / 180.0);
            y = y * OriginShift / 180.0;

            return new Vector2d(x, y);
        }

        /// <summary>
        /// 将Web墨卡托投影坐标转换为经纬度坐标,缩放比例为1
        /// 注意：经纬度坐标和Web墨卡托投影坐标的坐标系不一样，Web墨卡托投影坐标的中心点在经线的正北方，经度范围为[-20037508.34, 20037508.34]，纬度范围为[-180, 85.0511287798066]
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="centerPoint">中心点</param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Vector2d GeoToWorldPositionLongLat(double lon, double lat, Vector2d centerPoint, float scale = 1)
        {
            var posx = lon * OriginShift / 180.0;
            var posy = Math.Log(Math.Tan((90 + lat) * Math.PI / 360.0)) / (Math.PI / 180.0);
            posy = posy * OriginShift / 180.0;
            return new Vector2d((posx - centerPoint.x) * scale, (posy - centerPoint.y) * scale);
        }

        /// <summary>
        /// 将经纬度坐标转换为Web墨卡托投影坐标
        /// 注意：经纬度坐标和Web墨卡托投影坐标的坐标系不一样，Web墨卡托投影坐标的中心点在经线的正北方
        /// </summary>
        /// <summary>
        /// 将Web墨卡托投影坐标转换为经纬度坐标
        /// </summary>
        /// <param name="x">Web墨卡托投影的X坐标</param>
        /// <param name="y">Web墨卡托投影的Y坐标</param>
        /// <returns>经纬度坐标</returns>
        public static Vector2d WebMercatorToLonLat(double x, double y)
        {
            double lon = (x / OriginShift) * 180.0;
            // 反向转换纬度
            double lat = (180 / Math.PI * (2 * Math.Atan(Math.Exp(y / OriginShift * Math.PI)) - Math.PI / 2));
            return new Vector2d(lon, lat);
        }
    }
}