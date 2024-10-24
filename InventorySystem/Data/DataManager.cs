using InventorySystem.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InventorySystem.Data
{
    public class DataManager
    {
        public static async Task RolesSeeding(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();

            if(!await roleManager.RoleExistsAsync(RolesType.Role_Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(RolesType.Role_Admin));
            }

            if(!await roleManager.RoleExistsAsync(RolesType.Role_Employee))
            {
                await roleManager.CreateAsync(new IdentityRole(RolesType.Role_Employee));
            }
        }

        public static async Task AdminSeeding(IServiceProvider serviceProvider)
        {

            var configurationService = serviceProvider.GetRequiredService<IConfiguration>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var email = configurationService["AdminUser:Email"];
            var password = configurationService["AdminUser:Password"];

            if(email != null && password != null)
            {   
                if(await userManager.FindByEmailAsync(email) is null) 
                {
                    var user = new ApplicationUser
                    {
                        UserName = "Admin",
                        Email = email,
                        EmailConfirmed = true,
                        FirstName = "Admin",
                        LastName = string.Empty,
					    Employee = new Employee { Name = "Admin", IsAdmin = true, Status= true},              
                    };

                    var result =await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, RolesType.Role_Admin);           
                    }
                }
            }
        }

        public static T? DeepCopySerialization<T>(T obj)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(obj, options);
            return JsonSerializer.Deserialize<T>(json, options);
        }


        public static bool CompareObjects<T>(T obj1, T obj2, string[]? properties = null) 
        {
			//var options = new JsonSerializerOptions
			//{
			//    ReferenceHandler = ReferenceHandler.Preserve,
			//    WriteIndented = true
			//};

			//return  JsonSerializer.Serialize(obj1, options) ==  JsonSerializer.Serialize(obj2, options);

            if (ReferenceEquals(obj1, obj2)) 
                return true;

            if (obj1 == null || obj2 == null)
                return false;

            if (properties is not null)
            {
                foreach (var property in properties)
                {
                    var prop = typeof(T).GetProperty(property);
                    if (prop != null && !ComparePropertyValues(prop, obj1, obj2, properties))
                    {
                        return false;
                    }
                }
            }
            else{
                foreach(var prop in typeof(T).GetProperties())
                {
                    if (!ComparePropertyValues(prop, obj1, obj2, properties))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public static void UpdateObjectValuesForSpecificProp<T>(T currentObj, T newObj, string[] propertiesName)
        {
            foreach(var propertyName in propertiesName)
            {
                //var prop = typeof(T).GetProperty(propertyName);
                //if(prop != null && !ComparePropertyValues(prop, currentObj, newObj, propertiesName))
                //{
                //    var currentValue = prop.GetValue(currentObj);
                //    var newValue = prop.GetValue(newObj);

                //    if(!currentValue.Equals(newValue) && newValue is not null)
                //    {
                //        prop.SetValue(currentObj, newValue);
                //    }
                //}

                var prop = typeof(T).GetProperty(propertyName);
                if(prop != null && !ComparePropertyValues(prop, currentObj, newObj))
                {
                    var newValue = prop.GetValue(newObj);

                    prop.SetValue(currentObj, newValue);
                }
            }
        }


        private static bool ComparePropertyValues<T>(PropertyInfo propInfo, T obj1, T obj2, string[]? properties = null)
        {
            var value1 = propInfo.GetValue(obj1);
            var value2 = propInfo.GetValue(obj2);

            if (propInfo.PropertyType.IsPrimitive || propInfo.PropertyType == typeof(string))
            {
                return Equals(value1, value2);
            }
    
            return CompareObjects(value1, value2, properties);
        }
    }
}
