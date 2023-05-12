using Core.DataAccess.EntityFramework.EfEntityRepositoryBase;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCustomerDal : EfEntityRepositoryBase<Customer, RentACarContext>, ICustomerDal
    {
        public List<CustomerForListDto> ListAllCustomers()
        {
            using (var context = new RentACarContext())
            {
                var result = from customer in context.Customers join user in context.Users
                             on customer.UserId equals user.Id select new CustomerForListDto
                             {
                                 CustomerId = customer.Id,
                                 UserId = user.Id,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 Email = user.Email,
                             };

                return result.ToList();
            }
        }
    }
}
