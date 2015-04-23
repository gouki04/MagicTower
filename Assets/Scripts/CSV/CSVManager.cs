using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Utils
{
    /// <summary>
    /// csv数据表的管理器
    /// </summary>
	public class CSVManager : Singleton<CSVManager>
	{
	    private Dictionary<string, CSVTable> mCSVs;
	    
	    public CSVManager()
		{
			mCSVs = new Dictionary<string, CSVTable> ();
		}
	
        /// <summary>
        /// 打开一个csv表
        /// 打开后就可以通过GetCsv获取到
        /// </summary>
        /// <param name="csv_name">csv的名字</param>
        /// <returns></returns>
		public CSVTable Open(string csv_name)
		{
			var asset = Resources.Load (csv_name) as TextAsset;
			var lines = asset.text.Split (new char[]{'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
			var dict = new CSVTable ();
			var headers = lines [0].Split (new char[] {','});
			dict.SetHeaders (headers);
	
			for (int i = 1; i < lines.Length; ++i)
			{
				dict.AddLine(lines[i].Split(new char[] {','}));
			}
	
			return dict;
		}
	
        /// <summary>
        /// 获取一个csv表
        /// </summary>
        /// <param name="csv_name"></param>
        /// <returns></returns>
		public CSVTable GetCSV(string csv_name)
		{
			CSVTable csv = null;
			if (!mCSVs.TryGetValue (csv_name, out csv)) {
				csv = Open("config/" + csv_name);
				mCSVs[csv_name] = csv;
			}
	
			return csv;
		}
	
        /// <summary>
        /// 获取一个csv表
        /// </summary>
        /// <param name="csv_name"></param>
        /// <returns></returns>
		public CSVTable this[string csv_name]
		{
			get { return GetCSV(csv_name); }
		}
	}
}
