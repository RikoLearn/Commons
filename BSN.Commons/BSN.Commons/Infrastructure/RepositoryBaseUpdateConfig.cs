﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BSN.Commons.Infrastructure
{
	public abstract partial class RepositoryBase<T> where T : class
	{
		public class UpdateConfig : IUpdateConfig<T> 
		{
			public bool IncludeAllPropertiesEnabled
			{
				get
				{
					return _includeAllPropertiesEnabled;
				}
				private set
				{
					_includeAllPropertiesEnabled = value;
					if (value == true)
						_autoDetectChangedPropertiesEnabled = false;
				}
			}

			public bool AutoDetectChangedPropertiesEnabled
			{
				get
				{
					return _autoDetectChangedPropertiesEnabled;
				}
				private set
				{
					_autoDetectChangedPropertiesEnabled = value;
					if (value == true)
						_includeAllPropertiesEnabled = false;
				}
			}

			public IList<string> PropertyNames { get; }

			public UpdateConfig()
			{
				PropertyNames = new List<string>();
			}

			public IUpdateConfig<T> IncludeAllProperties()
			{
				IncludeAllPropertiesEnabled = true;
				return this;
			}

			public IUpdateConfig<T> AutoDetectChangedProperties()
			{
				AutoDetectChangedPropertiesEnabled = true;
				return this;
			}

			public IUpdateConfig<T> IncludeProperty<TProperty>(Expression<Func<T, TProperty>> propertyAccessExpression)
			{
				PropertyNames.Add(
					(propertyAccessExpression.Body as MemberExpression)?.Member.Name ??
						throw new ArgumentException(nameof(propertyAccessExpression))
				);

				AutoDetectChangedPropertiesEnabled = false;
				IncludeAllPropertiesEnabled = false;
				return this;
			}

			#region Fields

			private bool _includeAllPropertiesEnabled;

			private bool _autoDetectChangedPropertiesEnabled;

			#endregion
		}
    }
}
