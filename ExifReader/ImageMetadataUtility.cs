using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using PhotoManager.Core.Helpers.Media;

namespace ExifReader
{
	public class ImageMetadataUtility
	{
		#region Fields

        private readonly Image _image;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string this[PropertyId id]
        {
            get { return GetMetadata(id); }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetMetadata(PropertyId id)
        {
            string value = string.Empty;

            if (_imageProps.ContainsKey(id))
            {
                value = GetValueByType(_imageProps[id]);
            }
            return value;
        }

        private string GetValueByType(KeyValuePair<PropertyType, object> imageProp)
        {
            PropertyType type = imageProp.Key;
            switch (type)
            {
                case PropertyType.ASCII:
                    return Convert.ToString(imageProp.Value);
                case PropertyType.Int16:
                    return Convert.ToInt16(imageProp.Value).ToString(CultureInfo.InvariantCulture);
                case PropertyType.SLONG:
                case PropertyType.Int32:
                    return Convert.ToInt32(imageProp.Value).ToString(CultureInfo.InvariantCulture);
                case PropertyType.SRational:
                case PropertyType.Rational:
                    return Convert.ToDecimal(imageProp.Value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                default:
                    return string.Empty;
            }
        }

        private Dictionary<PropertyId, KeyValuePair<PropertyType, Object>> _imageProps;

        #region .ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
		public ImageMetadataUtility(Image image)
        {
            this._image = image;
            ReadImageMetaData();
        }

		public ImageMetadataUtility(byte[] image)
		{
			MemoryStream ms = new MemoryStream(image);
			this._image = Image.FromStream(ms);
			ReadImageMetaData();
		}

        #endregion


        private void ReadImageMetaData()
        {
            _imageProps = new Dictionary<PropertyId, KeyValuePair<PropertyType, object>>();

            foreach (PropertyItem property in _image.PropertyItems)
            {
                Object propValue = new Object();
                PropertyType type = (PropertyType)property.Type;
                switch (type)
                {
                    case PropertyType.ASCII:
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        propValue = encoding.GetString(property.Value, 0, property.Len - 1);
                        break;
                    case PropertyType.Int16:
                        propValue = BitConverter.ToInt16(property.Value, 0);
                        break;
                    case PropertyType.SLONG:
                    case PropertyType.Int32:
                        propValue = BitConverter.ToInt32(property.Value, 0);
                        break;
                    case PropertyType.SRational:
                    case PropertyType.Rational:
                        UInt32 numberator = BitConverter.ToUInt32(property.Value, 0);
                        UInt32 denominator = BitConverter.ToUInt32(property.Value, 4);
                        try
                        {
                            propValue = ((double)numberator / denominator).ToString(CultureInfo.InvariantCulture);
                            if (propValue.ToString() == "NaN")
                                propValue = "0";
                        }
                        catch (DivideByZeroException)
                        {
                            propValue = "0";
                        }
                        break;
                    case PropertyType.Undefined:
                        propValue = "Undefined Data";
                        break;
                }
                _imageProps.Add(NumToEnum<PropertyId>(property.Id), new KeyValuePair<PropertyType, object>(NumToEnum<PropertyType>(property.Type), propValue));
            }
        }

        private T NumToEnum<T>(int number)
        {
            return (T)Enum.ToObject(typeof(T), number);
        }
	}
}
