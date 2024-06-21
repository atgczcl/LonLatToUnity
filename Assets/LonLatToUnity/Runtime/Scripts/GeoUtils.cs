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
        /// ����뾶���ԦУ�����Webī����ͶӰ����
        /// </summary>
        public const double OriginShift = 2 * Math.PI * EarthRadiusMean / 2.0;
        /// <summary>
        /// ����ƽ���뾶����λ����
        /// </summary>
        public const float EarthRadiusMean = 6371000f; // ����뾶����λ����
        /// <summary>
        /// �������İ뾶����λ����
        /// </summary>
        public const float EarthRadiusEquator = 6378137f; // �������İ뾶����λ����
        /// <summary>
        /// ����γ������ת��ΪWebī����ͶӰ����
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static Vector2d LonLatToWebMercator(double lon, double lat)
        {
            // ����ת��
            double x = lon * OriginShift / 180.0;

            // γ��ת��
            //double latRad = lat * Math.PI / 180.0;
            double y = Math.Log(Math.Tan((90 + lat) * Math.PI / 360.0)) / (Math.PI / 180.0);
            y = y * OriginShift / 180.0;

            return new Vector2d(x, y);
        }

        /// <summary>
        /// ��Webī����ͶӰ����ת��Ϊ��γ������,���ű���Ϊ1
        /// ע�⣺��γ�������Webī����ͶӰ���������ϵ��һ����Webī����ͶӰ��������ĵ��ھ��ߵ������������ȷ�ΧΪ[-20037508.34, 20037508.34]��γ�ȷ�ΧΪ[-180, 85.0511287798066]
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="centerPoint">���ĵ�</param>
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
        /// ����γ������ת��ΪWebī����ͶӰ����
        /// ע�⣺��γ�������Webī����ͶӰ���������ϵ��һ����Webī����ͶӰ��������ĵ��ھ��ߵ�������
        /// </summary>
        /// <summary>
        /// ��Webī����ͶӰ����ת��Ϊ��γ������
        /// </summary>
        /// <param name="x">Webī����ͶӰ��X����</param>
        /// <param name="y">Webī����ͶӰ��Y����</param>
        /// <returns>��γ������</returns>
        public static Vector2d WebMercatorToLonLat(double x, double y)
        {
            double lon = (x / OriginShift) * 180.0;
            // ����ת��γ��
            double lat = (180 / Math.PI * (2 * Math.Atan(Math.Exp(y / OriginShift * Math.PI)) - Math.PI / 2));
            return new Vector2d(lon, lat);
        }
    }
}