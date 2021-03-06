﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage;

namespace Pandorax.EntityFrameworkCore.FileMaker.Storage.Internal
{
    public class FileMakerTypeMappingSource : RelationalTypeMappingSource
    {
        private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings
            = new Dictionary<Type, RelationalTypeMapping>
            {
                [typeof(string)] = new StringTypeMapping("VARCHAR"),
                [typeof(int)] = new IntTypeMapping("INT"),
                [typeof(DateTime)] = new DateTimeTypeMapping("DATE"),
                [typeof(double)] = new DoubleTypeMapping("DECIMAL"),
            };

        private readonly Dictionary<string, RelationalTypeMapping> _storeTypeMappings
            = new Dictionary<string, RelationalTypeMapping>
            {
            };

        public FileMakerTypeMappingSource(
            TypeMappingSourceDependencies dependencies,
            RelationalTypeMappingSourceDependencies relationalDependencies)
            : base(dependencies, relationalDependencies)
        {
        }

        protected override RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
        {
            var clrType = mappingInfo.ClrType;
            if (clrType != null
                && _clrTypeMappings.TryGetValue(clrType, out var mapping))
            {
                return mapping;
            }

            var storeTypeName = mappingInfo.StoreTypeName;
            if (storeTypeName != null
                && _storeTypeMappings.TryGetValue(storeTypeName, out mapping))
            {
                return mapping;
            }

            mapping = base.FindMapping(mappingInfo);

            return mapping;
        }
    }
}
