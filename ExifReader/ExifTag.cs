namespace ExifReader
{
	public sealed class ExifTag
	{
		private readonly int _id;
		private readonly string _description;
		private readonly string _fieldName;

		public int Id
		{
			get
			{
				return _id;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
		}

		public string FieldName
		{
			get
			{
				return _fieldName;
			}
		}

		public string Value { get; set; }

		public ExifTag(int id, string fieldName, string description)
		{
			_id = id;
			_description = description;
			_fieldName = fieldName;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1}) = {2}", Description, FieldName, Value);
		}
	}
}