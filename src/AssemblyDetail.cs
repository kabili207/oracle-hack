/*
 *  Copyright © 2013-2018, Amy Nagle.
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
using System.Reflection;

namespace Zyrenth.Zora
{
	/// <summary>
	/// Provides quick access to details of an assembly
	/// </summary>
	public class AssemblyDetail
	{
		private readonly Assembly _asm;

		/// <summary>
		/// Gets the assembly company
		/// </summary>
		public string Company => GetAttribute<AssemblyCompanyAttribute>(x => x.Company);

		/// <summary>
		/// Gets the assembly copyright information
		/// </summary>
		public string Copyright => GetAttribute<AssemblyCopyrightAttribute>(x => x.Copyright);

		/// <summary>
		/// Gets the description about the assembly.
		/// </summary>
		public string Description => GetAttribute<AssemblyDescriptionAttribute>(x => x.Description);

		/// <summary>
		/// Gets the assembly's file version
		/// </summary>
		public string FileVersion => _asm.GetName().Version?.ToString() ?? "0.0.0.0";

		/// <summary>
		///  Gets the assembly's full name.
		/// </summary>
		public string Product => GetAttribute<AssemblyProductAttribute>(x => x.Product);

		/// <summary>
		/// Gets the assembly's version.
		/// </summary>
		public string ProductVersion => GetAttribute<AssemblyInformationalVersionAttribute>(x => x.InformationalVersion) ?? FileVersion;

		/// <summary>
		/// Gets the assembly title
		/// </summary>
		public string Title => GetAttribute<AssemblyTitleAttribute>(a => a.Title);

		/// <summary>
		/// Gets the assembly trademark information
		/// </summary>
		public string Trademark => GetAttribute<AssemblyTrademarkAttribute>(a => a.Trademark);

		/// <summary>
		/// Creates a new AssemblyDetail object from the specified Assembly
		/// </summary>
		/// <param name="asm">The assembly</param>
		public AssemblyDetail(Assembly asm)
		{
			if (asm is null)
			{
				throw new ArgumentNullException(nameof(asm));
			}

			_asm = asm;
		}

		private string GetAttribute<T>(Func<T, string> resolveFunc) where T : Attribute
		{
			object[] attribs = _asm.GetCustomAttributes(typeof(T), true);
			if (attribs.Length > 0)
			{
				return resolveFunc((T)attribs[0]);
			}
			return null;
		}
	}
}
