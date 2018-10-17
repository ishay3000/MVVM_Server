using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvvm_Server.Models
{
	static class Extentions
	{
		/// <summary>
		/// Extentiosn method for getting the string value of a bytes array
		/// </summary>
		/// <param name="buffer">a bytes array buffer</param>
		/// <returns>the string contained in the bytes buffer</returns>
		public static string GetStringValue(this byte[] buffer)
		{
			string result = Encoding.UTF8.GetString(buffer);
			return result;
		}
	}
}
