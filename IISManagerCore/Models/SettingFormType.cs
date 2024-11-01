﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IISManagerCore.Models
{
	/// <summary>
	/// Defines the the UI representation that a setting should have.
	/// </summary>
	public enum SettingFormType
	{
		/// <summary>
		/// A textbox.
		/// </summary>
		Textbox = 0,
		/// <summary>
		/// A checkbox.
		/// </summary>
		Checkbox = 1,
		/// <summary>
		/// A textarea.
		/// </summary>
		Textarea = 2
	}
}
