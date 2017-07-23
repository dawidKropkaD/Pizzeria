using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pizzeria.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Models
{
    public static class SeedData
    {
        public static void InitializeMenu(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            if (context.Menu.Any())
            {
                return;
            }

            context.Menu.AddRange(
            #region Pizza
                new Menu
                {
                    ProductName = "Margherita",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano",
                    Size = 30,
                    Price = 21.04M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Margherita",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano",
                    Size = 45,
                    Price = 28.40M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Fungi",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Pieczarki, Oregano",
                    Size = 30,
                    Price = 22.64M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Fungi",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Pieczarki, Oregano",
                    Size = 45,
                    Price = 29.20M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Salami",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano, Salami",
                    Size = 30,
                    Price = 22.64M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Salami",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano, Salami",
                    Size = 45,
                    Price = 31.60M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Rustica",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano, Boczek",
                    Size = 30,
                    Price = 22.64M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Rustica",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano, Boczek",
                    Size = 45,
                    Price = 31.60M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Wegetariana",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Pieczarki, Oregano, Papryka, Kukurydza",
                    Size = 30,
                    Price = 22.64M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Wegetariana",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Pieczarki, Oregano, Papryka, Kukurydza",
                    Size = 45,
                    Price = 31.60M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Amore",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano, Kukurydza, Kurczak grillowany, Ananas",
                    Size = 30,
                    Price = 23.12M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Amore",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano, Kukurydza, Kurczak grillowany, Ananas",
                    Size = 45,
                    Price = 33.36M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Hawajska",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano, Szynka, Ananas, Brzoskwinie",
                    Size = 30,
                    Price = 23.12M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Hawajska",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano, Szynka, Ananas, Brzoskwinie",
                    Size = 45,
                    Price = 33.36M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Wiejska",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano, Cebula, Kiełbasa, Ogórek kiszony",
                    Size = 30,
                    Price = 23.12M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Wiejska",
                    Category = "Pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano, Cebula, Kiełbasa, Ogórek kiszony",
                    Size = 45,
                    Price = 33.36M,
                    IsInLocal = true,
                    IsOnline = true
                },
            #endregion

            #region Sos
                new Menu
                {
                    ProductName = "Sos czosnkowy",
                    Category = "Sos",
                    Weight = 80,
                    Price = 1.90M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Sos pomidorowy pikantny",
                    Category = "Sos",
                    Weight = 80,
                    Price = 1.90M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Sos pomidorowy łagodny",
                    Category = "Sos",
                    Weight = 80,
                    Price = 1.90M,
                    IsInLocal = true,
                    IsOnline = true
                },
            #endregion

            #region Sałatki
                //SAŁATKI
                new Menu
                {
                    ProductName = "Sałatka z tuńczykiem",
                    Category = "Sałatka",
                    Components = "Sałata, pomidor, kapusta pekińska, ogórek kiszony, fasola czerwona, kukurydza, tuńczyk, sos jogurtowy",
                    Price = 13.90M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Sałatka amerykańska",
                    Category = "Sałatka",
                    Components = "Sałata, pomidor, kapusta pekińska, ananas, brzoskwinia, kurczak grilowany, rodzynki, sos gyros",
                    Price = 13.90M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Sałatka grecka",
                    Category = "Sałatka",
                    Components = "Kapusta pekińska, sałata lodowa, pomidor, cebula czerwona, ogórek świezy, ser feta, oliwki czarne, oliwki zielone, sos grecki",
                    Price = 13.90M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Sałatka z łososiem",
                    Category = "Sałatka",
                    Components = "Sałata lodowa, łosoś wędzony, pomidor koktajlowy, cebula czerwona, cytryna, kapary, koperek, oliwa",
                    Price = 15.90M,
                    IsInLocal = true,
                    IsOnline = true
                },
            #endregion

            #region Napoje bezalkoholowe
                new Menu
                {
                    ProductName = "Czekolada na gorąco",
                    Category = "Napoje bezalkoholowe",
                    SubCategory = "Czekolada",
                    Price = 8M,
                    IsInLocal = true,
                    IsOnline = false
                },

                new Menu
                {
                    ProductName = "Mirinda 0,2l",
                    Category = "Napoje bezalkoholowe",
                    SubCategory = "Pepsi/7Up/Mirinda",
                    Price = 3.20M,
                    IsInLocal = true,
                    IsOnline = false
                },

                new Menu
                {
                    ProductName = "7up 0,2l",
                    Category = "Napoje bezalkoholowe",
                    SubCategory = "Pepsi/7Up/Mirinda",
                    Price = 3.20M,
                    IsInLocal = true,
                    IsOnline = false
                },

                new Menu
                {
                    ProductName = "Pepsi 0,2l",
                    Category = "Napoje bezalkoholowe",
                    SubCategory = "Pepsi/7Up/Mirinda",
                    Price = 3.20M,
                    IsInLocal = true,
                    IsOnline = false
                },

                new Menu
                {
                    ProductName = "Rockstar",
                    Category = "Napoje bezalkoholowe",
                    SubCategory = "Pepsi/7Up/Mirinda",
                    Price = 4.50M,
                    IsInLocal = true,
                    IsOnline = false
                },

                new Menu
                {
                    ProductName = "Pepsi 0.5l",
                    Category = "Napoje bezalkoholowe",
                    SubCategory = "Pepsi/7Up/Mirinda",
                    Price = 5.50M,
                    IsInLocal = true,
                    IsOnline = false
                },

                new Menu
                {
                    ProductName = "Mirinda 0.5l",
                    Category = "Napoje bezalkoholowe",
                    SubCategory = "Pepsi/7Up/Mirinda",
                    Price = 5.50M,
                    IsInLocal = true,
                    IsOnline = false
                },

                new Menu
                {
                    ProductName = "Woda mineralna gazowana 0,5l",
                    Category = "Napoje bezalkoholowe",
                    SubCategory = "Woda mineralna",
                    Price = 3.90M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Woda mineralna niegazowana 0,5l",
                    Category = "Napoje bezalkoholowe",
                    SubCategory = "Woda mineralna",
                    Price = 3.90M,
                    IsInLocal = true,
                    IsOnline = true
                },

                new Menu
                {
                    ProductName = "Kawa czarna",
                    Category = "Napoje bezalkoholowe",
                    SubCategory = "Kawa",
                    Price = 4M,
                    IsInLocal = true,
                    IsOnline = false
                },

                new Menu
                {
                    ProductName = "Kawa espresso",
                    Category = "Napoje bezalkoholowe",
                    SubCategory = "Kawa",
                    Price = 4M,
                    IsInLocal = true,
                    IsOnline = false
                },

                new Menu
                {
                    ProductName = "Kawa capuccino",
                    Category = "Napoje bezalkoholowe",
                    SubCategory = "Kawa",
                    Price = 6M,
                    IsInLocal = true,
                    IsOnline = false
                },
            #endregion

            #region Mini pizze
                new Menu
                {
                    ProductName = "Margherita",
                    Category = "Mini pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano",
                    Price = 6.50M,
                    IsInLocal = true,
                    IsOnline = false
                },

                new Menu
                {
                    ProductName = "Fungi",
                    Category = "Mini pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano, Pieczarki",
                    Price = 8.40M,
                    IsInLocal = true,
                    IsOnline = false
                },

                new Menu
                {
                    ProductName = "Rafaello",
                    Category = "Mini pizza",
                    Components = "Sos pomidorowy łagodny, Ser mozarella, Oregano, Pieczarki, Szynka, Kurczak grillowany",
                    Price = 9.40M,
                    IsInLocal = true,
                    IsOnline = false
                }
                #endregion
            );

            context.SaveChanges();
        }
    }
}
