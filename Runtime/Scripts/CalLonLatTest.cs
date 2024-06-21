using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace ATGC.GEO
{
    public class CalLonLatTest : MonoBehaviour
    {
        public Vector2d myPos = new Vector2d(116.397428, 39.90923);
        public GeoCoordinateModel lonLat = new GeoCoordinateModel();
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            myPos.x = transform.position.x;
            myPos.y = transform.position.z;
            lonLat.CalculateLonLat(myPos, GeoLocateMgr.Instance.CenterLocaterObj.m_GeoCoordinateModel, GeoLocateMgr.Instance.Scale);
        }
    }
}