using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public static class helper 
{ 
    private static PointerEventData _eventDdataCurrentPosition;
    private static List<RaycastResult> _results;
    public static bool IsOverUI(){
        _eventDdataCurrentPosition = new PointerEventData(EventSystem.current) { position =  Input.mousePosition} ;
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDdataCurrentPosition , _results);
        return _results.Count > 0 ;
    }

    public static IEnumerable<T> GetValues<T>() {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static Vector2 getRandomForce(Vector2 centerDirection,Vector2Int scaleRange , Vector2 angleRange ){
        int scale  =  UnityEngine.Random.Range(scaleRange.x, scaleRange.y)  ;
        Vector2 force = scale*centerDirection;
        float r = UnityEngine.Random.Range(angleRange.x, angleRange.y) ;
        force = Quaternion.AngleAxis(r, Vector3.forward) * force;
        return force;
    }

    public static Dictionary<TKey, TValue> DeepCloneDictionary<TKey, TValue>
    (Dictionary<TKey, TValue> original) 
    {
        Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>();
        foreach (KeyValuePair<TKey, TValue> entry in original)
        {
            ret.Add(entry.Key, entry.Value);
        }
        return ret;
    }

    public static List<T> DeepCloneList<T>
    (List<T> original) 
    {
        List<T> ret = new List<T>();
        foreach ( T entry in original)
        {
            ret.Add(entry);
        }
        return ret;
    }
    /* 
    foreach( EnemyType t in l){
        killedEnemy[t] = 0 ;
        currentEnemy[t] = 0;
    }
    */
    
}
