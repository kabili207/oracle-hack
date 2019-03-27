/*
 *  Copyright © 2011-2018, Amy Nagle.
 *  All rights reserved.
 *
 *  This file is part of ZoraSharp.
 *
 *  ZoraSharp is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  ZoraSharp is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with ZoraSharp. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Zyrenth.Zora
{
	/// <summary>
	/// Extension methods used within the library
	/// </summary>
	public static class Extensions
	{
		internal static string Reverse(this string value)
		{
			return new string(Enumerable.Reverse(value).ToArray());
		}

		internal static string ReversedSubstring(this string value, int start, int length)
		{
			return new string(Enumerable.Reverse(value.Substring(start, length)).ToArray());
		}

		internal static string NullPad(this string value, int totalLength)
		{
			return ( value ?? "" ).TrimEnd().PadRight(totalLength, '\0').Substring(0, totalLength);
		}

		/// <summary>
		/// Reads the value of the dictionary in the specified key and casts it to the specified type.
		/// </summary>
		/// <typeparam name="T">The type to convert the stored value to</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value in the dictionary after it has been converted to the desired type</returns>
		[CLSCompliant(false)]
		public static T ReadValue<T>(this IDictionary<string, object> dictionary, string key) where T : IConvertible
		{
			Type type = typeof(T);
			var val = default(T);

			if (dictionary.ContainsKey(key))
			{
				object obj = dictionary[key];
				if (type.IsEnum)
				{
					// The T constraints on our method conflict with those on Enum.TryParse<T>
					// so we have to use some black magic instead.
					MethodInfo method = typeof(Enum).GetMethods().First(x => x.Name.StartsWith("TryParse") &&
						x.GetParameters().Length == 2);

					method = method.MakeGenericMethod(type);

					object[] args = new object[] { ReadValue<string>(dictionary, key), default(T) };
					method.Invoke(null, args);

					val = (T)args[1];
				}
				else
				{
					val = (T)Convert.ChangeType(obj, type);
				}
			}

			return val;
		}

		/// <summary>
		/// Gets the <see cref="System.Text.Encoding"/> associated with the specified <paramref name="region"/>
		/// </summary>
		/// <param name="region">The region of the game</param>
		/// <returns></returns>
		public static Encoding GetEncoding(this GameRegion region)
		{
			if (region == GameRegion.US)
			{
				return new USEncoding();
			}
			else
			{
				return new JapaneseEncoding();
			}
		}
	}
}
