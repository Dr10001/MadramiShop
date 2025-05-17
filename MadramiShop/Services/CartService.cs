using MadramiShop.Data;
using MadramiShop.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MadramiShop.Services
{
    public class CartService
    {
        public List<CartModel> Items { get; private set; } = [];
        public int Count { get; private set; }
        public string CountDisplay => Count < 100 ? $"{Count}" : "99+";

        public event Action? CartCountChanged;
        public decimal TotalAmount => Items.Sum(i => i.Amount);
        public void IncreaseQuantity(ProductDto product)
        {
            var cartItem = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (cartItem is null)
            {
                cartItem = CartModel.FromDto(product);
                Items.Add(cartItem);
            }
            else 
            {
                cartItem.Quantity = product.Quantity;
            }
            NotifyCountChanged();
        }
        public void DecreaseQuantity(ProductDto product)
        {
            var cartItem = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (cartItem is null)
            {
                return;
            }
            else
            {
                cartItem.Quantity = product.Quantity;
                if(cartItem.Quantity == 0)
                {
                    Items.Remove(cartItem);
                }
                    
            }
            NotifyCountChanged();
        }

        public void IncreaseCartItemQuantity(CartModel cartItem)
        {
            cartItem.Quantity++;
            NotifyCountChanged();
        }
        public void DecreaseCartItemQuantity(CartModel cartItem)
        {
            cartItem.Quantity--;
            if (cartItem.Quantity == 0)
            {
                Items.Remove(cartItem);
            }
            NotifyCountChanged();
        }

        public async Task RemoveItem(CartModel cartItem)
        {
            Items.Remove(cartItem);
            NotifyCountChanged();
           // await MauiInterop.ToastAsync("Item removed");
        }

        public async Task ClearCartAsync()
        {
            if (Items.Count == 0)
                return;

            //if(await App.Current.Windows[0].Page.DisplayAlert("Confirm?", "Are you sure You want to clear the cart?", "Yes", "No"))
            if (await MauiInterop.ConfirmAsync("Are you sure You want to clear the cart?", "Confirm?"))
            {

                Items.Clear();
                NotifyCountChanged();
                //await MauiInterop.ToastAsync("Cart cleared"))
            }
        }
        private void NotifyCountChanged()
        {
            Count = Items.Sum(i => i.Quantity);
        }
    }
}
