﻿using System.Web.Mvc;
using TestShop.Models.Data;

namespace TestShop.Models.ViewModels.Pages
{
    public class SidebarVM
    {
        public SidebarVM()
        {

        }

        public SidebarVM(SidebarDTO row)
        {
            Id = row.Id;
            Body = row.Body;
        }

        public int Id { get; set; }

        [AllowHtml]
        public string Body { get; set; }
    }
}