using Business.Abstract;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.AspectResults;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CustomerManager : ICustomerService
    {

        ICustomerDal _customerDal;

        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IResult AddCustomer(Customer customer)
        {
            _customerDal.Add(customer);
            return new SuccessResult(Messages.CustomerAddedSuccessfully);
        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IResult DeleteCustomer(Customer customer)
        {
            _customerDal.Delete(customer);
            return new SuccessResult(Messages.CustomerDeletedSuccessfully);
        }

        public IDataResult<List<Customer>> GetAllCustomers()
        {
            var result = _customerDal.GetAll();
            return new SuccessDataResult<List<Customer>>(result, Messages.CustomersListedSuccessfully);
        }

        public IDataResult<Customer> GetCustomerById(int customerId)
        {
            var result = _customerDal.Get(x => x.Id == customerId);
            return new SuccessDataResult<Customer>(result, Messages.CustomerListedSuccessfully);
        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IResult UpdateCustomer(Customer customer)
        {
            _customerDal.Update(customer);
            return new SuccessResult(Messages.CustomerUpdatedSuccessfully);
        }
    }
}
