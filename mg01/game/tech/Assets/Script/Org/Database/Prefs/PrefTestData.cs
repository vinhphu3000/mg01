/* ==============================================================================
 * SecurePrefData
 * @author jr.zeng
 * 2016/8/25 11:41:41
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class PrefTestData
{
    public bool isBool = false;
    public int isInt = 10;
    public uint isUInt = 20;
    public float isFloat = 100f;
    public long isLong = 1001L;
    public double isDouble = 1002D;

    public string isString = null;

    public Color isColor = Color.green;
    public Vector3 isVector3 = Vector3.down;

    public byte[] isByteArray = null;
    public Quaternion isQuaternion = Quaternion.identity;

    public mg.org.SubjectEvent isSubject = new mg.org.SubjectEvent("222", 111);
        
    public PrefTestData()
    {

    }
}
