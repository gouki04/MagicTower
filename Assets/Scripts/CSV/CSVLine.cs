using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mt
{
    /// <summary>
    /// csv表的行
    /// </summary>
	public class CSVLine
	{
	    private CSVTable mParent;
	    private string[] mValues;
	
	    public CSVLine(CSVTable parent, string[] values)
	    {
	        mParent = parent;
	        mValues = values;
	    }

        public int GetIntValue(string key)
        {
            return Convert.ToInt32(this[key]);
        }

        public uint GetUIntValue(string key)
        {
            return Convert.ToUInt32(this[key]);
        }

        public float GetFloatValue(string key)
        {
            return Convert.ToSingle(this[key]);
        }

        public bool GetBoolValue(string key)
        {
            return Convert.ToBoolean(this[key]);
        }

        public string GetStringValue(string key)
        {
            return this[key];
        }
	
	    public string this[string Key]
	    {
	        get
	        {
	            var idx = mParent.GetHeaderIndex(Key);
	            return mValues[idx];
	        }
	    }
	}
}
