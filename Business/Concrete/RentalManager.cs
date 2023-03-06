using Business.Abstract;
using Business.ValidationRules;
using Core.AspectMessages;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.AspectResults;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;

        public RentalManager(IRentalDal rentalDal)
        {
            _rentalDal = rentalDal;
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult AddRental(Rental rental)
        {
            var aspectResult = AspectRules.Check(Results.ValidationResult);

            if (aspectResult != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }

            var businessResult = BusinessRules.Run(ChecKIfCarAvailable(rental.CarId));

            if (businessResult != null)
            {
                return new ErrorResult();
            }
            _rentalDal.Add(rental);
            return new SuccessResult();
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult DeleteRental(Rental rental)
        {
            var aspectResult = AspectRules.Check(Results.ValidationResult);

            if (aspectResult != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _rentalDal.Delete(rental);
            return new SuccessResult();
        }

        public IDataResult<List<Rental>> GetAllRentals()
        {
            var result = _rentalDal.GetAll();
            return new SuccessDataResult<List<Rental>>(result);
        }

        public IDataResult<Rental> GetRentalById(int rentalId)
        {
            var result = _rentalDal.Get(x => x.Id == rentalId);
            return new SuccessDataResult<Rental>(result);
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult UpdateRental(Rental rental)
        {
            var aspectResult = AspectRules.Check(Results.ValidationResult);

            if (aspectResult != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _rentalDal.Update(rental);
            return new SuccessResult();
        }

        private IResult ChecKIfCarAvailable(int carId)
        {
            var result = _rentalDal.GetAll(x => x.CarId == carId);
            foreach (var record in result)
            {
                if (record.ReturnDate == null)
                {
                    return new ErrorResult();
                }
            }
            return new SuccessResult();
        }
    }
}
