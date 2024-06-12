# LonLatToUnity

将经纬度坐标转换为Web墨卡托投影坐标,再转换为unity内某个位置的相对坐标（Convert the earth's longitude and latitude coordinates to unity coordinates）

```bash
git clone https://github.com/atgczcl/LonLatToUnity.git
```

> 导入包：
>
> unitypackage manager add git url:
>
> <https://github.com/atgczcl/LonLatToUnity.git#upm>

Use:

```csharp
/// <summary>
/// Calculates Unity coordinates based on a string containing longitude and latitude.
/// </summary>
/// <param name="lonLatStr">A string representing the longitude and latitude.</param>
/// <returns>A Vector2 representing the calculated position in Unity coordinates.</returns>
public Vector2 CalculateUnityPos(string lonLatStr)
{
    // Initialize the GeoCoordinateModel with the provided longitude and latitude string.
    GeoCoordinateModel geoCoordinateModel = new GeoCoordinateModel(lonLatStr);

    // Get the Unity coordinates of the center point.
    Vector2 centerUnityPos = new Vector2(CenterLocaterObj.transform.position.x, CenterLocaterObj.transform.position.z);
    
    // Compute the scaled Unity coordinates.
    var scaledUnityPos = geoCoordinateModel.CalculateScaledUnityPos(centerUnityPos, Scale);
    
    return scaledUnityPos;
}


```

