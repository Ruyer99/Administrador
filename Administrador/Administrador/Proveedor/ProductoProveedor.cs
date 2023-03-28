using Administrador.Models;
using Administrador.Views;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Administrador.Proveedor
{
    class ProductoProveedor
    {
        private string WebApiKey = "AIzaSyA33uUzHinDOOitFq-WNed3dlctMJJmjyk";
        public static FirebaseClient firebaseClient = new FirebaseClient("https://proyectosupermercado-728d4-default-rtdb.firebaseio.com/");
        public static FirebaseStorage firebaseStorage = new FirebaseStorage("proyectosupermercado-728d4.appspot.com");
        
        public static async Task<List<Producto>> GetAllProducts()
        {
            try
            {
                var productsList = (await firebaseClient
                .Child("Products")
                .OnceAsync<Producto>()).Select(item =>
                new Products
                {
                    Id = item.Key,
                    Name = item.Object.Name,
                    Price = item.Object.Price,
                    Category = item.Object.Category,
                    Description = item.Object.Description,
                    Image = item.Object.Image,
                }).ToList();
                return productsList;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }
        
        public static async Task<bool> AddProduct(string name, string price, string description, string category, string image)
        {
            try
            {
                await firebaseClient
                .Child("Products")
                .PostAsync(new Producto() { Name = name, Price = price, Description = description, Category = category, Image = image });
                return true;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", e.Message, "OK");
                return false;
            }
        }
        
        public static async Task<List<Producto>> GetAllByCategory(string category)
        {
            return (await firebaseClient.Child(nameof(Producto)).OnceAsync<Producto>()).Select(item => new Producto
            {
                Id = item.Key,
                Name = item.Object.Name,
                Price = item.Object.Price,
                Category = item.Object.Category,
                Description = item.Object.Description,
                Image = item.Object.Image,
            }).Where(c => c.Category.ToLower().Contains(category.ToLower())).ToList();
        }


       
        public async void CreateNewProduct(string name, string price, string description, string category, string image)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(price) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(image))
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Se requiere llenar todos los campos del formulario.", "OK");
            }
            else
            {
                try
                {
                    await AddProduct(name, price, description, category, image);
                }
                catch (Exception ex)
                {
                }
            }
        }

       

        public static async Task<bool> DeleteProduct(string id)
        {
            await firebaseClient.Child(nameof(Producto) + "/" + id).DeleteAsync();
            return true;
        }

       
        public static async Task<bool> UpdateProduct(Producto product)
        {
            await firebaseClient.Child(nameof(Producto) + "/" + product.Id).PutAsync(JsonConvert.SerializeObject(product));
            return true;
        }


       

        public static async Task<string> SaveImage(Stream image, string filename)
        {
            var img = await firebaseStorage.Child("ProductsImages").Child(filename).PutAsync(image);
            return img;
        }
      

        public static async Task<bool> DeleteImage(string filename)
        {
            await firebaseStorage.Child("ProductsImages").Child(filename).DeleteAsync();
            return true;
        }
    }
}


