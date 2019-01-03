﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestShop.Models.Data;

namespace TestShop.Models.ViewModels.Shop
{
    public class ProductVM
    {
        public ProductVM()
        {

        }

        public ProductVM (ProductDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Description = row.Description;
            Price = row.Price;
            CategoryName = row.CategoryName;
            CategoryId = row.CategoryId;
            ImageName = row.ImageName;
        }

        public int Id { get; set; }
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required]
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Cena")]
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string ImageName { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}