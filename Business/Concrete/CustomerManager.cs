using Business.Abstract;
using Business.ValidationRules;
using Core.AspectMessages;
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
            var aspectResults = AspectRules.Check(Results.ValidationResult);
            if (aspectResults != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _customerDal.Add(customer);
            return new SuccessResult();
        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IResult DeleteCustomer(Customer customer)
        {
            var aspectResults = AspectRules.Check(Results.ValidationResult);
            if (aspectResults != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _customerDal.Delete(customer);
            return new SuccessResult();
        }

        public IDataResult<List<Customer>> GetAllCustomers()
        {
            var result = _customerDal.GetAll();
            return new SuccessDataResult<List<Customer>>(result);
        }

        public IDataResult<Customer> GetCustomerById(int customerId)
        {
            var result = _customerDal.Get(x => x.Id == customerId);
            return new SuccessDataResult<Customer>(result);
        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IResult UpdateCustomer(Customer customer)
        {
            var aspectResults = AspectRules.Check(Results.ValidationResult);
            if (aspectResults != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _customerDal.Update(customer);
            return new SuccessResult();
        }
    }
}
