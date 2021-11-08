﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Relational.BaseModels.AspNetCore.Generics.Annotations
{
    public class FieldLengthModel
    {
        public FieldLengthModel(int maximumLength, string id, int? minimumLength)
        {

            ID = id.FirstLetterToLower();
            MaximumLength = maximumLength;
            MinimumLength = minimumLength;
        }
        public string ID { get; set; }
        public int? MinimumLength { get; }
        public int MaximumLength { get; }
    }
}