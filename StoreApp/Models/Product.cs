using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace StoreApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public long CategoryId { get; set; }


        //ERROR:Self referencing loop detected with type 'StoreApp.Models.Product'
        //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling 
        //= Newtonsoft.Json.ReferenceLoopHandling.Ignore;  OR:
        //[JsonIgnore]
        //[IgnoreDataMember]    JsonIgnore is for JSON.NET and IgnoreDataMember is for XmlDCSerializer. 
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

    }


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