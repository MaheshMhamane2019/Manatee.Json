﻿/***************************************************************************************

	Copyright 2012 Greg Dennis

	   Licensed under the Apache License, Version 2.0 (the "License");
	   you may not use this file except in compliance with the License.
	   You may obtain a copy of the License at

		 http://www.apache.org/licenses/LICENSE-2.0

	   Unless required by applicable law or agreed to in writing, software
	   distributed under the License is distributed on an "AS IS" BASIS,
	   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	   See the License for the specific language governing permissions and
	   limitations under the License.
 
	File Name:		PrimitiveMapper.cs
	Namespace:		Manatee.Json.Serialization.Helpers
	Class Name:		PrimitiveMapper
	Purpose:		Provides type-safe generic casting with additional functionality.

***************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manatee.Json;
using Manatee.Json.Serialization.Exceptions;

namespace Manatee.Json.Serialization.Helpers
{
	internal static class PrimitiveMapper
	{
		public static JsonValue MapToJson<T>(T obj)
		{
			if (obj is string)
				return new JsonValue(ObjectCaster.Cast<string>(obj));
			if (obj is bool)
				return new JsonValue(ObjectCaster.Cast<bool>(obj));
			if (obj is Enum)
				return new JsonValue(ObjectCaster.Cast<int>(obj));
			double result;
			return ObjectCaster.TryCast(obj, out result) ? result : JsonValue.Null;
		}

		public static T MapFromJson<T>(JsonValue json)
		{
			if (!IsPrimitive(typeof(T)))
			    throw new NotPrimitiveTypeException(typeof(T));
			var value = json.GetValue();
			T result;
			ObjectCaster.TryCast(value, out result);
			return result;
		}

		public static bool IsPrimitive(Type type)
		{
			return (type == typeof (string)) || type.IsPrimitive;
		}

		private static object GetValue(this JsonValue json)
		{
			switch (json.Type)
			{
				case JsonValueType.Number:
					return json.Number;
				case JsonValueType.String:
					return json.String;
				case JsonValueType.Boolean:
					return json.Boolean;
				case JsonValueType.Null:
					return null;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
