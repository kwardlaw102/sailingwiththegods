using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CoordinateUtil
{
	//Easting (x) will always equal Unity X
	//Northing (y) will always equal Unity Z
	//Elevation (z) will always equal Unity Y
	static readonly Vector2 rasterMapOriginMeter = new Vector2(526320, 2179480);//in meters

	public const float unityWorldUnitResolution = 1193.920898f;//in meters
	const float unityOrigin = 0;//This will always be 0,0 for both x and y
	
	public static float ToMeters(float worldUnits) => worldUnits * unityWorldUnitResolution;
	public static float ToKilometers(float worldUnits) => ToMeters(worldUnits) / 1000f;

	/// <summary>
	/// Given coordinate must be stored like (x, height, z) where x and z are on the "world plane".
	/// the height will be unchanged from what is passed in.
	/// </summary>
	public static (Vector2, float height) ConvertUnityWorldToWGS1984(Vector3 unity_coordinate) {
		var (wtm, height) = Convert_UnityWorld_WebMercator(unity_coordinate);
		var wgs1984 = ConvertWebMercatorToWGS1984(wtm);
		return (wgs1984, height);
	}

	/// <summary>
	/// Given coordinate should be stored like (LongX, LatY)
	/// Result will put the given height in the y field of the result. the height will be unchanged from what is passed in.
	/// </summary>
	public static Vector3 ConvertWGS1984ToUnityWorld(Vector2 wgsLongLat, float height) {
		var wtm = ConvertWGS1984ToWebMercator(wgsLongLat);
		return Convert_WebMercator_UnityWorld(wtm, height);
	}


	/// <summary>
	/// Given coordinate should be stored like (WebMercX, WebMercY)
	/// Result will be in the form (convertedX, height, convertedY) in Unity world space
	/// </summary>
	public static Vector3 Convert_WebMercator_UnityWorld(Vector2 WTM_Coordinate, float height) {

		Vector3 convertedCoordinate;
		convertedCoordinate.x = (WTM_Coordinate.x - (rasterMapOriginMeter.x - unityOrigin)) / unityWorldUnitResolution;
		convertedCoordinate.y = height;
		convertedCoordinate.z = (WTM_Coordinate.y - (rasterMapOriginMeter.y - unityOrigin)) / unityWorldUnitResolution;
		return convertedCoordinate;
	}

	/// <summary>
	/// Given coordinate must be stored like (x, height, z) where x and z are on the "world plane".
	/// Result will put the given height in the z field of the result in case it's needed. the height will be unchanged from what is passed in.
	/// </summary>
	public static (Vector2, float height) Convert_UnityWorld_WebMercator(Vector3 unity_coordinate) {

		Vector2 convertedCoordinate;
		convertedCoordinate.x = (unity_coordinate.x * unityWorldUnitResolution) + (rasterMapOriginMeter.x - unityOrigin);
		convertedCoordinate.y = (unity_coordinate.z * unityWorldUnitResolution) + (rasterMapOriginMeter.y - unityOrigin);
		return (convertedCoordinate, unity_coordinate.y);
	}


	public static Vector2 ConvertWGS1984ToWebMercator(Vector2 WGSLongLat) {


		if ((Math.Abs(WGSLongLat.x) > 180 || Math.Abs(WGSLongLat.y) > 90))
			return Vector3.zero;

		double num = WGSLongLat.x * 0.017453292519943295;
		double x = 6378137.0 * num;
		double a = WGSLongLat.y * 0.017453292519943295;

		WGSLongLat.x = (float)x;
		WGSLongLat.y = (float)(3189068.5 * Math.Log((1.0 + Math.Sin(a)) / (1.0 - Math.Sin(a))));

		return WGSLongLat;

	}

	public static Vector2 ConvertWebMercatorToWGS1984(Vector2 MercatorEastNorth) {
		double mercatorX_lon = MercatorEastNorth.x;
		double mercatorY_lat = MercatorEastNorth.y;
		//if (Math.Abs(mercatorX_lon) < 180 && Math.Abs(mercatorY_lat) < 90)
		//		return Vector2.zero;

		//if ((Math.Abs(mercatorX_lon) > 20037508.3427892) )
		//TODO figure out what to do if the coordinate is OUTSIDE the extents of the projection
		//TODO	--E.G. if we sail through the north pole and start heading south--unity world coordinates need to reflect this
		//TODO	--We shouldn't need to worry about this unless we do globe circumnavigation--which we aren't
		//If we are outside the northern extent
		//if(Math.Abs(mercatorY_lat) > 20037508.3427892)
		//	mercatorY_lat = 20037508.3427892 - (mercatorY_lat - 20037508.3427892) - 1;
		//else if (Math.Abs(mercatorY_lat) < -20037508.3427892)

		double x = mercatorX_lon;
		double y = mercatorY_lat;
		double num3 = x / 6378137.0;
		double num4 = num3 * 57.295779513082323;
		double num5 = Math.Floor((double)((num4 + 180.0) / 360.0));
		double num6 = num4 - (num5 * 360.0);
		double num7 = 1.5707963267948966 - (2.0 * Math.Atan(Math.Exp((-1.0 * y) / 6378137.0)));
		mercatorX_lon = num6;
		mercatorY_lat = num7 * 57.295779513082323;

		return new Vector2((float)mercatorX_lon, (float)mercatorY_lat);
	}

	public static float GetDistanceBetweenTwoLatLongCoordinates(Vector2 lonXlatY_A, Vector2 lonXlatY_B) {
		//This distance formula uses a Haversine formula to calculate the distance
		//It assumes a spherical earth rather than ellipsoidal which can give distance errors of roughly 0.3%
		//	--source:www.movable-type.co.uk/scripts/latlong.html -- accessed May 24 2016
		//We need to make sure our angles are in radians
		float latA = Mathf.Deg2Rad * lonXlatY_A.y;
		float latB = Mathf.Deg2Rad * lonXlatY_B.y;
		float changeOfLat = (lonXlatY_B.y - lonXlatY_A.y) * Mathf.Deg2Rad;
		float changeOfLon = (lonXlatY_B.x - lonXlatY_A.x) * Mathf.Deg2Rad;
		float earthRadius = 6371000f; // in meters

		//This is the Haversine implementation
		float a = Mathf.Pow(Mathf.Sin(changeOfLat / 2), 2) +
			Mathf.Cos(latA) * Mathf.Cos(latB) *
			Mathf.Pow(Mathf.Sin(changeOfLon / 2), 2);
		float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
		float distance = earthRadius * c;

		//return the distance
		return distance;
	}
}
