using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATGC.GEO
{
    /// <summary>
    /// ��������ģ��
    /// </summary>
    [Serializable]
    public class GeoCoordinateModel
    {
        /// <summary>
        /// γ��
        /// </summary>
        public double latitude = 34.05f;   // γ��
        /// <summary>
        /// ����
        /// </summary>
        public double longitude = -118.25f; // ����

        /// <summary>
        /// ī����ͶӰ����
        /// </summary>
        public Vector2d WebMercatorPos;
        /// <summary>
        /// �����web����
        /// </summary>
        public double calc_web_distance;
        /// <summary>
        /// ���㵥λ����
        /// </summary>
        public Vector2d calc_directionVector;
        /// <summary>
        /// �����web��γ��
        /// </summary>
        public Vector2d calc_WebMercatorNormal;
        /// <summary>
        /// �����unity����
        /// </summary>
        public Vector2d calc_unityPos;
        /// <summary>
        /// unity����������
        /// </summary>
        public Vector2d unityPos_world;
        /// <summary>
        /// unity�ı�������
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
                //�ֽ����γ��
                string[] strs = LonLatStr.Split(',');
                if (strs.Length == 2)
                {
                    double.TryParse(strs[0], out longitude);
                    double.TryParse(strs[1], out latitude);
                }
            }
        }

        /// <summary>
        /// ��������unity����
        /// </summary>
        /// <param name="unityPos"></param>
        public void SetWorldUnityPos(Vector2 unityPos)
        {
            unityPos_world = new Vector2d(unityPos.x, unityPos.y);
        }
        
        /// <summary>
        /// ���ñ���unity����
        /// </summary>
        /// <param name="unityPos"></param>
        public void SetLocalUnityPos(Vector2 unityPos)
        {
            unityPos_local = new Vector2d(unityPos.x, unityPos.y);
        }

        /// <summary>
        /// ����ī����ͶӰ����
        /// </summary>
        public void CalculateWebMercatorPos()
        {
            WebMercatorPos = GeoUtils.LonLatToWebMercator(longitude, latitude);
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="centerPos"></param>
        /// <returns></returns>
        public Vector2d GetRelativePos(Vector2d centerPos)
        {
            return new Vector2d(WebMercatorPos.x - centerPos.x, WebMercatorPos.y - centerPos.y);
        }

        /// <summary>
        /// CalculateInfo - ������Ϣ
        /// </summary>
        public void CalculateInfo(Vector2d centerWebMercatorPos)
        {
            //���������ĵ�ľ���
            calc_web_distance = Vector2d.Distance(WebMercatorPos, centerWebMercatorPos);
            // ���㷽������
            calc_directionVector = WebMercatorPos - centerWebMercatorPos;
            // ���㵥λ����
            calc_WebMercatorNormal = calc_directionVector.normalized;

        }

        /// <summary>
        /// �������ź��unity���꣬�����׼��ķ���dir(0,0,0)��web mercator������ͬ
        /// </summary>
        /// <param name="centerUnityPos"> ���ĵ��unity���� x,zˮƽ����</param>
        /// <param name="scale"></param>
        public Vector2 CalculateScaledUnityPos(Vector2 centerUnityPos, float scale)
        {
            //�����ӳ������µ��λ��
            if (scale == 0) return centerUnityPos;
            Vector2d centerWebMercatorPos = new Vector2d(centerUnityPos.x, centerUnityPos.y);
            calc_unityPos = centerWebMercatorPos + (calc_WebMercatorNormal * calc_web_distance / scale);
            return new Vector2((float)calc_unityPos.x, (float)calc_unityPos.y);
        }

        /// <summary>
        /// �������ź��unity���꣬�����׼��ķ���dir(0,0,0)��web mercator������ͬ
        /// </summary>
        /// <param name="centerUnityPos"> ���ĵ��unity���� x,zˮƽ����</param>
        /// <param name="scale"></param>
        public Vector2 CalculateScaledUnityPos(Vector2d centerUnityPos, float scale)
        {
            //�����ӳ������µ��λ��
            if (scale == 0) return new Vector2((float)centerUnityPos.x, (float)centerUnityPos.y);
            calc_unityPos = centerUnityPos + (calc_WebMercatorNormal * calc_web_distance / scale);
            return new Vector2((float)calc_unityPos.x, (float)calc_unityPos.y);
        }

        /// <summary>
        /// ֪��ĳ��������꣬scale,���ĵ㾭γ�ȣ������ĳ���㾭γ��
        /// </summary>
        /// <param name="centerUnityPos"></param>
        /// <param name="scale"></param>
        /// <param name="centerLonLat"></param>
        /// <returns></returns>
        public GeoCoordinateModel CalculateLonLat(Vector2d unityPos, GeoCoordinateModel centerLonLat, float scale)
        {
            //GeoCoordinateModel lonLat = new GeoCoordinateModel();
            //������web unity���ĵ�ľ���
            calc_web_distance = Vector2d.Distance(unityPos, centerLonLat.unityPos_world)*scale;
            // ���㷽������
            calc_directionVector = unityPos - centerLonLat.unityPos_world;
            // ���㵥λ����
            calc_WebMercatorNormal = calc_directionVector.normalized;
            //����webĦ��������
            WebMercatorPos = centerLonLat.WebMercatorPos + (calc_WebMercatorNormal * calc_web_distance);
            //���㾭γ��
            calc_unityPos = unityPos;
            Vector2d lonLatPos = GeoUtils.WebMercatorToLonLat(WebMercatorPos.x, WebMercatorPos.y);
            longitude = lonLatPos.x;
            latitude = lonLatPos.y;
            LonLatStr = longitude.ToString() + "," + latitude.ToString();
            return this;
        }
    }
}