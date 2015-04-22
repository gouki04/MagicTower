using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mt
{
    /// <summary>
    /// csv表
    /// </summary>
    public class CSVTable : Dictionary<uint, CSVLine>
	{
        /// <summary>
        /// 表头
        /// </summary>
	    private Dictionary<string, uint> mHeaders;
	
	    public CSVTable()
	    {
	
	    }
	
        /// <summary>
        /// 设置表头
        /// </summary>
        /// <param name="headers"></param>
	    public void SetHeaders(string[] headers)
	    {
	        mHeaders = new Dictionary<string, uint>();
	        for (int i = 0; i < headers.Length; ++i)
	        {
	            mHeaders[headers[i]] = (uint)i;
	        }
	    }
	
        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="line"></param>
	    public void AddLine(string[] line)
	    {	
	        var csv_line = new CSVLine(this, line);
	        var id = Convert.ToUInt32(csv_line["id"]);

            Add(id, csv_line);
	    }

        public CSVLine this[uint id]
        {
            get { return base[id]; }
            protected set { base[id] = value; }
	    }
	
        /// <summary>
        /// 获取表头名的idx
        /// </summary>
        /// <param name="header_name"></param>
        /// <returns></returns>
	    public uint GetHeaderIndex(string header_name)
	    {
	        return mHeaders[header_name];
	    }
	}
}
