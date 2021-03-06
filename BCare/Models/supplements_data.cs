﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCare.Models
{
    public class supplements_data
    {
        //primary key
        public int BoaID { get; set; }
        //foreign key//primary key
        public int SomID { get; set; }
        public Double Value { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
    }
    public enum MeasurementUnit
    {
        gram,
        milligram,
        microgram,
        units,
        KU,
        milliliter,
        millimole,
        NotInList
    }
}
