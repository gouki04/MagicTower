using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Csv
{
	static private Csv mInstance;
	static public Csv Instance
	{
		get 
		{
			if (mInstance == null)
				mInstance = new Csv();

			return mInstance;
		}
	}

	public Csv()
	{
		mCSVs = new Dictionary<string, CsvTable> ();
	}

	public class CsvTable
	{
		public class CsvLine
		{
			private CsvTable mParent;
			private string[] mValues;

			public CsvLine(CsvTable parent, string[] values)
			{
				mParent = parent;
				mValues = values;
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

		private Dictionary<string, uint> mHeaders;

		private Dictionary<uint, CsvLine> mValues;

		public CsvTable()
		{

		}

		public void SetHeaders(string[] headers)
		{
			mHeaders = new Dictionary<string, uint> ();
			for (int i = 0; i < headers.Length; ++i) {
				mHeaders[headers[i]] = (uint)i;
			}
		}

		public void AddLine(string[] line)
		{
			if (mValues == null)
				mValues = new Dictionary<uint, CsvLine> ();

			var csvline = new CsvLine(this, line);
			var id = Convert.ToUInt32(csvline["id"]);
			mValues [id] = csvline;
		}

		public CsvLine this[uint id]
		{
			get
			{
				return mValues[id];
			}
		}

		public uint GetHeaderIndex(string headerName)
		{
			return mHeaders [headerName];
		}
	}

	public CsvTable Open(string filename)
	{
		var asset = Resources.Load (filename) as TextAsset;
		var lines = asset.text.Split (new char[]{'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		var dict = new CsvTable ();
		var headers = lines [0].Split (new char[] {','});
		dict.SetHeaders (headers);

		for (int i = 1; i < lines.Length; ++i)
		{
			dict.AddLine(lines[i].Split(new char[] {','}));
		}

		return dict;
	}

	private Dictionary<string, CsvTable> mCSVs;

	public CsvTable GetCsv(string name)
	{
		CsvTable csv = null;
		if (!mCSVs.TryGetValue (name, out csv)) {
			csv = Open(name);
			mCSVs[name] = csv;
		}

		return csv;
	}

	public CsvTable this[string csvName]
	{
		get { return GetCsv(csvName); }
	}
}
