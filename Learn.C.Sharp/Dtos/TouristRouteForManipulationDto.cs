﻿using Learn.C.Sharp.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Learn.C.Sharp.Dtos
{
    [TouristRouteTitleMustBeDifferentFromDescriptionAttribute]
    public abstract class TouristRouteForManipulationDto // IValidatableObject
    {
        [Required(ErrorMessage = "title bukeweikong")]// 报错来自于DTO而不是数据库，更安全
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public virtual string Description { get; set; }
        //public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public double? DiscountPresent { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public string Features { get; set; }
        public string Fees { get; set; }
        public string Notes { get; set; }
        public double Rating { get; set; }
        public string? TravelDays { get; set; }
        public string? TripType { get; set; }
        public string? DepartureCity { get; set; }
        public ICollection<TouristRoutePictureFroCreationDto> TouristRoutePictures { get; set; }
      = new List<TouristRoutePictureFroCreationDto>();

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult("not same", new[] { "TouristRouteForCreationDto" });
        //    }
        //}
    }
}
