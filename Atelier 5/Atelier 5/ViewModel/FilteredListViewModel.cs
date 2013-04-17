using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Atelier_5.ViewModel
{
    public class FilteredListViewModel : INotifyPropertyChanged
    {
        private int _selectedFilter = 0;
        private readonly string[] _filters;
        private Model.EntrepriseEntities _context;

        public FilteredListViewModel(Model.EntrepriseEntities context)
        {
            _context = context;
            _filters = "Tout le staff,10$ et moins,Nés en Janvier,Par âge croissant,Commandes françaises,Prix moyen par catégorie,Montant Total".Split(',');

        }
        public IEnumerable<object> FilteredList
        {
            get
            {
                switch (this._selectedFilter)
                {
                    case 0:
                        return from employee in _context.Employees
                               select new
                               {
                                   Prénom = employee.First_Name,
                                   Nom = employee.Last_Name
                               };
                    case 1:
                        return from product in _context.Products
                               where product.Unit_Price < 10.0m
                               select new
                               {
                                   Produit = product.Product_Name,
                                   Prix = product.Unit_Price
                               };
                    case 2:
                        return from employee in _context.Employees
                               where employee.Birth_Date.HasValue == true
                               where employee.Birth_Date.Value.Month == 01
                               select new
                               {
                                   Prénom = employee.First_Name,
                                   Nom = employee.Last_Name
                               };
                    case 3:
                        return from employee in _context.Employees
                               let age = DateTime.Now.Year - employee.Birth_Date.Value.Year
                               orderby age
                               select new
                               {
                                   Prénom = employee.First_Name,
                                   Nom = employee.Last_Name,
                                   age

                               };
                    case 4:
                        return from customers in _context.Customers
                               where customers.Country == "France"
                               select new
                               {
                                   Nom = customers.Contact_Name,
                                   Total = customers.Orders.Count
                                    
                               };
                  
                   case 5:
                        return from product in _context.Products
                            group product by product.Category.Category_Name into grpCat
                            select new
                            {
                                 Category = grpCat.Key,
                                 Prix = grpCat.Average( product => product.Unit_Price)
                            
                            };
                    case 6:
                        return _context.Orders;
                           
                    default:
                        return new string[] {
                            "(Not implemented filter)"
                        };
                }
            }
        }
        public IEnumerable<String> Filters
        {
            get { return _filters; }
        }
        public int SelectedFilter
        {
            get { return this._selectedFilter; }
            set
            {
                this._selectedFilter = value;
                if (PropertyChanged != null)
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("FilteredList")
                    );
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
