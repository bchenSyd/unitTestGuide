using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace StoreApp.Models
{
    public class Category
    {
        [Key]
        public long Id { get; set; }

        public string CategoryCode { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }


    /*when user put a request like below
     {  id:20, //ignored
        name:'ip payments 2',
        Description:'hello',
        price:12.36,
        categoryid:1,  //ignored
        Category:{
        CategoryCode:'tool'}
    }
     
     * a new Product will be created along with a new Category in database; 
     * the categoryid is ignored (i.e. EF won't look up datebase first before it create)
     
     */
}