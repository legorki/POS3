﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Grouping.Model
{
    public class Exam
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Exam() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [MaxLength(8)]
        public string SchoolclassId { get; set; }       // FK Feld
        [JsonIgnore]
        [CsvHelper.Configuration.Attributes.Ignore]
        public Schoolclass Schoolclass { get; set; }   // FK Navigation
        [MaxLength(8)]
        public string TeacherId { get; set; }
        [JsonIgnore]
        [CsvHelper.Configuration.Attributes.Ignore]
        public Teacher Teacher { get; set; }
        [MaxLength(8)]
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public int PeriodNr { get; set; }
        [JsonIgnore]
        [CsvHelper.Configuration.Attributes.Ignore]
        public Period Period { get; set; }
    }
}
