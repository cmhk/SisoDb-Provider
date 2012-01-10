﻿using System;
using System.Collections.Generic;
using NCore.Reflections;
using PineCone.Structures;
using SisoDb.DbSchema;

namespace SisoDb.Dac.BulkInserts
{
    public class IndexesReader : SingleResultReaderBase<IStructureIndex>
    {
        private bool ValueIsConsumedForCurrent { get; set; }

        public IndexesReader(IndexStorageSchema storageSchema, IEnumerable<IStructureIndex> items)
            : base(storageSchema, items)
        {
        }

        public override bool Read()
        {
            ValueIsConsumedForCurrent = false;

            return base.Read();
        }

        public override object GetValue(int ordinal)
        {
            if (ValueIsConsumedForCurrent)
                return DBNull.Value;

            if (ordinal == 0)
                return Enumerator.Current.StructureId.Value;

            var schemaField = StorageSchema[ordinal];
            
            if (schemaField.Name == IndexStorageSchema.Fields.MemberPath.Name)
                return Enumerator.Current.Path;
            
            var dataType = Enumerator.Current.DataType;

            if (schemaField.Name == IndexStorageSchema.Fields.StringValue.Name && (dataType.IsStringType() || dataType.IsAnyEnumType()))
            {
                ValueIsConsumedForCurrent = true;
                return Enumerator.Current.Value;
            }

			if (schemaField.Name == IndexStorageSchema.Fields.StringValue.Name && !(dataType.IsStringType() || dataType.IsAnyEnumType()))
			{
				return SisoEnvironment.StringConverter.AsString(Enumerator.Current.Value);
			}

            if (schemaField.Name == IndexStorageSchema.Fields.IntegerValue.Name && dataType.IsAnyIntegerNumberType())
            {
                ValueIsConsumedForCurrent = true;
                return Enumerator.Current.Value;
            }

            if (schemaField.Name == IndexStorageSchema.Fields.FractalValue.Name && dataType.IsAnyFractalNumberType())
            {
                ValueIsConsumedForCurrent = true;
                return Enumerator.Current.Value;
            }

            if (schemaField.Name == IndexStorageSchema.Fields.DateTimeValue.Name && dataType.IsAnyDateTimeType())
            {
                ValueIsConsumedForCurrent = true;
                return Enumerator.Current.Value;
            }

            if (schemaField.Name == IndexStorageSchema.Fields.BoolValue.Name && dataType.IsAnyBoolType())
            {
                ValueIsConsumedForCurrent = true;
                return Enumerator.Current.Value;
            }

            if (schemaField.Name == IndexStorageSchema.Fields.GuidValue.Name && dataType.IsAnyGuidType())
            {
                ValueIsConsumedForCurrent = true;
                return Enumerator.Current.Value;
            }

            return DBNull.Value;
        }
    }
}