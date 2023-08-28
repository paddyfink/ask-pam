//using AskPam.Domain.Entities;
//using AskPam.Domain.Entities.Auditing;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Text;

//namespace AskPam.Crm.Features
//{
//    public abstract class FeatureSetting : Entity<long>, ICreationAudited
//    {
//        /// <summary>
//        /// Maximum length of the <see cref="Name"/> field.
//        /// </summary>
//        public const int MaxNameLength = 128;

//        /// <summary>
//        /// Maximum length of the <see cref="Value"/> property.
//        /// </summary>
//        public const int MaxValueLength = 2000;

//        /// <summary>
//        /// Feature name.
//        /// </summary>
//        [Required]
//        [MaxLength(MaxNameLength)]
//        public virtual string Name { get; set; }

//        /// <summary>
//        /// Value.
//        /// </summary>
//        [Required(AllowEmptyStrings = true)]
//        [MaxLength(MaxValueLength)]
//        public virtual string Value { get; set; }
//        public DateTime? CreatedAt { get; set; }
//        public string CreatedById { get; set; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="FeatureSetting"/> class.
//        /// </summary>
//        protected FeatureSetting()
//        {

//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="FeatureSetting"/> class.
//        /// </summary>
//        /// <param name="name">Feature name.</param>
//        /// <param name="value">Feature value.</param>
//        protected FeatureSetting(string name, string value)
//        {
//            Name = name;
//            Value = value;
//        }
//    }
//}
