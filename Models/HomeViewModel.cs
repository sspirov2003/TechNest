namespace TechNestClean.ViewModels;

using TechNestClean.Models;
using System.Collections.Generic;

public class HomeViewModel
{
    public List<Product> FeaturedProducts { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
}
